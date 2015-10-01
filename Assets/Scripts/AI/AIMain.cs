using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

[RequireComponent(typeof(NavMeshAgent))]	//Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]
/*! 
 *	This code is the main ai controller. The class is segmented into the following regions: attack, movement, state, sight, and health
 */
[EventVisible]
public class AIMain : MonoBehaviour {

	//Variables Editable

	public int 					hp 					= 100;						//!<Health of the NPC
	public float 				sightDistance 		= 45;						//!<The distance the NPC is capable of seeing
	public float 				sightAngle			= 45;						//!<The max angle of the cone of vision
	public float 				speed 				= 5;						//!<Casual Speed of the NPC
	public float		 		runSpeed 			= 8;						//!<Scared, Charging, Aggressive Speed of the NPC*
	public string[]		 		seekTag 			= {"Player"};				//!<The enemy Tag of this NPC
	public float 				attackDistance		= 3;						//!<The distance the NPC stands away from the target and attacks*
	public float 				aggressionLimit		= 100;						//!<The aggression level of the attacker
    public float                suspicionLimit      = 150;						//!<The suspicon level of the attacker
	public GameObject 			startPoint 			= null;						//!<Sets the first navPoint, defaults to stationary
	public string				panicPoints			= "PanicPoints";			//!<The object name of the vector keeper of related panic points for the AI
	public bool 				aggression 			= false;					//!<If the NPC will attack the player
    public GameObject           PlayerLastPos       = null;                     //visual representation of where the AI last remembers seeing the player before they went of LoS
    public bool                 Searching           = false;                    //if the AI is currently looking for the player at its shadow
    public bool                 checking            = false; 


	//Variables Controllers
	
	public bool 		        seesTarget 			= false;					//!<If the Player has been spotted
	[ReadOnly]public GameObject target				= null;						//!<The transform of the player
	public bool 		        attacking 			= false;					//!<If the AI is attacking
	[ReadOnly]public bool 		panic 				= false;					//!<If the AI is panicking
	[ReadOnly]public Vector3 	panicTarget			= new Vector3 (0, 0, 0);	//!<Target of AI panic
	[ReadOnly]public float 		aggressionLevel 	= 0;						//!<The current awareness of the NPC to the Player
    [ReadOnly]public float      suspicionLevel      = 0;						//!<The current suspicion of the NPC to the Player	
    public bool                 suspicious          = false;					//!<If the AI is suspicous
    public bool                 alert               = false;					//!<If the AI is alert
    public bool                 unconsious          = false;                    //!<If the AI is unconsious
    public bool                 dazed               = false;                    //!<If the AI is dazed

	//Movement variables

	private NavMeshAgent 		mesh 				= null;						//!<Contains the component to use the navmesh
	[ReadOnly]public string		navCheck			= null;						//!<The name of the current checkpoint
	[ReadOnly]public Vector3 	navPoint			= new Vector3 (0, 0, 0);	//!<Contains the point to move in the navmesh

	//Sight variables

	private RaycastHit 			hit;											//!<Takes information from RayCast
	private GameObject[] 		viableTargets;									//!<All the available targets for the AI

	//! Unity Start function
	void Start() 
	{
		GetComponent<Rigidbody> ().isKinematic = true;
		#region movement
			//The AI will find the navMesh of the level and position itself on the navMesh
			mesh = GetComponent<NavMeshAgent>();	//Get personal nav controller from GameObject(Gameobject will automaticly have navMesh applied from this script)
			if(startPoint != null)
				ChangeNavPoint(startPoint.name,startPoint.transform.position);	//Set the current target point, the starting destination the AI will go to
			else
				ChangeNavPoint(this.name,this.transform.position);	//set the current target point to current posistion
			SetSpeed(speed);						//Set the speed that the AI will move at to normal speed
		#endregion

		#region state
			//GetComponent<AIMain>().panicTarget = GameObject.Find(panicPoints).GetComponent<PanicTargets>().GetPanickPoint();	//Get personal refuge point 
		#endregion
    }

    //! Unity Update function
    void Update() 
	{ 
        //will this satisfy a knockedout condition?
		if (hp > 0)
        {
            if (dazed == false) //if the enemey's hp is above 0 and not dazed it will perform its normal routine
            {

                #region attack
                //If the AI does not have an active target then there is nothing to attack
                if (target != null)
                {
                    
                    //If the AI is within range to attack then activate the Attack Funtionality
                    if (attackDistance >= Vector3.Distance(transform.position, target.transform.position))
                    {
                        attacking = true;   //begin the attack timer
                                            /*
                            * This is where the function for attacking is called. It is currently unclear as to whether the player will be reset or damaged,
                            * If this code will be an external, internal, or written function here, and if the game will check for checkpoints from this
                            * location in the script
                            */
                                            //Debug.Log("ai","ATTACK!");
                                            /*
                        */
                        target = null;      //reset target to make the AI search for the player again
                    }
                }
                #endregion

                #region movement
                //continue walking to the current location, reseting the nav grid to insure other AI, the player, or movable objects have not become obsticals
                mesh.SetDestination(navPoint);
                #endregion

                #region sight
                //If the AI is willing to fight but has not found an enemy begin the coroutine to find a viable target
                //modified this so it will keep checking as long as AI is aggressive. Allows for checks to see if the player can be seen
                if (aggression == true)
                {
                    StartCoroutine("CheckForTargets");
                }
                #endregion

                
            }
            else // if the enemy's hp is above 0 and dazed it will perform this
            {

            }

		}

        else //if the enemies hp is 0 or below it will perform this
        {
            Pause();
            //Log.M ("ai","DEAD");
        }
	}


    //! Unity FixedUpdate function
	void FixedUpdate ()
    {
        #region state
        //If the AI is currently scared
        if (panic == true)
        {
            //If the AI is willing to fight then it cannot be scared, reset fear
            if (aggression == true)
            {
                panic = false;
            }
            //If the AI is scared find the panicTarget and run to that location
            else
            {
                ChangeNavPoint("panic", panicTarget);
                SetSpeed(runSpeed);
            }
        }
        //If the AI has a target already and is willing to fight and is not currently attacking the target
        if (aggression == true && seesTarget == true && attacking == false)
        {
            //Debug.Log("ai", aggressionLevel.ToString());
            //If the AI is aware of its target set the navPoint to the target
            if (aggressionLevel >= aggressionLimit)
                ChangeNavPoint(target.name, target.transform.position);
            //If the AI is not yet aware of its target increase its awarness
            else
                aggressionLevel += 1;
        }
        //If the AI is not aware of a target,  not aggressive, or is attacking, decrease its aggression levels
        else
        {

            //So long as the AI is aggressive, this cannot go below 0, decrement agressionLevel
            if (aggressionLevel > 0)
            {
                if (checking == false && Searching == false)
                {
                    LastKnowLoc(target.transform.position);
                    
                }
                aggressionLevel -= 1;
                //Debug.Log("ai", aggressionLevel.ToString());
                //If the AI's agression level hits 0 it will go back to its origial destiation set on start
                if (aggressionLevel == 0)
                {
                    ChangeNavPoint(startPoint.name, startPoint.transform.position);
                }
            }
        }
        #endregion
    }

    #region attack
    //! Wait time between attacks???
    public IEnumerator EndAttack() 
	{
        yield return new WaitForSeconds (1);
		attacking = false;
    }
    #endregion

    #region dazed

    //turn off line of sight (assuming visible LoS is a feature)
    //after x number of seconds
    //stop being dazed (animation)
    //start search sequence
    //turn left and right to look around
    //after set amount of time set its target to its last target
    //set dazed to false


    #endregion

    #region distracted

    //if noiseHeard = true (triggered either here through send message or in the throw script)
    //if enemymy does not see player
    //store last target
    //set location of noise from noise throw
    // if current location = target location
    //start search sequence

    /*if (noiseHeard == true)
    {
        if (seesTarget = false)
        {
            navPoint = (location fed from noise throw);
            mesh.SetDestination(navPoint);
            if (transform.position == navPoint)
            {
                Investigate();
            }
        }
        else
        {

        }
    }
    else
    {
    }
    */

    #endregion

#region movement
    //!<Sets a new destination for the AI
public void ChangeNavPoint(string N,Vector3 T)
{
navCheck = N;
navPoint = T;
}
//!<Sets the AI's Speed on the navMesh
public void SetSpeed(float Spd)
{
mesh.speed = Spd;
}
//!<Stops the AI from moving on the mesh
public void Pause()
{
//Log.M ("ai", "W");
mesh.Stop();
}
//!<Resumes the AI on its current path
public void Resume()
{
mesh.Resume();
}
#endregion

#region sight
//called every tenth of a second to look for a new target
public IEnumerator CheckForTargets()
{
yield return new WaitForSeconds(.1f);
GetTargets();
}

//!<Sends Raycast and sets up AI_Main
public void GetTargets()
{
//Create a dynamic type array to add all targets into an array
List<GameObject> viableTargets = new List<GameObject> ();


//Gathers all enemy targets by tags and adds them to the end of the array
for(int enemyType = 0; enemyType < seekTag.Length; enemyType++)
{
    GameObject[] targets = GameObject.FindGameObjectsWithTag(seekTag[enemyType]);
    for(int searchEnemyType = 0;searchEnemyType < targets.Length; searchEnemyType++)
         viableTargets.Add(targets[searchEnemyType]);
}
//sets our dynamic array to a static array for easy use		GameObject[] viableTargets = (GameObject[])checkingTargets.ToArray (typeof(GameObject));

if(viableTargets.Count != 0)
{
    for(int searchTargets = 0; searchTargets < viableTargets.Count; searchTargets++)
    {          
        //If the target is within the field of view of the AI continue
        if (Vector3.Angle(viableTargets[searchTargets].transform.position-transform.position,transform.forward)< sightAngle)
        {  
            //If the target is within the sight distance of the AI continue
            if(Vector3.Distance(transform.position,viableTargets[searchTargets].transform.position) < sightDistance)
            {
                
                //If the target is within Line of Sight continue
                if(Physics.Raycast(transform.position,viableTargets[searchTargets].transform.position-transform.position,out hit))
                {
                    //If sight is true continue
                    if(hit.collider.tag == viableTargets[searchTargets].tag)
                    {
                        //Set this as the target
                        target = hit.collider.gameObject;
                        seesTarget = true;
                    }
                    else
                    {
                        seesTarget = false;
                        //Log.M ("ai", "OUT OF SIGHT");
                    }
                }
            }
            else
            {
                seesTarget = false;
                //Log.M ("ai", "OUT OF DISTANCE");
            }
        }
        else
        {
            seesTarget = false;
            //Log.M ("ai", "out of angles");
        }
    }
}
else
{
    //Debug.Log("ai","No available Targets");
}
}
#endregion

#region health
//!<Removes from the Health of the AI
public int hurt (int damage) 
{
int HP = hp;
HP -= damage;
return HP;
}
//!<Adds to the health of the AI
public int heal (int health) 
{
int HP = hp;
HP += health;
return HP;
}
#endregion

#region recover
//AI waits ten seconds before recovering from it's dazed status


public IEnumerator statusRecover()
{
yield return new WaitForSeconds(10.0f);
dazeRecover();
}

//Enemy starts recovering AI from dazed state
public void dazeRecover()
{
//dazed == false
//whatever actions need to be performed after recovery
//ie. high suspicion level, aggressive, look around? contine patrol?
}
#endregion

    public void LastKnowLoc(Vector3 LastPos)
    {
    //triggers upon the player moving out of sight of the enemy
    //spawn an unmoving transparent version of the player at the players location
    //enemys target is set to that
    //upon reaching that spot the enemy runs search sequence

        
       
        checking = true;
        Instantiate(PlayerLastPos, target.transform.position, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, .1f);
        int counter = 0;
        while (target.name != "shadowPlayer")
        {

            target = hitColliders[counter].gameObject;
        }
        Searching = true;
        
        
        ChangeNavPoint(target.name, target.transform.position);
        if (transform.position == navPoint)
        {
            Searching = false;
            checking = false;
            Investigate();
        }
        
    }

    public void Investigate()
    {
        Debug.Log("ai","works");
        //play animation
        //after animation ChangeNavPoint(startPoint.name, startPoint.transform.position);
        //set noiseHeard to false
        //Searching = false;
    }
}