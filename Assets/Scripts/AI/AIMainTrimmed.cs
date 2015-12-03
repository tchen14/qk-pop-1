using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
    
[RequireComponent(typeof(NavMeshAgent))]	                //!<Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]                       //!<Automaticaly make a rigid body on this object
[EventVisible]

public class AIMainTrimmed : MonoBehaviour
{

    //Vision Variables
    public bool CheckForTargetsRunning = false;             //!<Bool to only run CheckForTarges one at a time. 
    public bool seesTarget = false;                         //!<Bool to determine if the target is seen.
    public string[] seekTag = { "Player" };                 //!<The enemy Tag of this NPC.
    public GameObject target = null;                        //!<The Gameobject the enemy is looking for
    private RaycastHit hit;                                 //!<Returns what was hit by the raycast. Used for searching for player
    private GameObject[] viableTargets;                     //!<Array of all gameobjects that have the tag specified by seekTag.
    public float sightDistance;                             //!<The distance the NPC is capable of seeing
    public float sightAngle;                                //!<Stores the angle of the NPC's cone of vision. Do not modify this value. Refer to passiveSightAngle and chasingSightAngle
    public float passiveSightAngle;                         //!<The sight angle of the npc when not searching for its target.
    public float chasingSightAngle;                         //!<The sight angle of the npc when searching for its target. Much greater than passiveSightAngle. Used to make it difficult for the player to just run arond the npc and break line of sight while chasings.


    //Movement Variables
    public string navCheck = null;                          //!<The name of the current checkpoint
    public Vector3 navPoint = new Vector3(0, 0, 0);         //!<Contains the point to move in the navmesh
    public Vector4 startPoint = new Vector4(0, 0, 0, 0);    //!<The Starting point of the AI. Stored on start of this object. Place the object where you want its start point to be.
    public Vector4 endPoint = new Vector4(0, 0, 0, 0);      //!<The End point of the AI. Stores the last checkpoint on the last path.
    private NavMeshAgent mesh = null;                       //!<Variable placeholder for the navmesh. Set on start.

    //Path Variables
    public List<GameObject> Pathways;                       //!<List of the paths the AI uses. Paths are gameobjects with a list of vectors 3s that the AI uses to as waypoints. See AI's editor for paths.
    public List<int> PathType;                              //!<Specifies the Type of path the AI will use. To point, Back and forth, Loop, On guard.
    public List<int> nofLoops;                              //!<Number of loops the AI will do on a certain path. Each path has its own nofLoops.
    public List<bool> infinite;                             //!<Sets the number of loops to infinite for an AI that patrols indefinitely.
    private bool looping;                                   //!<Bool to set the AI to the loop type of path.
    private int LoopCount = 1;                              //!<Int to track the number of times looped.
    private bool BacknForth;                                //!<Bool to set the AI to the back and forth type of path
    private bool back = false;                              //!<Bool to determine if it is going back on its path during the back and forth loop.
    public int PathwayListLength;                           //!<Int to store the number of checkpoints on a path.
    public GameObject Path;                                 //!<Stores the current path of the AI so it's checkpoints can be accessed.
    public int PathwayCount = 0;                            //!<Int for tracking which path the AI is on
    public int CheckpointCount = 0;                         //! Int for tracking which checkpoint the AI is on
    public bool enemy;                                      //!<Bool to determine if the AI is and enemy or not. If not an enemy it wont use functions that an enemy would

    //State Variables
    private float aggressionLevel = 0;						//!<The current awareness of the NPC to the Player
    private float suspicionLevel = 0;						//!<The current suspicion of the NPC to the Player	
    private bool suspicious = false;                        //!<If the AI is suspicous
    public float aggressionLimit = 2;			            //!<The aggression level of the attacker
    public float suspicionLimit = 2;                        //!<The suspicon level of the attacker
    public bool IncrementsuspicionRunning = false;          //!<Bool to only run IncrementSuspicion again after it finishes
    public bool IncrementaggressionRunning = false;         //!<Bool to only run Incrementaggression again after it finishes
    public bool DecrementaggressionRunning = false;         //!<Bool to only run Decrementaggression again after it finishes
    public bool DecrementsuspicionRunning = false;          //!<Bool to only run Decrementsuspicion again after it finishes
    public bool chasing = false;                            //!<Bool to check if AI is currently Chasing its target
    public bool dazed = false;                              //!<Bool to check if AI is currently Dazed
    public float attackDistance = 2;                        //!<The distance the AI will catch its target.
    public float hp;                                        //!<Float for HP. Currently unused.
    public float speed;                                     //!<The default speed of the AI.
    public float runSpeed;                                  //!<The running speed of the AI.
    public string panicPoints = "PanicPoints";              //!<Where the AI will run when the panic state is activated. Currently unused.
    public int current_preset = 0;                          //!<Sets the AI to a type of AI such as guard, townperson, boss etc. Set the preset type in editor window. Sets Variables essential to the AI.
    private GameObject PlayerLastPos = null;                //!Visual representation of where the AI last remembers seeing the player before they went of LoS. Not currently used.
    private bool shadowcreated = false;                     //!<Bool to check and see if the shadow has already been created. Not being used currently
    public enum AIState { Idle, Move, Pivot, Sprint, Chasing, Normal, Search, Dazed, KnockoutLight, KnockoutHeavy } //!<Various states the AI can be in for animation. 
    public AIState _moveState { get; private set; }         //!<Sets the movement state of the AI for animations.

    //start variables 
    //!<All variables listd below store their respective variable at start. Upon "Restart" such as when the player dies and is moved to the last checkpoint, the AI restores all of its original states to when it started the scene.
    private bool startCheckForTargetsRunning;
    private bool startseesTarget;
    private GameObject starttarget;
    private GameObject[] startviableTargets;
    private string startnavCheck;
    private Vector3 startnavPoint;
    private List<GameObject> startPathways;
    private List<int> startPathType;
    private List<int> startnofLoops;
    private List<bool> startinfinite;
    private bool startlooping;
    private int startLoopCount;
    private bool startBacknForth;
    private bool startback;
    private int startPathwayListLength;
    private GameObject startPath;
    private int startPathwayCount;
    private int startCheckpointCount;
    private float startaggressionLevel;
    private float startsuspicionLevel;
    private bool startsuspicious;
    private bool startIncrementsuspicionRunning;
    private bool startIncrementaggressionRunning;
    private bool startDecrementaggressionRunning;
    private bool startDecrementsuspicionRunning;
    private bool startchasing;
    private bool startdazed;
    private float starthp;
    private int startcurrent_preset;
    private bool startshadowcreated;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 startScale;
    private float startsightAngle;
     

    void Awake()
    {
        //This sets all the variables that change in runtime to a start variable that can be used later. Start variables store the values of the ai of when it is created.
        sightAngle = passiveSightAngle;
        startCheckForTargetsRunning = CheckForTargetsRunning;
        startseesTarget = seesTarget;
        starttarget = target;
        startviableTargets = viableTargets;
        startnavCheck = navCheck;
        startnavPoint = navPoint;
        startPathways = Pathways;
        startPathType = PathType;
        startnofLoops = nofLoops;
        startinfinite = infinite;
        startlooping = looping;
        startLoopCount = LoopCount;
        startBacknForth = BacknForth;
        startback = back;
        startPathwayListLength = PathwayListLength;
        startPath = Path;
        startPathwayCount = PathwayCount;
        startCheckpointCount = CheckpointCount;
        startaggressionLevel = aggressionLevel;
        startsuspicionLevel = suspicionLevel;
        startsuspicious = suspicious;
        startIncrementsuspicionRunning = IncrementsuspicionRunning;
        startIncrementaggressionRunning = IncrementaggressionRunning;
        startDecrementsuspicionRunning = DecrementsuspicionRunning;
        startDecrementaggressionRunning = DecrementaggressionRunning;
        startchasing = chasing;
        startdazed = dazed;
        starthp = hp;
        startcurrent_preset = current_preset;
        startshadowcreated = shadowcreated;
        startPos = transform.position;
        startRot = transform.localRotation;
        startScale = transform.localScale;



        startPoint = this.transform.position;               //!<Sets the startPoint to its current location.
        PlayerLastPos = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/shadowPlayer.prefab", typeof(GameObject)) as GameObject;
        mesh = GetComponent<NavMeshAgent>();                //!<Sets the navmesh for the AI
        GetComponent<Rigidbody>().isKinematic = true;       //!<Assigns Kinematic to true to the rigidbody
        Path = Pathways[PathwayCount];                      //!<Sets the first path to the current path.
        AIPath CheckpointScript = Path.GetComponent<AIPath>(); //!<
        //endPoint = CheckpointScript.getPoints().Count;
        ChangeNavPoint(this.name, this.transform.position);
        SetSpeed(speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //sets the destination to
        mesh.SetDestination(navPoint);
        if (CheckForTargetsRunning == false)
        {
            CheckForTargetsRunning = true;
            GetTargets();
        }
        if (target != null)
        {
            //if the target is in attack distance it will attack the target. If it is the shadow player destroy it. If the target is the player reset the level.
            if ((attackDistance >= Vector3.Distance(transform.position, target.transform.position) && (chasing == true)))
            {
                Time.timeScale = 0;
            }
        }
        if ((Vector3.Distance(transform.position, navPoint) < 3) && (chasing == false))
        {
            nextCheckpoint();
        }
        if (seesTarget == true)
        {
            if (suspicionLevel < suspicionLimit)
            {
                if (IncrementsuspicionRunning == false)
                {
                    IncrementsuspicionRunning = true;
                    StartCoroutine("IncrementSuspicion");
                }
            }
            else //if suspicion is at max
            {
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                if (aggressionLevel < aggressionLimit)
                {
                    if (IncrementaggressionRunning == false)
                    {
                        IncrementaggressionRunning = true;
                        StartCoroutine("Incrementaggression");
                    }
                }
                else //if aggression is at max
                {
                    chasing = true;
                    sightAngle = chasingSightAngle;
                    ChangeNavPoint(target.name, target.transform.position);
                    SetSpeed(speed);
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    _moveState = AIState.Chasing;
                }
            }
        }
        else //if the target is not seen
        {
            //if the AIs aggression level is greater than 0 and it can't find the player decrease aggression
            if (aggressionLevel > 0)
            {
                if (DecrementaggressionRunning == false)
                {
                    DecrementaggressionRunning = true;
                    StartCoroutine("Decrementaggression");
                }
            }
            else //if aggression is at 0
            {
                if (suspicionLevel > 0)
                {
                    if (DecrementsuspicionRunning == false)
                    {
                        DecrementsuspicionRunning = true;
                        StartCoroutine("Decrementsuspicion");
                    }
                }
                else //if suspicion is at 0
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.grey;
                    chasing = false;
                    sightAngle = passiveSightAngle;
                    Path = Pathways[PathwayCount];
                    AIPath CheckpointScript = Path.GetComponent<AIPath>();
                    string CheckpointCountString = CheckpointCount.ToString();
                    ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
                    _moveState = AIState.Move;
                    //return to path
                }
            }

        }
    }
    #region Sight Functions
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
                        //raycast to top of box and raycast to bottom
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

    public void ChangeNavPoint(string N, Vector3 T)
    {
        navCheck = N;
        navPoint = T;
    }

    public IEnumerator IncrementSuspicion()
    {
        yield return new WaitForSeconds(.5f);
        suspicionLevel += 1;
        IncrementsuspicionRunning = false;
    }

    public IEnumerator Incrementaggression()
    {
        yield return new WaitForSeconds(.7f);
        aggressionLevel += 1;
        IncrementaggressionRunning = false;
    }
    public IEnumerator Decrementsuspicion()
    {
        yield return new WaitForSeconds(.7f);
        suspicionLevel -= 1;
        DecrementsuspicionRunning = false;
    }

    public IEnumerator Decrementaggression()
    {
        yield return new WaitForSeconds(.5f);
        aggressionLevel -= 1;
        DecrementaggressionRunning = false;
    }

    public void NoiseHeard(GameObject soundPos)
    {
        //if the AI does not currently see the player it will be go to the object that emited the sound.  
        if (seesTarget == false)
        {
            ChangeNavPoint(soundPos.name, soundPos.transform.position);
            //once the AI is near the sound source it will pause for a moment looking around
            if ((Vector3.Distance(transform.position, navPoint) < 5) && (target == null))
            {
                InvestigateSound(5f);
            }
        }
        else
        {
            //AI sees the player.
        }
    }

    public IEnumerator InvestigateSound(float lookTime)
    {
        //run search animation
        _moveState = AIState.Search;
        yield return new WaitForSeconds(lookTime);
        Path = Pathways[PathwayCount];
        AIPath CheckpointScript = Path.GetComponent<AIPath>();
        string CheckpointCountString = CheckpointCount.ToString();
        ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount - 1]);

    }
    #region dazed
    public IEnumerator Dazed(float dazeTime)
    {
        //AI becomes dazed, no longer performing its normal functions, not moving on its path for a set time. After that time it resumes like normal.
        //run dazed animation
        _moveState = AIState.Dazed;
        dazed = true;
        Pause();
        yield return new WaitForSeconds(dazeTime);
        dazed = false;
        Resume();
    }


    #endregion
    //sets the speed of the AI. Needs a float. 
    public void SetSpeed(float Spd)
    {
        mesh.speed = Spd;
        //change the animation to the proper speed.
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

                            if (!infinite[PathwayCount])
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

                                if (!infinite[PathwayCount])
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

                case 3:
                    if (CheckpointCount < CheckpointScript.getPoints().Count)
                    {
                        string CheckpointCountString = CheckpointCount.ToString();
                        ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
                        Debug.Log("works");
                    }
                    break;
            }
        }
        else
        {

        }
    }
    #endregion

    public void Restart()
    {
        CheckForTargetsRunning = startCheckForTargetsRunning;
        seesTarget = startseesTarget;
        target = starttarget;
        viableTargets = startviableTargets;
        navCheck = startnavCheck;
        navPoint = startnavPoint;
        Pathways = startPathways;
        PathType = startPathType;
        nofLoops = startnofLoops;
        infinite = startinfinite;
        looping = startlooping;
        LoopCount = startLoopCount;
        BacknForth = startBacknForth;
        back = startback;
        PathwayListLength = startPathwayListLength;
        Path = startPath;
        PathwayCount = startPathwayCount;
        CheckpointCount = startCheckpointCount;
        aggressionLevel = startaggressionLevel;
        suspicionLevel = startsuspicionLevel;
        suspicious = startsuspicious;
        IncrementsuspicionRunning = startIncrementsuspicionRunning;
        IncrementaggressionRunning = startIncrementaggressionRunning;
        DecrementsuspicionRunning = startDecrementsuspicionRunning;
        DecrementaggressionRunning = startDecrementsuspicionRunning;
        chasing = startchasing;
        dazed = startdazed;
        hp = starthp;
        current_preset = startcurrent_preset;
        shadowcreated = startshadowcreated;
        transform.position = startPos;
        transform.localRotation = startRot;
        transform.localScale = startScale;
    }
    
}