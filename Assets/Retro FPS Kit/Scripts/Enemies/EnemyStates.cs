
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

//ENEMY SCRIPT 2 - ALMOST ALL FUNCTIONALITIES (HEALTH AND DEATH IS IN ENEMY.SCRIPT)
namespace FPSRetroKit
{
	public class EnemyStates : MonoBehaviour
	{
		[Header("Range of Action Functions")]
		public Transform[] waypoints; //Waypoints (empty game objects) where This enemy will walk between
		public int patrolRange; //From How far will enemy see the player
		public int attackRange; //From How far will enemy attack player with malee
		public int shootRange; //From how far will enemy shoot the player

		[Header("Visibility Functions")]
		public Transform vision; //Child Object from Enemy (Vision object has script that rotates enemy towards his destination target)
		public float stayAlertTime; //After how long enemy will forget about Player after finishing conquer
		public float viewAngle; //What's the Enemy's view angle

		[Header("Attack Settings")]
		public GameObject missile; //Missile Prefab (what he shoots of)
		public float missileDamage; //How much Damage will it deal to the Player
		public float missileSpeed; //How fast the missile is going to travel to the Player

		public bool onlyMelee = false; //Should enemy attack only from close distance? True = yes, false = it can shoot range
		public float meleeDamage; //How much damage malee deals
		public float attackDelay; //How often enemy attacks with malee (how many seconds he needs to wait to attack again)

		public LayerMask raycastMask; //Visibility of the Enemy 

		//States of the Enemy
		[HideInInspector]
		public AlertState alertState; //Enemy knows about us
		[HideInInspector]
		public AttackState attackState; //Enemy starts attacking us
		[HideInInspector]
		public ChaseState chaseState; //Enemy going after us
		[HideInInspector]
		public PatrolState patrolState; //Enemy patrols the area
		[HideInInspector]
		public IEnemyAI currentState; //Output current state to other scripts
		[HideInInspector]
		public NavMeshAgent navMeshAgent; //NavMesh (it makes the enemy understand where to go, what to follow, how to walk and patrol)
		[HideInInspector]
		public Transform chaseTarget; //Who Enemy should chase? (Player)
		[HideInInspector]
		public Vector3 lastKnownPosition; //If enemy hits us then he knows we where there. This is his point.
										  //I will develop it, so he starts patroling that area next. Once he knows our last location - he will patrol new area to find us.
		void Awake()
		{
			// Creating each state instance
			// Putting instances to the EnemyStates Object
			alertState = new AlertState(this);
			attackState = new AttackState(this);
			chaseState = new ChaseState(this);
			patrolState = new PatrolState(this);
			navMeshAgent = GetComponent<NavMeshAgent>();
		   
		}

		void Start()
		{
			// This is the beginning state. Enemy starts by patrolling
			currentState = patrolState;
		}

		void Update()
		{
			// Each frame this script checks what state the Enemy is currently on. To follow accordingly with actions
			currentState.UpdateActions();
		}

		void OnTriggerEnter(Collider otherObj)
		{
			//If Enemy collides with Triggers - do the action corresponding to the state enemy's currently on
			//Idea: We can make that if he patrols, he does different actions to the walls than when he attacks etc.
			currentState.OnTriggerEnter(otherObj);
		}

		// This function takes our Player's shots to understand its location and become alerted if Player shoots around enemy
		//Last known shoot location is being tracked and saved 
		void HiddenShot(Vector3 shotPosition)
		{
			Debug.Log("Ktoś strzelił");
			lastKnownPosition = shotPosition;
			currentState = alertState;
		}

		// Function takes the angle and distance between Player and Enemy's eyes sight vector (where Enemy looks).
		// If Enemy sees the Player, it changes last known position and gives "true" value as "enemy spotted".
		// If enemy wasn't looking at the Player or can't see him then spotted value is false.
		public bool EnemySpotted()
		{
			Vector3 direction = GameObject.FindWithTag("Player").transform.position - transform.position; //Take distance between enemy and player
			float angle = Vector3.Angle(direction, vision.forward); //check angle and if enemy is looking at you or not

			//If Player is in the angle view and shot to the Enemy, Enemy should chase after the Player
			if (angle < viewAngle * 0.5f)
			{
				RaycastHit hit;
				if (Physics.Raycast(vision.transform.position, direction.normalized, out hit, patrolRange, raycastMask))
				{
					if (hit.collider.CompareTag("Player"))
					{
						chaseTarget = hit.transform;
						lastKnownPosition = hit.transform.position;
						return true;
					}
				}
			}
			return false;
		}
	}
}