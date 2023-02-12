using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{

	// This script is responsible for Enemy to always look at the target destination (waypoints, player target, any target we set for destination)
	public class Vision : MonoBehaviour
	{
		public Enemy enemyScript;
		Vector3 destination;

		void Update()
		{
			//Destination 
			destination = transform.parent.GetComponent<EnemyStates>().navMeshAgent.destination;

			//Do it only if health is above 0 (after death, it will only look at Player - always)
			if (enemyScript.health > 0)
			{
				transform.LookAt(destination);
			}

		}
	}
}