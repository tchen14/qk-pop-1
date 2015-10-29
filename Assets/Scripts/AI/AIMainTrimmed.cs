using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float attackDistance = 3;                    //!<The distance the NPC stands away from the target and attacks*
    public float aggressionLimit = 100;			        //!<The aggression level of the attacker
    public float suspicionLimit = 150;                  //!<The suspicon level of the attacker
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

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
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
        #region state 
        if (hp > 0)
        {
            if (dazed == false)
            {
                mesh.SetDestination(navPoint);
                print(navPoint);
                #region attack
                if (target != null)
                {
                    if (attackDistance >= Vector3.Distance(transform.position, target.transform.position))
                    {
                        if (target.name == "shadowPlayer(Clone)")
                        {
                            InvestigatePlayerPos(10f);
                        }
                        target = null;
                    }
                }
                #endregion

                #region sight
                if (aggressive == true)
                {
                    StartCoroutine("CheckForTargets");
                }
                #endregion
            }
            else
            {

            }
        }
        else
        {
            Pause();
        }

        if (panic == true)
        {
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
        if (aggressive == true && seesTarget == true)
        {
            if (alert == true)
            {
                aggressionLevel = aggressionLimit;
            }
            else
            {
                if (suspicionLevel >= suspicionLimit)
                {
                    suspicious = true;
                    if (aggressionLevel >= aggressionLimit)
                    {
                        attacking = true;
                        ChangeNavPoint(target.name, target.transform.position);
                        SetSpeed(runSpeed);
                    }
                    else
                    {
                        aggressionLevel += 1;
                    }
                }
                else
                {
                    suspicionLevel += 1;
                }
            }
        }
        else
        {
            if (attacking == true && searching == false)
            {
                target = (GameObject)Instantiate(PlayerLastPos, target.transform.position, Quaternion.identity);
                searching = true;
            }
            if (suspicionLevel > 0)
            {
                StartCoroutine("Decrementsuspicion");
            }
            if (aggressionLevel > 0)
            {
                StartCoroutine("Decrementaggression");
            }
        }


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
        #endregion

        #region path
        if ((Vector3.Distance(transform.position, navPoint) < 3) && (target == null))
        {
			Debug.Log ("arrived.");
            nextCheckpoint();
        }
        #endregion
    }

    public IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    public IEnumerator Decrementaggression()
    {
        yield return new WaitForSeconds(3f);
        if (aggressionLevel > 0)
        {
            aggressionLevel -= 1;
        }
        if (aggressionLevel == 0)
        {
            searching = false;
            attacking = false;
            ChangeNavPoint(checkpointsArray[CheckpointCount].name, checkpointsArray[CheckpointCount].transform.position);
        }
    }

    public IEnumerator Decrementsuspicion()
    {
        yield return new WaitForSeconds(3f);
        if (suspicionLevel > 0)
        {
            suspicionLevel -= 1;

        }
        if (suspicionLevel == 0)
        {
            suspicious = false;
        }
    }

    #region dazed
    public IEnumerator Dazed(float dazeTime)
    {
        dazed = true;
        Pause();
        yield return new WaitForSeconds(dazeTime);
        dazed = false;
        Resume();

    }


    #endregion

    #region distracted

    public void NoiseHeard(GameObject soundPos)
    {
        if (seesTarget == false)
        {
            ChangeNavPoint(soundPos.name, soundPos.transform.position);
            if ((Vector3.Distance(transform.position, navPoint) < 10) && (target == null))
            {
                InvestigateSound(5f);
            }
        }
    }

    #endregion

    #region Move functions
    public void ChangeNavPoint(string N, Vector3 T)
    {
		Debug.Log ("Changing to:" + T.ToString());
        navCheck = N;
        navPoint = T;
    }
    public void SetSpeed(float Spd)
    {
        mesh.speed = Spd;
    }
    public void Pause()
    {
        mesh.Stop();
    }
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
            //check for and if need be.
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
                                if (target != null && target.name == "shadowPlayer(Clone)")
                                {
                                    Destroy(target);
                                    CheckForTargets();
                                }
                                target = hit.collider.gameObject;
                                searching = false;
                                seesTarget = true;
                            }
                            else
                            {
                                seesTarget = false;
                            }
                        }
                        else
                        {
                            seesTarget = false;
                        }
                    }
                    else
                    {
                        seesTarget = false;
                    }
                }
                else
                {
                    seesTarget = false;
                }
            }
        }
        else
        {
        }
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
        ChangeNavPoint(checkpointsArray[CheckpointCount].name, checkpointsArray[CheckpointCount].transform.position);

    }

    public IEnumerator InvestigatePlayerPos(float lookTime)
    {
        yield return new WaitForSeconds(lookTime);
        ChangeNavPoint(checkpointsArray[CheckpointCount].name, checkpointsArray[CheckpointCount].transform.position);

    }

    public void nextCheckpoint()

    {
        
        #region LoopPath
        if (PathwayCount <= Pathways.Count - 1)
        {
			Debug.Log ("next chackpoint");
            Path = Pathways[PathwayCount];
            //AINavPoints CheckpointScript = Path.GetComponent<AINavPoints>();
			AIPath CheckpointScript = Path.GetComponent<AIPath>();
			Debug.Log(PathType[PathwayCount]);

            switch (PathType[PathwayCount])
            { 
                
                case 0:
                    if (CheckpointCount <= CheckpointScript.getPoints().Count - 1)
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
					if (CheckpointCount <= CheckpointScript.getPoints().Count - 1)
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
                            print(CheckpointCount);
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
