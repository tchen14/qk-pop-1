using UnityEngine;
using System.Collections;

/*
This Script is attached to Each AI. all of the states an AI can be in are linked to this script.
Update and OnTrigger in this script will run the updateState/OnTriggerstate inside the script of its current state. Ex: If the AI's currentState is set to Patrol State, on update it will run the updateState in the patrolState script.


*/

public class StatePatternEnemy : MonoBehaviour
{

    public float searchingTurnSpeed = 180f;
    public float searchingDuration = 4f;
    public float sightRange = 20f;
    public Transform[] wayPoints;
    public Transform eyes;
    public Vector3 offset = new Vector3(0, .5f, 0);
    public MeshRenderer meshRendererFlag;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public GuardState guardState;
    [HideInInspector] public DazedState dazedState;
    [HideInInspector] public DistractedState distractedState;
    [HideInInspector] public SuspiciousState suspiciousState;
    [HideInInspector] public KOState koState;
    [HideInInspector] public WalkState walkState;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        guardState = new GuardState(this);
        dazedState = new DazedState(this);
        distractedState = new DistractedState(this);
        suspiciousState = new SuspiciousState(this);
        koState = new KOState(this);
        walkState = new WalkState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }



    // Use this for initialization
    void Start ()
    {
        /*
        Set default state of the AI (walking, patroling, guarding)
        set current state to default state

        
        
        
         
        */
        currentState = patrolState; //sets the current state
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentState.UpdateState(); //calls the update of the current state
	}

    private void OnTriggerEnter(Collider other)
    {
        /*
        if the player is not sneaking or undetectable in some form
            OnTriggerEnter
            (will AI detect player if they get in a certain range in any direction?)
            (should this ^ be placed in each individual script?)
                (wont be needed for some states such as dazed or ko)

        */



        currentState.OnTriggerEnter(other); // calls OnTriggerEnter in the current State
    }
}
