using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script saves Health, Armour, Ammo, Score and Player Position on the Map

//This script is going to be updated with new things to be saved. For now please reffer to the tutorial:
// https://www.youtube.com/watch?v=XOjd_qU2Ido 

namespace FPSRetroKit
{
	[System.Serializable]
	public class PlayerData
	{
		//Player Variables to be saved
		public float armor; //Player's armour
		public float health; //Player's health
		public float levelScore; //Player's score
		public float[] position; //Player position on the map

		//Weapon Variables to be saved
		public int ammoAmountPistol;
		public int ammoClipSizePistol;

		public int ammoRocketLeft;
		public int ammoRocketClipSizeLeft;

		public int ammoShotgunLeft;
		public int ammoShotgunClipSizeLeft;

		public float enemyHealth;

		//THINGS TO BE SAVED IN THE FUTURE:
		//ENEMIES POSITIONS / ENEMIES HEALTH
		//HIGHSCORE
		//BONUSES POSITIONS/ACTIVE/INACTIVE STATUS
		//Health.Text Colour

		#region Getting Player Data
		//Functionality of taking Data from our Player Script to be saved in this script. PlayerHealth Script
		public PlayerData (PlayerHealth playerHealthScript)
		{
			armor = playerHealthScript.armor;
			health = playerHealthScript.health;
			levelScore = playerHealthScript.levelScore;

			position = new float[3];
			position[0] = playerHealthScript.transform.position.x;
			position[1] = playerHealthScript.transform.position.y;
			position[2] = playerHealthScript.transform.position.z;
		}
		#endregion

		#region Getting Weapons Data
		//Functionality of saving our Players Ammo for each weapon
		public PlayerData (Pistol pistolScript, Shotgun shotgunScript, RocketLauncher rocketLauncherScript)
		{

			//Pistol
			ammoAmountPistol = pistolScript.ammoLeft;
			ammoClipSizePistol = pistolScript.ammoClipLeft;

			ammoRocketLeft = rocketLauncherScript.rocketsAmount;
			ammoRocketClipSizeLeft = rocketLauncherScript.rocketsLeft;

			ammoShotgunLeft = shotgunScript.ammoLeft;
			ammoShotgunClipSizeLeft = shotgunScript.ammoClipLeft;
		}
		#endregion

		public PlayerData (Enemy enemyScript)
		{
			enemyHealth = enemyScript.health;
		}
	}
}
