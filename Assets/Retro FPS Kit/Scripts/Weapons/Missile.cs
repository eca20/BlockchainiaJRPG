using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
    public class Missile : MonoBehaviour
    {
        //Missile Script and missile functionality 

        [HideInInspector]
        public float damage; //Damage of missile
        [HideInInspector]
        public float speed; //Speed of the Missile
        Transform player; //Take Player Position and rotation

        [Header("How long missile lasts")]
        public int missileLife; //How long missile lasts
        float timer; //Timer for missiles

        void Start()
        {
            missileLife = 15; //Misile has 15  seconds before disappearing
            player = GameObject.FindGameObjectWithTag("Player").transform; //Find Player Object in the Level
            transform.LookAt(player); //Missile should get to the player position (if he doesn't move)
        }

        void Update()
        {
            timer += Time.deltaTime; //Start counting missile life
            if (timer > missileLife) // after 15 seconds, destroy missile
                Destroy(this.gameObject);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        //If missile hits trigger do action
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) //If missile hits Player
            {
                other.SendMessage("EnemyHit", damage, SendMessageOptions.DontRequireReceiver); //Send to the enemy that we got hit
            }
            Destroy(this.gameObject); //Destroy missile on hit any trigger (floor, walls, player etc)
        }
    }
}