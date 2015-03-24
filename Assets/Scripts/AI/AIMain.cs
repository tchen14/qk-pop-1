using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Debug = FFP.Debug;

[RequireComponent(typeof(NavMeshAgent))]	//Automaticly Make a navMeshAgent on this game object when this script is applied
[RequireComponent(typeof(Rigidbody))]
/*! 
 *	This code is the main ai controller. The class is segmented into the following regions: attack, movement, state, sight, and health
 */
public class AIMain : MonoBehaviour {

	//Variables Editable

	public int 					hp 					= 100;						//!<Health of the NPC
	public float 				sightDistance 		= 20;						//!<The distance the NPC is capable of seeing
	public float 				sightAngle			= 35;						//!<The max angle of the cone of vision
	public float 				speed 				= 5;						//!<Casual Speed of the NPC
	public float		 		runSpeed 			= 8;						//!<Scared, Charging, Aggressive Speed of the NPC*
	public string[]		 		seekTag 			= {"Player"};				//!<The enemy Tag of this NPC
	public float 				attackDistance		= 3;						//!<The distance the NPC stands away from the target and attacks*
	public float 				aggressionLimit		= 100;						//!<The aggression level of the attacker
	public GameObject 			startPoint 			= null;						//!<Sets the first navPoint, defaults to stationary
	public string				panicPoints			= "PanicPoints";			//!<The object name of the vector keeper of related panic points for the AI
	public bool 				aggression 			= false;					//!<If the NPC will attack the player

	//Variables Controllers
	
	[ReadOnly]public bool 		seesTarget 			= false;					//!<If the Player has been spotted
	[ReadOnly]public GameObject target				= null;						//!<The transform of the player
	[ReadOnly]public bool 		attacking 			= false;					//!<If the AI is attacking
	[ReadOnly]public bool 		panic 				= false;					//!<If the AI is panicking
	[ReadOnly]public Vector3 	panicTarget			= new Vector3 (0, 0, 0);	//!<Target of AI panic
	[ReadOnly]public float 		aggressionLevel 	= 0;						//!<The current awareness of the NPC to the Player	

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
    	#region attack
	        //If the AI does not have an active target then there is nothing to attack
	        if (target != null) 
			{
				//If the AI is within range to attack then activate the Attack Funtionality
	            if (attackDistance >= Vector3.Distance (transform.position,target.transform.position)) 
				{
	               attacking = true;	//begin the attack timer
				/*
				 * This is where the function for attacking is called. It is currently unclear as to whether the player will be reset or damaged,
				 * If this code will be an external, internal, or written function here, and if the game will check for checkpoints from this
				 * location in the script
				 */
	               //Debug.Log("ai","ATTACK!");
				/*
				 */
	               target = null;		//reset target to make the AI search for the player again
	            }
	        }
        #endregion

        #region movement
			//continue walking to the current location, reseting the nav grid to insure other AI, the player, or movable objects have not become obsticals
	        mesh.SetDestination(navPoint);
		#endregion

        #region sight
        //If the AI is willing to fight but has not found an enemy begin the coroutine to find a viable target
		if(aggression == true && target == null)
		{
			StartCoroutine("CheckForTargets");
		}
        #endregion
    }

    //! Unity FixedUpdate function
	void FixedUpdate () 
	{
		#region state
			//If the AI is currently scared
			if(panic == true)
			{
				//If the AI is willing to fight then it cannot be scared, reset fear
				if(aggression == true)
				{
					panic = false;
				}
				//If the AI is scared find the panicTarget and run to that location
				else
				{
					ChangeNavPoint("panic",panicTarget);
					SetSpeed(runSpeed);
				}
			}
			//If the AI has a target already and is willing to fight and is not currently attacking the target
			if(aggression == true && seesTarget == true && attacking == false)
			{
				Log.M ("ai", aggressionLevel.ToString());
				//If the AI is aware of its target set the navPoint to the target
				if(aggressionLevel >= aggressionLimit)
					ChangeNavPoint(target.name,target.transform.position);
				//If the AI is not yet aware of its target increase its awarness
				else
					aggressionLevel += 1;
			}
			//If the AI is not aware of a target,  not aggressive, or is attacking, decrease its aggression levels
			else
			{
				//So long as the AI is aggressive, this cannot go below 0, decrement agressionLevel
				if(aggressionLevel > 0)
				{
					aggressionLevel -= 1;
				}
			}
		#endregion
	}

	#region attack
    //! Wait time between attacks???
    public IEnumerator EndAttack() 
	{
        yield return new WaitForSeconds (1);
    }
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
		Log.M ("ai", "W");
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
	private void GetTargets()
	{
		//Create a dynamic type array to add all targets into an array
		List<GameObject> viableTargets = new List<GameObject> ();

		 
		//Gathers all enemy targets by tags and adds them to the end of the array
		for(int i = 0; i < seekTag.Length; i++)
		{
			GameObject[] targets = GameObject.FindGameObjectsWithTag(seekTag[i]);
			for(int ii = 0; ii < targets.Length; ii++)
				 viableTargets.Add(targets[ii]);
		}
		//sets our dynamic array to a static array for easy use		GameObject[] viableTargets = (GameObject[])checkingTargets.ToArray (typeof(GameObject));

		if(viableTargets.Count != 0)
		{
			for(int i = 0; i < viableTargets.Count; i++)
			{
				//If the target is within the field of view of the AI continue
				if(Vector3.Angle(viableTargets[i].transform.position-transform.position,transform.forward)< sightAngle)
				{
					//Debug.DrawRay("ai",transform.position,viableTargets[0].transform.position - transform.position);
					//If the target is within the sight distance of the AI continue
					if(Vector3.Distance(transform.position,viableTargets[i].transform.position) < sightDistance)
					{
						//If the target is within Line of Sight continue
						if(Physics.Raycast(transform.position,viableTargets[i].transform.position-transform.position,out hit))
						{
							//If sight is true continue
							if(hit.collider.tag == viableTargets[i].tag)
							{
								//Set this as the target
								target = hit.collider.gameObject;
								//GetComponent<AI_movement>().ChangeNavPoint(GetComponent<AI_main>().AI_target.transform);
								seesTarget = true;
							}
						}
					}
					else
					{
						//Log.M ("ai", "OUT OF DISTANCE");
					}
				}
				else
				{
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
}
