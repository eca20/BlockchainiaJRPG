using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace FPSRetroKit
{
	public class SavingController : MonoBehaviour
	{


		//SAVE & LOAD
		//Active prefabs in the scene
		private PlayerHealth playerHealthScript; //Take PlayerHealth Script to be gathered information from
		[Space(5)]
		//Inactive objects have to be set manually in the inspector to take variables from them. Otherwise script can't see inactive objects 
		[Header("Save Deactivated Objects Information")]
		public Pistol pistolScript; //Set in the Inspector (inactive object pistol)
		public RocketLauncher rocketLauncherScript; //Set in the Inspector (inactive object rocket)
		public Shotgun shotgunScript; //Set in the Inspector (inactive object shotgun)

		[Space(5)]
		[Header("UI Text Save/Load")]
		public GameObject gameSavedText;
		public GameObject gameLoadedText;


		public void Awake()
		{
			playerHealthScript = FindObjectOfType<PlayerHealth>(); //Find PlayerHealth at the beginning
																   //We don't do that with weapons because we set them before as public in the inspector 
		}

		#region Save/Load Game MANAGER (Update v1.6)
		public void SaveGame()
		{
			SaveSystem.SavePlayer(playerHealthScript); //Save SavePlayer Function from SaveSystem Script
			SaveSystem.SaveWeapons(pistolScript, shotgunScript, rocketLauncherScript); //Save SaveWeapon Function from SaveSystem Script
			Invoke("GameSavedText", 1f); //Show message of game Saved
		}

		public void LoadGame()
		{
			PlayerData dataPlayer = SaveSystem.LoadPlayer(); //Load player to PlayerData from Save System script
			PlayerData dataWeapons = SaveSystem.LoadWeapons(); //Load weapons to PlayerData from Save System script

			#region Player Variables
			playerHealthScript.levelScore = dataPlayer.levelScore;
			playerHealthScript.health = dataPlayer.health;
			playerHealthScript.armor = dataPlayer.armor;

			Vector3 position;
			position.x = dataPlayer.position[0];
			position.y = dataPlayer.position[1];
			position.z = dataPlayer.position[2];
			playerHealthScript.transform.position = position;
			#endregion

			#region Load Weapon Data
			pistolScript.ammoLeft = dataWeapons.ammoAmountPistol;
			pistolScript.ammoClipLeft = dataWeapons.ammoClipSizePistol;

			shotgunScript.ammoClipLeft = dataWeapons.ammoShotgunClipSizeLeft;
			shotgunScript.ammoLeft = dataWeapons.ammoShotgunLeft;

			rocketLauncherScript.rocketsAmount = dataWeapons.ammoRocketLeft;
			rocketLauncherScript.rocketsLeft = dataWeapons.ammoRocketClipSizeLeft;
			#endregion


			Invoke("GameLoadedText", 1f); //Show message of game loaded
		}
		#endregion



		public void GameSavedText()
		{
			gameSavedText.SetActive(false);
		}

		public void GameLoadedText()
		{
			gameLoadedText.SetActive(false);
		}
	}
}
