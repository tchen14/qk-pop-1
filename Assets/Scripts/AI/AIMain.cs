using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
/*! 
 *	This code is the main ai controller. The class is segmented into the following regions: attack, movement, state, sight, and health
 */
public class AIMain : MonoBehaviour {
	public bool aggression 		= false;					/*!<If the NPC will attack the player */
	public float aggressionLimit 	= 100;					/*!<The aggression level of the attacker */
	public int hp 				= 100;						/*!<Health of the NPC */
	public float sightDistance 	= 20;						/*!<The distance the NPC is capable of seeing */
	public float sightAngle		= 35;						/*!<The max angle of the cone of vision */
	public float attackDistance	= 3;						/*!<The distance the NPC stands away from the target and attacks* */
	public Transform startPoint = null;						/*!<Sets the first navPoint, defaults to stationary */
	public float speed 			= 5;						/*!<Casual Speed of the NPC */
	public float runSpeed 		= 8;						/*!<Scared, Charging, Aggressive Speed of the NPC* */
	public bool seesTarget 		= false;					/*!<If the Player has been spotted */
	public GameObject target	= null;						/*!<The transform of the player */
	public string seekTag 		= "Player";					/*!<The enemy Tag of this NPC */
	public bool attacking 		= false;					/*!<If the AI is attacking */
	public bool panic 			= false;					/*!<If the AI is panicking */
	public Vector3 panicTarget	= new Vector3 (0, 0, 0);	/*!<Target of AI panic */

	//movement variables
	private NavMeshAgent mesh = null;						/*!<Contains the component to use the navmesh */
	public Vector3 navPoint;								/*!<Contains the point to move in the navmesh */

	//state variables
	private float aggressionLevel = 0;							

	//sight variables
	private RaycastHit hit;									/*!<Takes information from RayCast */
	private GameObject[] viableTargets;						/*!<All the available targets */

	//! Unity Start function
	void Start() {
		#region movement
		//if no navMesh has been given to this AI then it will create one, AI needs navMesh
		mesh = GetComponent<NavMeshAgent>();
		ChangeNavPoint(GetComponent<AIMain>().startPoint.position);
		SetSpeed (GetComponent<AIMain> ().speed);
		#endregion

		#region state
		GetComponent<AIMain>().panicTarget = GameObject.Find("Panicpoints").GetComponent<PanicTargets>().GetPanickPoint();
		#endregion
    }

    //! Unity Update function
    void Update() {
    	#region attack
        //Log.M ("ai", Vector3.Distance(transform.position, GetComponent<AI_main>().AI_target.transform.position));
        if (GetComponent<AIMain>().target != null) {
            if (GetComponent<AIMain>().attackDistance >= Vector3.Distance (transform.position, GetComponent<AIMain>().target.transform.position)) {
                GetComponent<AIMain>().attacking = true;
                //GetComponent<AI_movement>().ChangeNavPoint(transform);
                Log.M ("ai","ATTACK!");
                GetComponent<AIMain> ().target = null;
            }
        }
        #endregion

        #region movement
        mesh.SetDestination(navPoint);
        #endregion

        #region sight
        //if there is no target then look for a target
		if(GetComponent<AIMain>().aggression == true && GetComponent<AIMain>().target == null)
		{
			StartCoroutine("CheckForTargets");

		}
        #endregion
    }

    //! Unity FixedUpdate function
	void FixedUpdate () {
		#region state
		if(GetComponent<AIMain>().panic == true)
		{
			if(GetComponent<AIMain>().aggression == true)
			{
				GetComponent<AIMain>().panic =false;
			}
			else
			{
				ChangeNavPoint(GetComponent<AIMain>().panicTarget);
				SetSpeed(GetComponent<AIMain>().runSpeed);
			}
		}
		//If the AI has a target already
		if(GetComponent<AIMain>().aggression == true && GetComponent<AIMain>().seesTarget == true && GetComponent<AIMain>().attacking == false)
		{
			Log.M ("ai", aggressionLevel.ToString());
			if(aggressionLevel >= GetComponent<AIMain>().aggressionLimit)
				ChangeNavPoint(GetComponent<AIMain>().target.transform.position);
			else
				aggressionLevel += 1;
		}
		else
		{
			if(aggressionLevel > 0)
			{
				aggressionLevel -= 1;
			}
		}
		#endregion
	}

	#region attack
    //! Wait time between attacks???
    public IEnumerator EndAttack() {
        yield return new WaitForSeconds (1);
    }
    #endregion

    #region movement
    //sets a new destination for the AI and can be publicly accessed
	public void ChangeNavPoint(Vector3 T)
	{
		navPoint = T;
	}
	public void SetSpeed(float Spd)
	{
		mesh.speed = Spd;
	}
	public void Pause()
	{
		Log.M ("ai", "W");
		mesh.Stop();
	}
	#endregion

	#region sight
	//called every tenth of a second
	public IEnumerator CheckForTargets()
	{

		yield return new WaitForSeconds(.1f);
		GetTargets();

	}
	//Sends Raycast and sets up AI_Main
	private void GetTargets()
	{
		//Gathers proabable enemy targets
		GameObject[] viableTargets = GameObject.FindGameObjectsWithTag(GetComponent<AIMain>().seekTag);

		for(int i = 0; i < viableTargets.Length; i++)
		{
			if(Vector3.Angle(viableTargets[i].transform.position-transform.position,transform.forward)< GetComponent<AIMain>().sightAngle)
			{
				//Debug.DrawRay(transform.position,viableTargets[0].transform.position - transform.position);
				if(Vector3.Distance(transform.position,viableTargets[i].transform.position) < GetComponent<AIMain>().sightDistance)
				{
					if(Physics.Raycast(transform.position,viableTargets[i].transform.position-transform.position,out hit))
					{
						if(hit.collider.tag == viableTargets[i].tag)
						{
							Log.M ("ai", "GOT THE BASTARD");
							GetComponent<AIMain>().target = hit.collider.gameObject;
							//GetComponent<AI_movement>().ChangeNavPoint(GetComponent<AI_main>().AI_target.transform);
							GetComponent<AIMain>().seesTarget = true;
						}
					}
				}
				else
				{
					Log.M ("ai", "OUT OF DISTANCE");
				}
			}
			else
			{
				Log.M ("ai", "out of angles");
			}
		}
	}
	#endregion

	#region health
	public int hurt (int damage) {
        int HP = GetComponent<AIMain> ().hp;
        HP -= damage;
        return HP;
    }
    public int heal (int health) {
        int HP = GetComponent<AIMain> ().hp;
        HP += health;
        return HP;
    }
    #endregion
}
