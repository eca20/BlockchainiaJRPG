using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{
    public class AttackState : IEnemyAI
    {

        //Attack State for Enemy

        EnemyStates enemy; //Take the EnemyStates script
        float timer; //Timer for attacking

        public AttackState(EnemyStates enemy)
        {
            this.enemy = enemy;
        }

        //Chase Player if Enemy is too far to attack him
        public void UpdateActions()
        {
            timer += Time.deltaTime;
            float distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);
            if (distance > enemy.attackRange && enemy.onlyMelee == true) //if enemy is too far to attack malee - chase him
            {
                ToChaseState();
            }
            if (distance > enemy.shootRange && enemy.onlyMelee == false) //if enemy is too far to shoot Player - chase him
            {
                ToChaseState();
            }
            Watch(); //Watch Player on states
            if (distance <= enemy.shootRange && distance > enemy.attackRange && enemy.onlyMelee == false && timer >= enemy.attackDelay)
            {
                Attack(true);
                timer = 0;
            }
            if (distance <= enemy.attackRange && timer >= enemy.attackDelay)
            {
                Attack(false);
                timer = 0;
            }
        }

        //Attack Function
        void Attack(bool shoot)
        {
            //Get information if Enemy hit Player
            if (shoot == false)
            {
                enemy.chaseTarget.SendMessage("EnemyHit", enemy.meleeDamage, SendMessageOptions.DontRequireReceiver);
            }
            else if (shoot == true)
            {
                GameObject missile = GameObject.Instantiate(enemy.missile, enemy.transform.position, Quaternion.identity);
                missile.GetComponent<Missile>().speed = enemy.missileSpeed;
                missile.GetComponent<Missile>().damage = enemy.missileDamage;
            }
        }

        //Change states after seeing the Player
        void Watch()
        {
            if (!enemy.EnemySpotted())
            {
                ToAlertState();
            }
        }

        public void OnTriggerEnter(Collider enemy)
        {

        }

        public void ToPatrolState()
        {
            Debug.Log("Can't do it now!");
        }

        public void ToAttackState()
        {
            Debug.Log("Can't do it now!");
        }

        public void ToAlertState()
        {
            enemy.currentState = enemy.alertState;
        }

        public void ToChaseState()
        {
            enemy.currentState = enemy.chaseState;
        }
    }
}