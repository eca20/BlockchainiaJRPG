using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPSRetroKit
{
	public class NewLevelScript : MonoBehaviour
	{

		//Level Selection Save&Progress
		public int nextSceneLoad; //Next Level that will be loaded after completing current one

		//[SerializeField] string yourLevelName; //in Inspector you can write your next scene name
		[SerializeField] GameObject openAfterDefeat; //This is the Enemy Object from hierarchy we need to defeat to activate script
		[SerializeField] bool isDefeated;

		bool noEnemySelected;

		void Start()
		{
			//If we do not need to kill any boss to enter next level
			if (openAfterDefeat == null) // we can leave it without assigning any enemy
			{
				Debug.Log("We have no enemy selected to activate new Level Trigger");
				noEnemySelected = true; //No Enemy is being chosen to open the door
				isDefeated = true; //Door will work straight away to the next level without defeating anyone
			}
			else
			{
				Debug.Log("Enemy has been assigned to activate new level");
			}

			//Stating to load next level that is in build setting after completing this one
			nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
		}
		void FixedUpdate()
		{
			//Getting enemy health from his gameobject script
			if (noEnemySelected == false && openAfterDefeat.gameObject.GetComponent<Enemy>().health <= 0 && noEnemySelected == false)
			{
				isDefeated = true; //Once we kill Boss - activate new level trigger
			}
		}
		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && isDefeated == true)
			{
				/* < Change this int value to whatever your last level build index is on your build settings - we have 5 scenes so 5 */
				if (SceneManager.GetActiveScene().buildIndex == 5) 
				{
					Debug.Log("You Completed ALL Levels");
					//Show Win Screen ith HighScore at the completion of all levels(to be done in Future updates)
					SceneManager.LoadScene("LevelSelect"); //For now - load levelSelect menu
				}
				else //if We are not at the last level 
				{
					//Move to next level
					SceneManager.LoadScene(nextSceneLoad);

					//Setting Int for Index
					if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
					{
						PlayerPrefs.SetInt("levelAt", nextSceneLoad);
					}
				}
			}

		}

	}
}
