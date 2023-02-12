//How Objects turn to our player position

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
	public class DynamicBillboardChange : MonoBehaviour
	{

		[SerializeField]
		Sprite[] sprites; //Sprites that we use for our left, forward, back, right turns
		[SerializeField]
		string[] animStates = new string[4] { "Forward", "Backward", "Left", "Right" }; //Objects/Enemies have 4 angles (4 sprites) 
		[SerializeField]
		bool isAnimated; //Is it Animated?

		Animator anim; //Animator is responsible for showing right animation.
					   //E.G.: Forward means animation for forward walking (made using sprites)

		SpriteRenderer sr; //Take this SpriteRenderer to change sprites depends on movement rotation

		public Enemy enemyScript; //Take enemy script

		private void Awake()
		{
			anim = GetComponent<Animator>();
			sr = GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			//Function GetAngle is being updated all the time as it changes rotation of Enemy (changes sprites).
			GetAngle();
		}

		//Take enemy's (or object's) rotation to player position. If enemy is rotated left to player position, it checks that. 
		void GetAngle()
		{

			Vector3 playerDir = Camera.main.transform.forward;
			playerDir.y = 0;
			Vector3 enemyDir = transform.Find("Vision").forward; //Vision is an object within enemy character
			enemyDir.y = 0;

			float dotProduct = Vector3.Dot(playerDir, enemyDir);


			if (dotProduct < -0.5f && dotProduct >= -1.0f) //If Enemy is rotated forward - change to sprite 0  (in animator)
				ChangeSprite(0);
			else if (dotProduct > 0.5f && dotProduct <= 1.0f) //If Enemy is rotated backward - change to sprite 1 (in animator)
				ChangeSprite(1);
			else
			{
				Vector3 playerRight = Camera.main.transform.right;
				playerRight.y = 0;
				dotProduct = Vector3.Dot(playerRight, enemyDir);
				if (dotProduct >= 0)
					ChangeSprite(2); //If Enemy is rotated right - change to sprite 2 (in animator)
				else
					ChangeSprite(3); //If Enemy is rotated left - change to sprite 2 (in animator)
			}


		}

		//Function for changing sprites. It takes name from Animator and "Anim States" within this script in Inspector. 
		//IN animator we have an animation of "Forward" and "anim states" within this script are going to display this animation.
		//Of course if we have put as animation for Element 0. It should be shown as "Forward" and written exactly as Animation name.
		void ChangeSprite(int index)
		{
			if (isAnimated)
				anim.Play(animStates[index]);
			else
				sr.sprite = sprites[index];
		}
	}
}