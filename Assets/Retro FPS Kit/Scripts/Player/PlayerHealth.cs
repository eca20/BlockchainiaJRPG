using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPSRetroKit
{
	public class PlayerHealth : MonoBehaviour
	{
		//Player Stats Script

		[Header("Player Max Stats")]
		public int maxHealth; //Max Player's health
		public int maxArmor; //Max Player's Armour

		[Header("Player Starting Stats")]
		public int healthOnStart; //How much health On Start we have (for gamemodes like hardcore, we can set 1)
		public int armourOnStart; //How much armour we have on Start
		public float levelScore; //Score or Level of the player

		[Header("Player Current Health (Hidden)")]
		[HideInInspector] public float armor; //armour value in this script
		[HideInInspector] public float health; //health value in this script

		[Header("Player Effects")]
		public AudioClip hit; //Audio on damage received
		public FlashScreen flash; //Make flashscreen (blood on being hit)

		[Header("Player UI Canvas")]
		public Text healthText; //Health Text to be displayed on screen
		public Text armorText; //Armour Text to be displayed on screen
		public Text levelScoreText; //Score Text to be displayed on screen

		[Space(5)]

		//public Image playerFaceSpriteRenderer; //Rendering Sprites (we changed this to animations below - I've left this line just in case)
		public Animator faceAnim; //Animator will show different animations of face depends of our life %


		AudioSource source; //Audiosource to play sounds 



		//On Start of Game
		void Start()
		{
			armor = armourOnStart; //Player has 0 armour on start (set in inspector)
			health = maxHealth; //Player has max health on start

			//health = healthOnStart; //If you want player to have different health on start - comment above line and uncomment this one
			source = GetComponent<AudioSource>();
		}

		private void Update()
		{
			armor = Mathf.Clamp(armor, 0, maxArmor);
			health = Mathf.Clamp(health, -Mathf.Infinity, maxHealth);

		}

		//Some UI Extras like changing health colour, face sprites, icons etc
		public void FixedUpdate()
		{

			#region Show Text of health and armour & score
			//Show how much health left on the screen and change its colour based on amount left
			if (armor > 0) //if we have armour - let's show it on screen
			{
				armorText.text = "Armour" + "\n" + armor + "%";
				healthText.text = "Health" + "\n" + health + "%";
			}
			else if (armor <= 0) //if we don't have armour - don't show armour at all
			{
				armorText.text = "No Armour";
				healthText.text = "Health" + "\n" + health + "%";
			}

			//Show Score text 
			levelScoreText.text = "Score" + "\n" + levelScore;

			#endregion

			#region Health % change colour & Change Face Graphics

			if (health > 75) //If Health is full or more than 75, show green health value
			{
				faceAnim.Play("100%HP");
			}
			if (health <= 75 && health >= 50) //If health is in middle make it yellow
			{
				faceAnim.Play("75%HP");
				//playerFaceSpriteRenderer.sprite = playerFaceImages[1]; //this line is to remember how to make NOT ANIMATED face depending on life %
				healthText.color = Color.yellow;
			}
			if (health < 50) //If health is less than 50 make it red
			{
				faceAnim.Play("50%HP");
				healthText.color = Color.red;
			}

			#endregion

		}

		//Add health from boxes (PlayerMovement.script)
		public void AddHealth(float value)
		{
			health += value;
		}

		//Add armour from armour boxes (PlayerMovement.script)
		public void AddArmor(float value)
		{
			armor += value;
		}

		//Level Score of the player (points got by defeating enemies)
		public void AddScore(float value)
		{
			levelScore += value;
		}

		//If Player has been hit by enemy
		void EnemyHit(float damage)
		{
			//If we have armour then substract armour by enemy's damage value 
			if (armor > 0 && armor >= damage)
			{
				armor -= damage;
			}
			//If we have some armour then substract whole armour and add rest of the damage to health
			else if (armor > 0 && armor < damage)
			{
				damage -= armor;
				armor = 0;
				health -= damage;
			}
			//If we don't have armour, substract health by damage value
			else
			{
				health -= damage;
			}
			source.PlayOneShot(hit); //And Play once sound of being hit
			flash.TookDamage(); //Put Flash on UI Screen (blood) after being hit
		}


	}
}