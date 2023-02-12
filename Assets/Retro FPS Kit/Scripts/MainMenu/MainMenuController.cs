using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace FPSRetroKit
{
	public class MainMenuController : MonoBehaviour
	{
		[Header("Options System")]
		[SerializeField] string firstSceneName; //name of your first scene to load on start game
		[SerializeField] GameObject optionsPanel; //Options Panel to be displayed
		[Space(5)]

		//Resolution System
		Resolution[] resolutions; //Resolutions of your Player's Computer
		[Header("Resolution System")]
		public Dropdown resolutionDropdown; //Our DropDown from resolution slider



		public void Start()
		{
			optionsPanel.SetActive(false); //Turn Off options panel on start the game

			#region Resolution Settings
			//Taking the resolutions from the player's computer
			resolutions = Screen.resolutions;

			resolutionDropdown.ClearOptions(); //Clearing Options for each PC (every screen has different resolution)

			List<string> options = new List<string>(); //creating new list of resolutions

			int currentResolutionindex = 0; //Getting the default,main resolution (best one)

			//Creating new resolutions to the drop-down menu in Settings
			for (int i = 0; i < resolutions.Length; i++)
			{
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);

				//Checking Computer's resolutions and setting it as default in options
				if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionindex = i;
				}
			}

			//Adding options to the drop-down menu
			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionindex; //Getting our default resolution
			resolutionDropdown.RefreshShownValue(); //Refreshing the values

			#endregion //Resolution Settings in game settings

		}

		#region Main Menu Buttons
		public void StartGame()
		{
			SceneManager.LoadScene(firstSceneName);
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void BackMainMenu()
		{
			SceneManager.LoadScene("MainMenu");
		}

		//Reloads the Level
		[System.Obsolete]
		public void Reload()
		{
			Application.LoadLevel(Application.loadedLevel);
		}


		public void OpenOptions()
		{
			optionsPanel.SetActive(true);
		}

		public void CloseOptions()
		{
			optionsPanel.SetActive(false);
		}
		#endregion

		//Settings (Update v1.2)
		#region Settings

		public AudioMixer audioMixer; //Mixer that is responsible for our in-game Audio. Can be found under Audio Folder

		//Changing Volume of the Game
		public void SetVolume (float volume)
		{
			audioMixer.SetFloat("Volume", volume);
		}

		//Changing Graphics Settings
		public void SetQuality (int qualityIndex)
		{
			QualitySettings.SetQualityLevel(qualityIndex);
		}

		//Fullscreen Toggle
		public void SetFullscreen (bool isFullscreen)
		{
			Screen.fullScreen = isFullscreen;
		}

		//Changing the Resolution

		public void SetResolution(int resolutionIndex)
		{
			Resolution resolution = resolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		#endregion

	}
}
