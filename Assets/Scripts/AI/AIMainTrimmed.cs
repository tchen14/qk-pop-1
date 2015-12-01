#pragma warning disable 414     //Variable assigned and not used: PlayerLastPos, DecrementaggressionRunning

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]	//Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]
[EventVisible]

public class AIMainTrimmed : MonoBehaviour
{

    //Vision Variables
    public bool CheckForTargetsRunning = false;
    public bool seesTarget = false;
    public string[] seekTag = { "Player" };             //!<The enemy Tag of this NPC
    public GameObject target = null;
    private RaycastHit hit;
    private GameObject[] viableTargets;
    public float sightDistance;                 //!<The distance the NPC is capable of seeing
    public float sightAngle;                       //!<The max angle of the cone of vision


    //Movement Variables
    public string navCheck = null;                        //!<The name of the current checkpoint
    public Vector3 navPoint = new Vector3(0, 0, 0);   //!<Contains the point to move in the navmesh
    private NavMeshAgent mesh = null;

    //Path Variables
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
    public int CheckpointCount = 0;                        //! Int for tracking the checkpoint the AI is on

    //State Variables
    private float aggressionLevel = 0;						//!<The current awareness of the NPC to the Player
    private float suspicionLevel = 0;						//!<The current suspicion of the NPC to the Player	
    private bool suspicious = false;                    //!<If the AI is suspicous
    public float aggressionLimit = 2;			        //!<The aggression level of the attacker
    public float suspicionLimit = 2;                  //!<The suspicon level of the attacker
    public bool IncrementsuspicionRunning = false;
    public bool IncrementaggressionRunning = false;
    public bool DecrementaggressionRunning = false;
    public bool DecrementsuspicionRunning = false;
    public bool chasing = false;
    public bool dazed = false;
    public float attackDistance = 2;                    //!<The distance the NPC stands away from the target and attacks*
    public float hp;
    public float speed;
    public float runSpeed;
    public string panicPoints = "PanicPoints";
    public bool enemy;
    public int current_preset = 0;
    private GameObject PlayerLastPos = null;             //visual representation of where the AI last remembers seeing the player before they went of LoS
    private bool shadowcreated = false;

    //start variables
    private bool startCheckForTargetsRunning;
    private bool startseesTarget;
    private GameObject starttarget;
    private GameObject[] startviableTargets;
    private string startnavCheck;
    private Vector3 startnavPoint;
    private List<Vector3> startAiCheckpoints;
    private List<GameObject> startPathways;
    private List<int> startPathType;
    private List<int> startnofLoops;
    private List<bool> startinfinite;
    private bool startlooping;
    private int startLoopCount;
    private bool startBacknForth;
    private bool startback;
    private int startPathwayListLength;
    private int startAiCheckpointsLength;
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
    private bool startenemy;
    private int startcurrent_preset;
    private bool startshadowcreated;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 startScale;

    Animator anim;

    void Start()
    {
        //This sets all the variables that change in runtime to a start variable that can be used later. Start variables store the values of the ai of when it is created.
        startCheckForTargetsRunning = CheckForTargetsRunning;
        startseesTarget = seesTarget;
        starttarget = target;
        startviableTargets = viableTargets;
        startnavCheck = navCheck;
        startnavPoint = navPoint;
        startAiCheckpoints = AiCheckpoints;
        startPathways = Pathways;
        startPathType = PathType;
        startnofLoops = nofLoops;
        startinfinite = infinite;
        startlooping = looping;
        startLoopCount = LoopCount;
        startBacknForth = BacknForth;
        startback = back;
        startPathwayListLength = PathwayListLength;
        startAiCheckpointsLength = AiCheckpointsLength;
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
        startenemy = enemy;
        startcurrent_preset = current_preset;
        startshadowcreated = shadowcreated;
        startPos = transform.position;
        startRot = transform.localRotation;
        startScale = transform.localScale;


        PlayerLastPos = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/shadowPlayer.prefab", typeof(GameObject)) as GameObject;
        mesh = GetComponent<NavMeshAgent>();
        GetComponent<Rigidbody>().isKinematic = true;
        ChangeNavPoint(this.name, this.transform.position);
        SetSpeed(speed);
        SetupAnimator();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                    ChangeNavPoint(target.name, target.transform.position);
                    SetSpeed(speed);
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
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
                    if (Pathways != null && Pathways.Count - 1 >= PathwayCount)
                    {

                        Path = Pathways[PathwayCount];
                        AIPath CheckpointScript = Path.GetComponent<AIPath>();
                        string CheckpointCountString = CheckpointCount.ToString();
                        ChangeNavPoint(CheckpointCountString, CheckpointScript.getPoints()[CheckpointCount]);
                    }
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
        AiCheckpoints = startAiCheckpoints;
        Pathways = startPathways;
        PathType = startPathType;
        nofLoops = startnofLoops;
        infinite = startinfinite;
        looping = startlooping;
        LoopCount = startLoopCount;
        BacknForth = startBacknForth;
        back = startback;
        PathwayListLength = startPathwayListLength;
        AiCheckpointsLength = startAiCheckpointsLength;
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
        enemy = startenemy;
        current_preset = startcurrent_preset;
        shadowcreated = startshadowcreated;
        transform.position = startPos;
        transform.localRotation = startRot;
        transform.localScale = startScale;
    }
    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach (Animator childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
}