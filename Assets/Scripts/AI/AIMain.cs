using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

[RequireComponent(typeof(NavMeshAgent))]	//Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]       //Attaches a rigid body to the object
/*! 
    This code is the main ai controller. The class is segmented into the following regions: attack, movement, state, sight, and health

    The only things you need to worry about are the public variables.




 */
[EventVisible]
public class AIMain : MonoBehaviour
{
    public int current_preset = 0;

    //Variables Editable

    [ReadOnly]public int hp = 100;                               //!<Health of the NPC
    public float sightDistance = 45;                   //!<The distance the NPC is capable of seeing
    [ReadOnly]public float sightAngle = 45;                      //!<The max angle of the cone of vision
    [ReadOnly]public float speed = 4;                            //!<Casual Speed of the NPC
    [ReadOnly]public float runSpeed = 6;                         //!<Scared, Charging, Aggressive Speed of the NPC*
    [ReadOnly]public string[] seekTag = { "Player" };            //!<The enemy Tag of this NPC
    public float attackDistance = 3;                   //!<The distance the NPC stands away from the target and attacks*
    private GameObject PlayerLastPos = null;            //visual representation of where the AI last remembers seeing the player before they went of LoS
    private bool searching = false;                     //if the AI is currently looking for the player at its shadow
    public bool aggressive = false;					    //!<If the NPC will attack the player (True means the AI is an enemy false means its a normal NPC)
    public float aggressionLimit = 100;			        //!<The aggression level of the attacker (Changes how long it takes for the enemy to get aggressive)
    public float suspicionLimit = 150;                  //!<The suspicon level of the attacker (changes how long it akes for the enemey to get suspicous) 



    //Variables Controllers

    [ReadOnly]public bool seesTarget = false;           //!<If the Player has been spotted
    [ReadOnly]public GameObject target = null;			//!<The transform of the player
    [ReadOnly]public GameObject temptarget = null;      //!Temp target for the shadowPlayer
    [ReadOnly]public bool attacking = false;            //!<If the AI is attacking
    [ReadOnly]public bool panic = false;				//!<If the AI is panicking
    [ReadOnly]public int CheckpointCount = 0;           //! Int for tracking the checkpoint the AI is on
    [ReadOnly]public Vector3 panicTarget = new Vector3(0, 0, 0);	//!<Target of AI panic
    [ReadOnly]public float aggressionLevel = 0;                 //!<The current awareness of the NPC to the Player
    [ReadOnly]public float suspicionLevel = 0;					//!<The current suspicion of the NPC to the Player	
    [ReadOnly]public bool suspicious = false;					//!<If the AI is suspicous

    public bool alert = false;					        //!<If the AI is alert (change this to set the AI to chase the player as soon as it sees the player. False uses the suspicious/aggression method)
    public bool dazed = false;                          //!<If the AI is dazed (do not change, changed automatically in function call)




    //Movement variables

    [ReadOnly]public string navCheck = null;                    //!<The name of the current checkpoint
    [ReadOnly]public Vector3 navPoint = new Vector3(0, 0, 0);   //!<Contains the point to move in the navmesh
    private NavMeshAgent mesh = null;                           //!<Contains the component to use the navmesh
    [ReadOnly]public bool looping;                                       //!<bool to check for infinite loops on a path
    [ReadOnly]public int LoopCount = 1;                                  //!<loop counter for keeping track of the number of loops
    [ReadOnly] public bool back = false;                                  //!<Check for the AI to come back the way it came
    private bool pathing = true;                                //!<Check for if the AI is currently on its path or looking for something
    private int PathwayCount = 0;                                //!<Int to keep track of the current pathway

    public GameObject[] Pathways;                               //!<Array that holds the Paths for the AI. Set the size to the number of paths and put in each path, in order!
    public int[] PathType;                                      //!<Sets the type of path for the AI to use
    public GameObject Path;                                     //!<The current path the AI is on. Do not manually change this

    //Sight variables

    private RaycastHit hit;                                     //!<Takes information from RayCast
    private GameObject[] viableTargets;                         //!<All the available targets for the AI

    //Runs upon this object being created
    void Start()
    {
        if (aggressionLevel >= aggressionLimit)
            GetComponent<Rigidbody>().isKinematic =  true;
        mesh = GetComponent<NavMeshAgent>();
        #region StartMoving
        ChangeNavPoint(this.name, this.transform.position);
        SetSpeed(speed);
        #endregion
    }


    //Happens every update
    void FixedUpdate()
    {
        //sets the state of the AI
        #region state 
        if (hp > 0)
        {
            if (dazed == false)
            {
                mesh.SetDestination(navPoint);
                //attacks the current target meaning the player is in range of the AI and can be caught. Used for starting a gameover/restart at last checkpoint
                #region attack
                if (target != null)
                {
                    if (attackDistance >= Vector3.Distance(transform.position, target.transform.position))
                    {
                        //if the current target is the shadowPlayer (players last know position) Go to that position
                        if (target.name == "shadowPlayer(Clone)")
                        {
                            InvestigatePlayerPos(10f);
                        }
                        target = null;
                        //restart from last checkpoint
                    }
                }
                #endregion

                //check for all viable targets
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

        //Has the AI move to a place if they are panicing (no use currently remains of last programmer)
        if (panic == true)
        {
            if (aggressive == true)
            {
                panic = false;
            }
            else
            {
                pathing = false;
                ChangeNavPoint("panic", panicTarget);
                SetSpeed(runSpeed);
            }
        }

        //if the AI is an enemye (aggressive) and it can see the target start bulding up suspicion level
        if (aggressive == true && seesTarget == true)
        {
            //if the Enemy is set to alert, it wil automatically chase the player upon seeing them
            if (alert == true)
            {
                aggressionLevel = aggressionLimit;
            }
            else
            {
                // if suspicion level is at the limit then set the enemy to suspicious of player and start building aggression
                if (suspicionLevel >= suspicionLimit)
                {
                    suspicious = true;
                    //if aggression is at the limit have the AI start chasing the player
                    if (aggressionLevel >= aggressionLimit)
                    {
                        attacking = true;
                        pathing = false;
                        ChangeNavPoint(target.name, target.transform.position);
                        SetSpeed(runSpeed);
                    }
                    //lower aggression level if the player isnt seen
                    else
                    {
                        aggressionLevel += 1;
                    }
                }
                //lower the suspicion level if the player is not seen
                else
                {
                    suspicionLevel += 1;
                }
            }
        }
        else
        {
            // if the player is not seen but the AI was chasing the player. (when the player escapes the enemy line of sight) create a shadow of the player and have the AI investigate it.
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

        #region path
        if ((Vector3.Distance(transform.position, navPoint) < 3) && (target == null) && (pathing == true))
        {
            nextCheckpoint();
        }
        #endregion
        #endregion
    }

    //Lower the aggression level if the player is not seen.
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
            string CheckpointCountString = CheckpointCount.ToString();
            AINavPoints CheckpointScript = Path.GetComponent<AINavPoints>();
            ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
            pathing = true;
        }
    }

    //Lower the suspicion level if the player is not seen
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

    //make the AI dazed for a set amount of time. done by calling Dazed(float);
    #region dazed
    public IEnumerator Dazed(float dazeTime)
    {
        pathing = false;
        dazed = true;
        Pause();
        yield return new WaitForSeconds(dazeTime);
        dazed = false;
        Resume();
        pathing = true;

    }
    #endregion

    //Distracts the AI. called with NoiseHeard(GameObject);
    #region distracted

    public void NoiseHeard(GameObject soundPos)
    {
        if (seesTarget == false)
        {
            pathing = false;
            ChangeNavPoint(soundPos.name, soundPos.transform.position);
            if ((Vector3.Distance(transform.position, navPoint) < 10) && (target == null))
            {
                InvestigateSound(5f);
            }
        }
    }

    #endregion

    //Functions for moving the AI to a specific point. ChangeNavPoint(String, Vector3);
    #region Move functions
    public void ChangeNavPoint(string N, Vector3 T)
    {
        navCheck = N;
        navPoint = T;
    }
    //Sets the speed of the AI. Default walking speed
    public void SetSpeed(float Spd)
    {
        mesh.speed = Spd;
    }
    //Stops the AI on its path
    public void Pause()
    {
        mesh.Stop();
    }
    //Resumes the AI on its path
    public void Resume()
    {
        mesh.Resume();
    }
    #endregion

    //Functions for creating the AI's cone of vision

    //Function call for Getting targets.
    #region Sight Functions
    public IEnumerator CheckForTargets()
    {
        yield return new WaitForSeconds(.1f);
        GetTargets();
    }

    //Gets all targets wih the tag "Player" and stores them in an array
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
            //Sets the cone of vision for the AI. sets the angle, distance, checks if the target is not behind an object, checks if the raycast returns the player or something else. Finally sets the player as target if everything else passes
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
                                if (target != null && target.name == "shadowPlayer(Clone)")
                                {
                                    Destroy(target);
                                    CheckForTargets();
                                }
                                
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

    //Function for the damaging the AI (not sure if being used. remains of laster programmer)
    #region health
    public int hurt(int damage)
    {
        int HP = hp;
        HP -= damage;
        return HP;
    }
    //Function for the healing the AI (not sure if being used. remains of laster programmer)
    public int heal(int health)
    {
        int HP = hp;
        HP += health;
        return HP;
    }
    #endregion

    //Funtion for Investigating the sound. Needs a float that specifys how long the AI looks for
    public IEnumerator InvestigateSound(float lookTime)
    {
        yield return new WaitForSeconds(lookTime);
        string CheckpointCountString = CheckpointCount.ToString();
        AINavPoints CheckpointScript = Path.GetComponent<AINavPoints>();
        ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
        pathing = true;

    }

    //Function for Investigating the players last position needs a float to specify how long.
    public IEnumerator InvestigatePlayerPos(float lookTime)
    {
        yield return new WaitForSeconds(lookTime);
        string CheckpointCountString = CheckpointCount.ToString();
        AINavPoints CheckpointScript = Path.GetComponent<AINavPoints>();
        ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
        pathing = true;

    }

    //Function for moving the AI to the next checkpoint in their path scripts
    public void nextCheckpoint()
    {
        #region LoopPath
        if (PathwayCount <= Pathways.Length - 1)
        {
            
            Path = Pathways[PathwayCount];
            AINavPoints CheckpointScript = Path.GetComponent<AINavPoints>();

            //check for the type of path 
            switch (PathType[PathwayCount])
            { 
                //To Point
                case 0:
                    if (CheckpointCount <= CheckpointScript.AiCheckpoints.Count - 1)
                    {
                        string CheckpointCountString = CheckpointCount.ToString();
                        ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
                        if (CheckpointCount != CheckpointScript.AiCheckpoints.Count)
                        {
                            CheckpointCount++;
                        }
                    }
                    else
                    {
                        if (PathwayCount != Pathways.Length - 1)
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
                //Loop number of times across the points
                case 1:
                    if (LoopCount <= CheckpointScript.NofLoops | CheckpointScript.infinite == true)
                    {
                        if (CheckpointCount <= CheckpointScript.AiCheckpoints.Count - 1)
                        {
                            string CheckpointCountString = CheckpointCount.ToString();
                            ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
                            if (CheckpointCount != CheckpointScript.AiCheckpoints.Count)
                            {
                                CheckpointCount++;
                            }
                        }
                        else
                        {
                            CheckpointCount = 0;
                            if (CheckpointScript.infinite == false)
                            {
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
                //Go back and forth a number of times across the points 
                case 2:
                    if (LoopCount <= CheckpointScript.NofLoops | CheckpointScript.infinite == true)
                    {
                        if ((CheckpointCount < CheckpointScript.AiCheckpoints.Count) && (back == false))
                        {
                            string CheckpointCountString = CheckpointCount.ToString();
                            ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);
                            if (CheckpointCount != CheckpointScript.AiCheckpoints.Count)
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
                                ChangeNavPoint(CheckpointCountString, CheckpointScript.AiCheckpoints[CheckpointCount]);

                            }
                            else
                            {
                                back = false;
                                if (CheckpointScript.infinite == false)
                                {
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
