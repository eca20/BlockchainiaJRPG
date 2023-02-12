using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
	public class PlayerManager : MonoBehaviour
	{
		PlayerMovement playerMovementScript;
		PlayerHealth playerHealthScript;


		[Header("Menus UI")]
		[SerializeField] GameObject deathScreen;
		[SerializeField] GameObject pauseMenuScreen;


		public void Start()
		{
			Time.timeScale = 1;
			playerMovementScript = GetComponent<PlayerMovement>();
			playerHealthScript = GetComponent<PlayerHealth>();
		}

		public void Update()
		{
			//Pause Menu Activation only if we have life
			if (Input.GetKeyDown(KeyCode.Escape) && playerHealthScript.health > 0)
			{
				PauseMenu();
			}
		}

		public void FixedUpdate()
		{
			if (playerHealthScript.health <= 0) //if we have 0 health, show Death Screen
			{
				Death();
			}
		}

		//Pause Menu Function
		public void PauseMenu()
		{
			if (Time.timeScale == 1) //If Pause Menu is not active
			{
				Cursor.lockState = CursorLockMode.None; //show cursor
				Cursor.visible = true; //show cursor
				Time.timeScale = 0; //Pasue Game
				pauseMenuScreen.SetActive(true); //Show Pause Screen
			}
			else if (Time.timeScale == 0) //If Pause Menu is active
			{
				Cursor.lockState = CursorLockMode.Locked; //lock cursor
				Cursor.visible = false; //disable cursor 
				Debug.Log("PauseMenu Closed");
				Time.timeScale = 1; //start game
				pauseMenuScreen.SetActive(false); //disable pause screen
			}
		}

		//Death Function
		public void Death()
		{
			if (playerHealthScript.health <= 0) //if player has no health
			{
				Cursor.lockState = CursorLockMode.None; //Unlock Cursor
				Cursor.visible = true;
				Time.timeScale = 0; //Pause Time in Game (we can't move, can't shoot, nothing)
				deathScreen.SetActive(true); //show death screen
			}
			else //if player is not dead
			{
				Cursor.lockState = CursorLockMode.Locked; //Lock Cursor
				Cursor.visible = false;
				Time.timeScale = 1; //Pause Time in Game (we can't move, can't shoot, nothing)
				deathScreen.SetActive(false); //show death screen
			}

		}

	}
}
