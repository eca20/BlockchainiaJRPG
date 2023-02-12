//ENEMY script. Health of Enemy and what happens after death.

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace FPSRetroKit
{

	public class Enemy : MonoBehaviour
	{
		[Header("Monster Health and Death Sprite")]
		public Sprite deadBody; //Sprite of deadbody to be shown if health is 0 or below.
		public int maxHealth; //Maximum Health of the Enemy
		public float health; //Actual health of the Enemy (while gameplay)

		#region MONSTER ATTACK PHASES VARIABLES
		//MONSTER ATTACK PHASES (Public - you select it from inspector | Below public one is a function that activates it in the script)
		[Header("ENEMY ATTACK PHASES (on lower health)")]

		public bool fasterWalking; //Select it in the inspector to activate the super power
		[Tooltip("Multiplication of the normal Speed")] [Range(1f, 50f)] public float walkingSpeed; //Change how much faster the walking speed should be
		[HideInInspector] bool fasterWalkingFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool fasterShooting; //Select it in the inspector to activate the super power
		[Tooltip("Multiplication of the normal Speed")] [Range (1f,10f)] public float attackSpeed; //Change how much faster shooting should be (multiply)
		[HideInInspector] bool fasterShootingFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool strongerShooting; //Select it in the inspector to activate the super power
		[Tooltip("Multiplication of the normal Damage")] [Range(1f, 50f)] public float damageMultiply; //Change how much stroger shooting should be (multiply)
		[HideInInspector] bool strongerShootingFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool onlyMelee; //Select it in the inspector to activate the super power
		[HideInInspector] bool onlyMeleeFunction; //Functionality to be activated within this script
		[Space(5)]
		public bool MeleeDamage; //How much multiply the malee damage 
		[Range (1f, 50f)] public float MeleeDamageMultiply; //Select it in the inspector to activate the multiplication of malee damage
		[HideInInspector] bool MeleeDamageFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool changeDelayAttack; //Select it in the inspector to activate the super power
		[Tooltip("Every X seconds monster will attack")] [Range(0f, 100f)] public float attackDelayTime; //How often should monster attack (seconds)
		[HideInInspector] bool changeDelayAttackFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool monsterSize; //Select it in the inspector to activate the super power
		[Tooltip("Multiply Monster Size")] [Range(0f, 20f)] public float monsterSizeMultiply; //How often should monster attack (seconds)
		[HideInInspector] bool monsterSizeFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool monsterColourChange; //Select it in the inspector to activate the super power
		[Tooltip("ChangeMonsterColour")] public Color monsterColourChoose; //How often should monster attack (seconds)
		[HideInInspector] bool monsterColourChangeFunction; //Functionality to be activated within this script

		[Space(15)]
		public bool healthRegeneration; //Select it in the inspector to activate the super power
		[HideInInspector] private float healthRegenerationTimer = 0; //Timer for the health recovery
		[Tooltip("How long (in seconds) Should Monster regenerate Health")] public float healthRegenerationTime;
		[Tooltip("How much health each second should monster recover")] [Range(1f, 100f)] public float healthRegenerationAmount;
		[HideInInspector] bool healthRegenerationFunction; //Functionality to be activated within this script
		#endregion

		[Header("How Much Health to activate super attack")]
		[Tooltip("It is correlant to the maxHealth of the Enemy")] public float activatePowerBelow; //Below this value (set in inspector) super attacks will be activated
		[Header("What Player gets after defeating this enemy")]
		public float scoreAfterDefeat = 104f; //After defeating this monster, Player gets 104 score
		bool scoreReceived = false;

		//Take components from Enemy Object
		EnemyStates es;
		NavMeshAgent nma;
		[HideInInspector] public SpriteRenderer sr;
		BoxCollider bc;
		GameObject vision;
		DynamicBillboardChange dbc;
		Animator anim;
		Rigidbody rgbody;

		GameObject player; //Player Prefab

		void Awake()
		{
		
		}

		//Let the script understand and take all the components
		private void Start()
		{
			health = maxHealth;
			es = GetComponent<EnemyStates>();
			sr = GetComponent<SpriteRenderer>();
			bc = GetComponent<BoxCollider>();
			rgbody = GetComponent<Rigidbody>();
			anim = GetComponent<Animator>();
			nma = GetComponent<NavMeshAgent>();


			//Getting Player statistics to send him the score
			player = GameObject.Find("Player"); //Finding Player on the scene (he has no parents) to add parents put "name of parent/Player "
		}


		private void Update()
		{
			//On Enemy's death (if enemy has less or equal to 0 health
			if (health <= 0)
			{
				anim.enabled = false; //disable animations
				es.enabled = false; //disable shooting scripts
				nma.enabled = false; //Disable walking scripts
				sr.sprite = deadBody; //Change sprite from walking to DeadBody (need to be assigned in inspector)
				rgbody.useGravity = false; //Disable gravity in the object
				bc.center = new Vector3(0, -0.8f, 0); //Change height of the collider (player might need to jump over body) (Can be commented)
				bc.size = new Vector3(1.05f, 0.43f, 0.2f); //change size of the collider 

				//Fix floating body after enemy's death - freezing all positions so it cannot move after death
				rgbody.constraints = RigidbodyConstraints.FreezeAll;

				//Adding Score only Once to the Player
				if(scoreReceived == false)
				{
					//Adding Score to the Player by defeating the monster
					player.GetComponent<PlayerHealth>().AddScore(scoreAfterDefeat);
					scoreReceived = true; //Player received their score
				}


			}
		}

		//DIFFERENT TYPE OF ATTACKS (PHASES) (v1.5)
		public void FixedUpdate()
		{

			#region TEMPLATE OF THE SCRIPT SO YOU COULD ADD MORE PHASES YOURSELF
			//Below line means: if (health is lower than set in inspector AND superPower is activated in inspector AND Power Wasn't activated before (this is security) THEN
			/*	if (health <= activatePowerBelow && fasterShooting == true && fasterShootingFunction == false)
				{

				//Here you can add anything to change witin the enemy examples are below in script. For example: add more health! (health recovery)
				// then
				//Set that the power has been activated:	fasterShootingFunction = true;

				}
			*/
			#endregion

			#region MONSTER WALKING SPEED
			//If Health is lower than selected by you & Power is activated & was not activated before
			if (health <= activatePowerBelow && fasterWalking == true && fasterWalkingFunction == false)
			{
				this.gameObject.GetComponent<NavMeshAgent>().speed = walkingSpeed; // Changing walking speed of the monster
				fasterWalkingFunction = true;
			}
			#endregion

			#region FASTER SHOOTING AND MELEE
			//If Health is lower than selected by you & Faster Shooting is selected & it wasn't activated before
			if (health <= activatePowerBelow && fasterShooting == true && fasterShootingFunction == false) 
			{
				fasterShootingFunction = true;
				es.missileSpeed = es.missileSpeed * attackSpeed; //Missile Speed is multiplied by number given in the inspector
				es.meleeDamage = es.meleeDamage * attackSpeed;
			}
			#endregion

			#region STRONGER DAMAGE (SHOOTING)
			//If Health is lower than selected by you & Stronger Shooting is selected & it wasn't activated before
			if (health <= activatePowerBelow && strongerShooting == true && strongerShootingFunction == false)
			{
				es.missileDamage = es.missileDamage * damageMultiply; //Missiles do double damage
				strongerShootingFunction = true;
			}
			#endregion

			#region ONLY MELEE & MELEE DAMAGE
			//If Health is lower than selected by you & Only Melee is selected & it wasn't activated before
			if (health <= activatePowerBelow && onlyMelee == true && onlyMeleeFunction == false)
			{
				es.onlyMelee = true; // Activate onlyMelee attacking for monster under "activatePowerBelow" health
				onlyMeleeFunction = true;
			}

			//MELEE DAMAGE
			if (health <= activatePowerBelow && MeleeDamage == true && MeleeDamageFunction == false)
			{
				es.meleeDamage = es.meleeDamage * MeleeDamageMultiply; // Activate onlyMelee attacking for monster under "activatePowerBelow" health
				MeleeDamageFunction = true;
			}
			#endregion

			#region ATTACK DELAY CHANGE
			//If Health is lower than selected by you & Power is activated & was not activated before
			if (health <= activatePowerBelow && changeDelayAttack == true && changeDelayAttackFunction == false)
			{
				es.attackDelay = attackDelayTime; //How often should monster attack (in seconds)
				changeDelayAttackFunction = true;
			}
			#endregion

			#region MONSTER SIZE MULTIPLY
			//If Health is lower than selected by you & Power is activated & was not activated before
			if (health <= activatePowerBelow && monsterSize == true && monsterSizeFunction == false)
			{
				this.gameObject.transform.localScale = new Vector3(transform.localScale.x * monsterSizeMultiply, transform.localScale.y * monsterSizeMultiply,
					transform.localScale.z * monsterSizeMultiply);  //Change monster Size (Multiply from Inspector)
				monsterSizeFunction = true;
			}
			#endregion

			#region MONSTER COLOUR CHANGE
			//If Health is lower than selected by you & Power is activated & was not activated before
			if (health <= activatePowerBelow && monsterColourChange == true && monsterColourChangeFunction == false)
			{
				this.gameObject.GetComponent<SpriteRenderer>().color = monsterColourChoose; //Choose new colour for the monster
				monsterColourChangeFunction = true;
			}
			#endregion

			#region HEALTH REGENERATION 
			//If Health is lower than selected by you & Power is activated & was not activated before
			if (health <= activatePowerBelow && healthRegeneration == true && healthRegenerationFunction == false)
			{
				healthRegenerationTimer += Time.deltaTime; //Set Timer
				if (healthRegenerationTimer <= healthRegenerationTime) //If Regeneration lasts less than set in inspector
				{
					if (health < maxHealth) //and that health should not exceed maximum monster's health
					{
						health = Mathf.Min(health + (healthRegenerationAmount * Time.deltaTime), maxHealth); //Then add +RegenerationAmount each second
					}
				}
				else //If our regeneration Time is over
				{
					healthRegenerationFunction = true; //Stop regenerating
				    healthRegenerationTimer = 0; //And set timer 0
				}
			}

			IEnumerator RegenHealth()
			{
				health += 1;
				yield return new WaitForSeconds(1);
				healthRegenerationFunction = true;
			}
			#endregion

		}


		//Damage dealt to the enemy is being reduced from enemy's health
		void AddDamage(float damage)
		{
			health -= damage;
		}
	}
}