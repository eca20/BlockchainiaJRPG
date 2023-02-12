using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPSRetroKit
{
	[RequireComponent(typeof(CharacterController))] //This script needs charactercontroller component in Prefab
	public class PlayerMovement : MonoBehaviour
	{

		[Header("Player Settings")]
		public float playerWalkingSpeed = 5f; //How fast player walks
		public float playerRunningSpeed = 15f; //How fast player runs
		public float jumpStrength = 20f; //How Height/Strong player jumps
		public float verticalRotationLimit = 80f; //How far can player move mouse up and down (sky/legs)

		[Header("Player Extras")]
		public AudioClip pickupSound; //Sound to play on pickup ammo/health etc.
		public FlashScreen flash; //Take Flash effect to Player's screen

		[Header("Pickup Bonuses values")]
		public float HealthBoxPickup = 20f; //Health Pickup
		public float ArmourBoxPickup = 50f; //Armour Pickup
		public int AmmoBoxPickup = 15; //Ammo Pistol Pickup
		public int AmmoShotgunPickup = 2; //Shotgun Ammo Pickup

		float forwardMovement; //Movement front
		float sidewaysMovement; //Movement left, right

		float verticalVelocity;

		float verticalRotation = 0; //Rotation of the camera is set to centre
		CharacterController cc; //Character controller is named "cc" in this script
		AudioSource source; //Take audiosrouce as "source" in this script

		//On start game - or when player is activated
		void Awake()
		{
			cc = GetComponent<CharacterController>();
			source = GetComponent<AudioSource>();
			Cursor.visible = false; //disable cursor visibility
			Cursor.lockState = CursorLockMode.Locked; //Make cursor locked so it does not go off the screen
		}

		void Update()
		{
			//Using mouse to look left/right
			float horizontalRotation = Input.GetAxis("Mouse X");
			transform.Rotate(0, horizontalRotation, 0);

			//Using mouse to look up/down
			verticalRotation -= Input.GetAxis("Mouse Y");
			verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
			Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

			//Character Movement
			//Only if player is on the ground (floor is detected)
			if (cc.isGrounded)
			{
				forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed;
				sidewaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;

				//Run on left shift button
				if (Input.GetKey(KeyCode.LeftShift))
				{
					forwardMovement = Input.GetAxis("Vertical") * playerRunningSpeed;
					sidewaysMovement = Input.GetAxis("Horizontal") * playerRunningSpeed;
				}
				if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
				{

					//Expand crosshair (add more spread) on running
					if (Input.GetKey(KeyCode.LeftShift))
					{
						DynamicCrosshair.spread = DynamicCrosshair.RUN_SPREAD;
					}
					else
					{
						DynamicCrosshair.spread = DynamicCrosshair.WALK_SPREAD;
					}
				}
			}
			else
			{
				DynamicCrosshair.spread = DynamicCrosshair.JUMP_SPREAD; //Make bigger crosshair spread on Jump
			}


			// Add proper gravity to the Player
			verticalVelocity += Physics.gravity.y * Time.deltaTime;

			//Jumping using Jump Button (default is space). To be changed in editor settings
			if (Input.GetButton("Jump") && cc.isGrounded)
			{
				verticalVelocity = jumpStrength;
			}

			Vector3 playerMovement = new Vector3(sidewaysMovement, verticalVelocity, forwardMovement);
			//Player movement - calculating movement
			cc.Move(transform.rotation * playerMovement * Time.deltaTime);




		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("HpBonus")) //On Trigger enter to HP box Prefab
			{
				GetComponent<PlayerHealth>().AddHealth(HealthBoxPickup); //Add X health on pickup health box (set in inspector)
			}
			else if (other.CompareTag("ArmorBonus")) //Add X armour on pickup armour box
			{
				GetComponent<PlayerHealth>().AddArmor(ArmourBoxPickup);
			}
			else if (other.CompareTag("AmmoBonus")) //Add X ammo on pickup ammo box
			{
				transform.Find("Weapons").Find("PistolHand").GetComponent<Pistol>().AddAmmo(AmmoBoxPickup);
			}
			else if (other.CompareTag("AmmoShotgunBonus")) //Picking Up Shotgun Ammo
			{
				transform.Find("Weapons").Find("ShotgunHand").GetComponent<Shotgun>().AddAmmo(AmmoShotgunPickup);
			}

			//If Player enters any of those bonuses to pickup them
			if (other.CompareTag("HpBonus") || other.CompareTag("ArmorBonus") || other.CompareTag("AmmoBonus") || other.CompareTag("AmmoShotgunBonus"))
			{
				flash.PickedUpBonus(); //Make flash for PickedUp function (set in FlashScreen.script)
				source.PlayOneShot(pickupSound); //Play sound of picking up once
				Destroy(other.gameObject); //Destroy pickingup object (so we cannot take it twice). 
										   //If you want so player always can pickup ammo only in one place - delete "Destroy" line.

				//In new update, this system will be separated from Player and to be added in pickup boxes. 
			}
		}
	}
}
