using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
	public class DoorScript : MonoBehaviour
	{

		//Door Functionality

		[Header("Door Settings")]
		public bool slide, rotate; //Door slide or Rotate (to be chosen)
		public float speed; //How fast door are opening

		public KeyCode openningKey; //Specify on what keyboard input we open the door

		public Vector3 endPosition; //Where Door ends opening (how far do door move)
		Vector3 startPosition; //Position of starting door (closed or opened) this is first position of door on level start

		GameObject doors; //Prefab of Doors

		public bool isOpen = false; //Are doors open? True/False
		bool finishedAnimation = false; //Have doors finished opening? (Not to repeat anim in the middle of action)

		Animator anim; //Animator for doors

		//On Level Start
		private void Awake()
		{
			doors = this.gameObject; //Find Doors Object in the level
			startPosition = doors.transform.position; //Get position of doors
			anim = doors.GetComponent<Animator>(); //Specify Animations for doors 
		}

		#region FixedUpdate (Fixing Doors not to re-animate in the middle of animation)
		private void FixedUpdate()
		{
			if (doors.transform.position == startPosition || doors.transform.position == endPosition)
			{
				finishedAnimation = true;
			}
			else
			{
				finishedAnimation = false;
			}
		}

		#endregion

		//If Player stay close to doors or at the doors
		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				if (Input.GetKeyDown(openningKey)) //If Player clicks opening key on keyboard
				{
					if (slide) //if we have selected door to slide 
					{
						StartCoroutine(SlideDoors()); //Start opening or closing doors (sliding)
					}
					else if (rotate) //If we have selected rotation for doors
					{
						if (!isOpen) //If doors are not opened
						{
							isOpen = !isOpen; //change them to open/closed on each click
							anim.SetBool("isOpened", true); //make animation for doors rotation
						}
						else
						{
							isOpen = !isOpen; //if we click again 
							anim.SetBool("isOpened", false); //then make doors closed on button click
						}
					}
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				if (openningKey == KeyCode.None) //If Player clicks opening key on keyboard
				{
					if (slide && finishedAnimation == true) //if we have selected door to slide 
					{
						StartCoroutine(SlideDoors()); //Start opening or closing doors (sliding)
					}
					else if (rotate && finishedAnimation == true) //If we have selected rotation for doors
					{
						if (!isOpen) //If doors are not opened
						{
							isOpen = !isOpen; //change them to open/closed on each click
							anim.SetBool("isOpened", true); //make animation for doors rotation
						}
						else
						{
							isOpen = !isOpen; //if we click again 
							anim.SetBool("isOpened", false); //then make doors closed on button click
						}
					}
				}
			}
		}

		//Sliding Doors function
		IEnumerator SlideDoors()
		{
			Vector3 current = doors.transform.position; //Take current door position
			Vector3 destination = isOpen ? startPosition : endPosition; //calculate distance between start point and end point
			isOpen = !isOpen; //make doors opened/closed depends what they are now
			float t = 0f;

			while (t < 1) //do the calculation to slide the door to open/close them
			{
				t += Time.deltaTime * speed;
				doors.transform.position = Vector3.Lerp(current, destination, t);
				yield return null;
			}

		}

	}
}
