using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]	//Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]
/*! 
 *	This code is the main ai controller. The class is segmented into the following regions: attack, movement, state, sight, and health
 */
[EventVisible]
public class AIMainTrimmed : MonoBehaviour
{
	//Editor variables
	public int current_preset = 0;

    //Variables Editable

    public int hp = 100;                                //!<Health of the NPC
	public float sightDistance = 45;                    //!<The distance the NPC is capable of seeing
	public float sightAngle = 45;                       //!<The max angle of the cone of vision
	public float speed = 4;                             //!<Casual Speed of the NPC
	public float runSpeed = 6;                          //!<Scared, Charging, Aggressive Speed of the NPC*
	public string[] seekTag = { "Player" };             //!<The enemy Tag of this NPC
	public float attackDistance = 2;                    //!<The distance the NPC stands away from the target and attacks*
    public float aggressionLimit = 10;			        //!<The aggression level of the attacker
    public float suspicionLimit = 10;                  //!<The suspicon level of the attacker
    private GameObject startPoint = null;                //!<Sets the first navPoint, defaults to stationary
    private GameObject endPoint = null;                  //!<Sets the last navPoint of the AI where they will stop.
	public string panicPoints = "PanicPoints";          //!<The object name of the vector keeper of related panic points for the AI
    public bool aggressive = false;					    //!<If the NPC will attack the player
    private GameObject PlayerLastPos = null;             //visual representation of where the AI last remembers seeing the player before they went of LoS
    private bool searching = false;                      //if the AI is currently looking for the player at its shadow
    private bool checking = false;


    //Variables Controllers

    [ReadOnly]
    public bool seesTarget = false;                   //!<If the Player has been spotted
    [ReadOnly]
    public GameObject target = null;						//!<The transform of the player
    [ReadOnly]
    public GameObject temptarget = null;                     //!Temp target for the shadowPlayer
    [ReadOnly]
    public bool attacking = false;                    //!<If the AI is attacking
    [ReadOnly]
    public bool panic = false;					//!<If the AI is panicking
    [ReadOnly]
    public int CheckpointCount = 0;                        //! Int for tracking the checkpoint the AI is on
    private Vector3 panicTarget = new Vector3(0, 0, 0);	//!<Target of AI panic
    private float aggressionLevel = 0;						//!<The current awareness of the NPC to the Player
    private float suspicionLevel = 0;						//!<The current suspicion of the NPC to the Player	
    private bool suspicious = false;					//!<If the AI is suspicous
    public bool alert = false;					//!<If the AI is alert
    private bool unconsious = false;                    //!<If the AI is unconsious
    public bool dazed = false;                    //!<If the AI is dazed
    public bool noiseHeard = false;                   //! bool to set the enemy as distracted for sound throw.




    //Movement variables

    private NavMeshAgent mesh = null;						//!<Contains the component to use the navmesh
    public GameObject[] checkpointsArray;                               //!Array to hold all the checkpoints for the AI to move to
    [ReadOnly]
    public string navCheck = null;                        //!<The name of the current checkpoint
    [ReadOnly]
    public Vector3 navPoint = new Vector3(0, 0, 0);   //!<Contains the point to move in the navmesh
    public List<Vector3> AiCheckpoints;
    public List<GameObject> Pathways;
	public List<int> PathType;
	public List<int> nofLoops;
	public List<bool> infinite;
    private bool looping;
    private int LoopCount = 1;
    private bool BacknForth;
    private bool back = false;
    public int PathwayListLength;
    public int AiCheckpointsLength;
    public GameObject Path;
    public int PathwayCount = 0;

    //Sight variables

    private RaycastHit hit;                                         //!<Takes information from RayCast
    private GameObject[] viableTargets;                                 //!<All the available targets for the AI

    //Coroutine controls
    private bool CheckForTargetsRunning;
    private bool DecrementsuspicionRunning;
    private bool DecrementaggressionRunning;
    private bool IncrementaggressionRunning;
    private bool IncrementSuspicionRunning;

    void Start()
    {
        PlayerLastPos = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/shadowPlayer.prefab", typeof(GameObject)) as GameObject;
        if (aggressionLevel >= aggressionLimit)
            GetComponent<Rigidbody>().isKinematic = true;
        mesh = GetComponent<NavMeshAgent>();
        #region StartMoving
        ChangeNavPoint(this.name, this.transform.position);
        SetSpeed(speed);
        #endregion
    }



    void FixedUpdate()
    {
        Debug.Log(target);
        #region state 

        //As long as the AI is not unonsious or dazed it wil continue moving to its destination. 
        if (hp > 0)
        {
            if (dazed == false)
            {
                mesh.SetDestination(navPoint);
                #region attack
                //if the AI has a target it will try to attack that target. 
                if (target != null)
                {
                    //if the target is in attack distance it will attack the target. If it is the shadow player destroy it. If the target is the player reset the level.
                    if ((attackDistance >= Vector3.Distance(transform.position, target.transform.position) && (attacking == true)))
                    {
                        if (target.name == "shadowPlayer(Clone)")
                        {
                            Destroy(target);
                            searching = false;
                            InvestigatePlayerPos(5f);
                        }
                        if (target.name == "_Player")
                        {
                            Time.timeScale = 0;
                        }
                        target = null;
                    }
                }
                #endregion
                //if the AI is aggressive (an enemy) look for viable targets.
                #region sight
                if (aggressive == true)
                {
                    if (CheckForTargetsRunning == false)
                    {
                        StartCoroutine("CheckForTargets");
                        CheckForTargetsRunning = true;
                    }
                    
                    if (seesTarget == true)
                    {
                        //if the AI is alert it will start chasing the player immediately upon seeing them.
                        if (alert == true)
                        {
                            aggressionLevel = aggressionLimit;
                        }
                        //if the AI is not alert it will build suspicion up to it's limit then build aggression to its limit before chasing the player down.
                        else
                        {
                            Debug.Log(suspicionLevel);
                            if (suspicionLevel >= suspicionLimit)
                            {
                                suspicious = true;
                                //if the AI reaches max aggression it will start chasing the player, defined as target for the AI. Also set the speed of the AI to it's run speeed
                                if (aggressionLevel >= aggressionLimit)
                                {
                                    attacking = true;
                                    ChangeNavPoint(target.name, target.transform.position);
                                    SetSpeed(runSpeed);
                                }
                                else
                                {
                                    //the AI sees the player and has max suspicion, its now getting aggressive
                                    if (IncrementaggressionRunning == false)
                                    {
                                   
                                        Incrementaggression();
                                        IncrementaggressionRunning = true;
                                    }
                                }
                            }
                            else
                            {
                                //the AI is aware of the player and is getting suspicious
                                if (IncrementSuspicionRunning == false)
                                {
                                    
                                    IncrementSuspicion();
                                    IncrementSuspicionRunning = true;
                                }
                            }
                        }
                    }
                    //if the AI does not see the player/shadow player
                    else
                    {
                        //if the AI was chasing the player and lost sight of them, spawn a player shadow at the players last know location. Will only occur if the AI is not currently looking for a shadow player.
                        if (attacking == true && searching == false && target != null)
                        {
                            if (PlayerLastPos != null)
                            {
                                target = (GameObject)Instantiate(PlayerLastPos, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z), Quaternion.identity);
                            }
                            else
                            {
                                //shadowPlayer prefab not assigned to PlayerLastPos in inspector. Attempting to resolve
                                PlayerLastPos = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/shadowPlayer.prefab", typeof(GameObject)) as GameObject;
                                target = (GameObject)Instantiate(PlayerLastPos, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z), Quaternion.identity);
                            }
                            searching = true;
                           
                        }
                        //if the AIs suscpicion level is greater than 0 and it can't find the player decrease suspicion
                        if (suspicionLevel > 0)
                        {
                            if (DecrementaggressionRunning == false)
                            {
                                StartCoroutine("Decrementsuspicion");
                                DecrementaggressionRunning = true;
                            }
                         //if the AIs aggression level is greater than 0 and it can't find the player decrease aggression
                        }
                        if (aggressionLevel > 0)
                        {
                            if (DecrementsuspicionRunning == false)
                            {
                                StartCoroutine("Decrementaggression");
                                DecrementsuspicionRunning = true;
                            }
                        }
                    }

                    //change the color of the AI based on its current state. For demonstration and visualization only.
                    if (suspicious == true && attacking == true)
                    {
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        if (suspicious == true)
                        {
                            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                        }
                        else
                        {
                            gameObject.GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                }


                #endregion
                

                

                #region path
                //if the AI has reached its destination and is currently not attacking the player move to the next checkpoint
                if ((Vector3.Distance(transform.position, navPoint) < 3) && (attacking == false))
                {
                    nextCheckpoint();
                }
                #endregion

                //if the AI is panicing move to its panic point. Remains of old code not being used.
                if (panic == true)
                {
                    //if the AI is an enemy it can't enter a panic state.
                    if (aggressive == true)
                    {
                        panic = false;
                    }
                    else
                    {
                        ChangeNavPoint("panic", panicTarget);
                        SetSpeed(runSpeed);
                    }
                }
                #endregion
            }
            else
            {
                //AI is dazed
            }
        }
        else
        {
            //AI is unconsious and stops moving/reacting.
            Pause();
        }
    }

    public IEnumerator Incrementaggression()
    {
        yield return new WaitForSeconds(.5f);
        aggressionLevel += 1;
        IncrementaggressionRunning = false;
    }

    public IEnumerator IncrementSuspicion()
    {
        yield return new WaitForSeconds(.5f);
        suspicionLevel += 1;
        IncrementSuspicionRunning = false;
    }

    public IEnumerator Decrementaggression()
    {
        //if aggression is greater than 0 decrement it by 1. otherwise go back to normal walking pace, stop searching for the shadow player, stop attacking player/shadow player, and move back to checkpoint you were going to.
        yield return new WaitForSeconds(.5f);
        if (aggressionLevel > 0)
        {
            aggressionLevel -= 1;
        }
        else
        {
            SetSpeed(speed);
            foreach (GameObject enemy in viableTargets)
            {
                if (enemy.name == "shadowPlayer(Clone)")
                {
                    Destroy(target);
                }
                searching = false;
                attacking = false;
                Path = Pathways[PathwayCount];
                AIPath CheckpointScript = Path.GetComponent<AIPath>();
                string CheckpointCountString = CheckpointCount.ToString();
                ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount - 1]);
            }
        }
        DecrementaggressionRunning = false;
    }

    public IEnumerator Decrementsuspicion()
    {
        //if suspicion is greater than 0 decrement it by 1. Otherwise set suspicious to false.
        yield return new WaitForSeconds(.5f);
        if (suspicionLevel > 0)
        {
            suspicionLevel -= 1;
            Debug.Log(suspicionLevel);
        }
        else
        {
            suspicious = false;
        }
        DecrementsuspicionRunning = false;
    }

    //function to call to daze an AI. Call Dazed(float); replacing float with how long you want the AI to be dazed for.
    #region dazed
    public IEnumerator Dazed(float dazeTime)
    {
        //AI becomes dazed, no longer performing its normal functions, not moving on its path for a set time. After that time it resumes like normal.
        dazed = true;
        Pause();
        yield return new WaitForSeconds(dazeTime);
        dazed = false;
        Resume();

    }
    #endregion
    
    //function for Distracting the AI. Call NoiseHeard(GameObject); replacing GameObject with the object that emits the sound.
    #region distracted
    public void NoiseHeard(GameObject soundPos)
    {
        //if the AI does not currently see the player it will be go to the object that emited the sound.  
        if (seesTarget == false)
        {
            ChangeNavPoint(soundPos.name, soundPos.transform.position);
            //once the AI is near the sound source it will pause for a moment looking around
            if ((Vector3.Distance(transform.position, navPoint) < 5) && (target == null))
            {
                InvestigateSound(10f);
            }
        }
        else
        {
            //AI sees the player.
        }
    }
    #endregion

    
    #region Move functions
    //sets a new navigation point for the AI not on it's path. Needs a string and a path.
    public void ChangeNavPoint(string N, Vector3 T)
    {
        navCheck = N;
        navPoint = T;
    }
    //sets the speed of the AI. Needs a float. 
    public void SetSpeed(float Spd)
    {
        mesh.speed = Spd;
    }
    //stops the AI on the navmesh path
    public void Pause()
    {
        mesh.Stop();
    }
    //resumes the AI on the navmesh path.
    public void Resume()
    {
        mesh.Resume();
    }
    #endregion

    #region Sight Functions
    public IEnumerator CheckForTargets()
    {
        yield return new WaitForSeconds(.1f);
        GetTargets();
    }
    public void GetTargets()
    {
        List<GameObject> viableTargets = new List<GameObject>();
        for (int enemyType = 0; enemyType < seekTag.Length; enemyType++)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(seekTag[enemyType]);
            for (int searchEnemyType = 0; searchEnemyType < targets.Length; searchEnemyType++)
                viableTargets.Add(targets[searchEnemyType]);
        }

        if (viableTargets.Count != 0)
        {
            //checks for the player in the AI's line of sight.
            for (int searchTargets = 0; searchTargets < viableTargets.Count; searchTargets++)
            {
                if (Vector3.Angle(viableTargets[searchTargets].transform.position - transform.position, transform.forward) < sightAngle)
                {
                    if (Vector3.Distance(transform.position, viableTargets[searchTargets].transform.position) < sightDistance)
                    {
                        if (Physics.Raycast(transform.position, viableTargets[searchTargets].transform.position - transform.position, out hit))
                        {
                            if (hit.collider.tag == viableTargets[searchTargets].tag)
                            {
                                target = hit.collider.gameObject;
                                seesTarget = true;
                            }
                            else
                            {
                                //The object that the raycast is hitting isnt tagged as "Player"
                                seesTarget = false;
                            }
                        }
                        else
                        {
                            //raycast is returning null
                            seesTarget = false;
                        }
                    }
                    else
                    {
                        //target is too far from AI. Distance from AI, not angular.
                        seesTarget = false;
                    }
                }
                else
                {
                    //target out of angular vision of the AI
                    seesTarget = false;
                }
            }
        }
        else
        {
            //THere are no viable targets
            Debug.Log("No viable targets detected. Make sure Player is tagged as Player");
        }
        CheckForTargetsRunning = false;
    }
    #endregion

    #region health
    public int hurt(int damage)
    {
        int HP = hp;
        HP -= damage;
        return HP;
    }
    public int heal(int health)
    {
        int HP = hp;
        HP += health;
        return HP;
    }
    #endregion

    #region Recover Functions
    public IEnumerator statusRecover()
    {
        yield return new WaitForSeconds(10.0f);
        dazeRecover();
    }

    public void dazeRecover()
    {
        //dazed == false
        //whatever actions need to be performed after recovery
        //ie. high suspicion level, aggressive, look around? contine patrol?
    }
    #endregion

    public IEnumerator InvestigateSound(float lookTime)
    {
        yield return new WaitForSeconds(lookTime);
        Path = Pathways[PathwayCount];
        AIPath CheckpointScript = Path.GetComponent<AIPath>();
        string CheckpointCountString = CheckpointCount.ToString();
        ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount - 1]);

    }

    public IEnumerator InvestigatePlayerPos(float lookTime)
    {
        yield return new WaitForSeconds(lookTime);
        Path = Pathways[PathwayCount];
        AIPath CheckpointScript = Path.GetComponent<AIPath>();
        string CheckpointCountString = CheckpointCount.ToString();
        ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount - 1]);

    }

    public void nextCheckpoint()

    {
        
        #region LoopPath
        if (PathwayCount <= Pathways.Count - 1)
        {
            Path = Pathways[PathwayCount];
			AIPath CheckpointScript = Path.GetComponent<AIPath>();

            switch (PathType[PathwayCount])
            { 
                
                case 0:
                    if (CheckpointCount < CheckpointScript.getPoints().Count)
                    {
                        string CheckpointCountString = CheckpointCount.ToString();
						ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
					if (CheckpointCount != CheckpointScript.getPoints().Count)
                        {
                            CheckpointCount++;
                        }
                    }
                    else
                    {
                        if (PathwayCount != Pathways.Count - 1)
                        {
                            PathwayCount++;
                            CheckpointCount = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;

                case 1:
                    if (LoopCount <= nofLoops[PathwayCount])
                    {
					if (CheckpointCount < CheckpointScript.getPoints().Count)
                        {
                            string CheckpointCountString = CheckpointCount.ToString();
							ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
						if (CheckpointCount != CheckpointScript.getPoints().Count)
                            {
                                CheckpointCount++;
                            }
                        }
                        else
                        {
                            CheckpointCount = 0;

							if(!infinite[PathwayCount]){
                        	    LoopCount++;
							}
                        }
                    }
                    else
                    {
                        PathwayCount++;
                        CheckpointCount = 0;
                        LoopCount = 1;
                    }
                    break;

                case 2:
				if (LoopCount <= nofLoops[PathwayCount])
                    {
					if ((CheckpointCount < CheckpointScript.getPoints().Count) && (back == false))
                        {
                            string CheckpointCountString = CheckpointCount.ToString();
							ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
						if (CheckpointCount != CheckpointScript.getPoints().Count)
                            {
                                CheckpointCount++;
                            }
                        }
                        else
                        {
                            if (CheckpointCount > 0)
                            {
                                back = true;
                                CheckpointCount--;
                                string CheckpointCountString = CheckpointCount.ToString();
								ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);

                            }
                            else
                            {
                                back = false;

								if(!infinite[PathwayCount]){
									LoopCount++;
								}
                            }
                        }
                    }
                    else
                    {
                        PathwayCount++;
                        CheckpointCount = 0;
                        LoopCount = 1;
                    }
                    break;

            }
        }
        else
        {
            
        }
    }
    #endregion
}
