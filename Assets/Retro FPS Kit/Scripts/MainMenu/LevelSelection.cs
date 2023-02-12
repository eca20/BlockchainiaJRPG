using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
	public Button[] levelButtons; //Our Level buttons to be set in the Inspector

	// Start is called before the first frame update
	void Start()
	{
		int levelAt = PlayerPrefs.GetInt("levelAt", 2); /* < Change this int value to whatever your
															 level selection build index is on your
															 build settings */

		for (int i = 0; i < levelButtons.Length; i++)
		{
			//If we haven't unlocked levels in PlayerPrefs - Buttons Levels are not available (interactable)
			if (i + 2 > levelAt)
				levelButtons[i].interactable = false;
		}
	}

	//Load Level from the Build Settings 
	public void LoadLevel(int levelIndex)
	{
		SceneManager.LoadScene(levelIndex);
	}

}
