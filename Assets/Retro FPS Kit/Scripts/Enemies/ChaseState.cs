using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{

    public class ChaseState : IEnemyAI
    {
        //Chase State for Enemy

        EnemyStates enemy; //take EneymStates Script

        public ChaseState(EnemyStates enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateActions()
        {
            Watch();
            Chase();
        }

        // Function for Seeing the Player
        // Once Enemy sees the Player - let's set Player as enemy's target
        // If Enemy loses Player, let's become alarmed
        void Watch()
        {
            if (!enemy.EnemySpotted())
            {
                ToAlertState();
            }
        }

        // Function resposible for chasing the player
        // Once Enemy is close enough to Player - He starts attacking him
        void Chase()
        {
            enemy.navMeshAgent.destination = enemy.chaseTarget.position;
            enemy.navMeshAgent.isStopped = false;
            if (enemy.navMeshAgent.remainingDistance <= enemy.attackRange && enemy.onlyMelee == true)
            {
                enemy.navMeshAgent.isStopped = true; //If enemy stopped being closed enough
                ToAttackState(); //Start attacking the playe
            }
            else if (enemy.navMeshAgent.remainingDistance <= enemy.shootRange && enemy.onlyMelee == false)
            {
                enemy.navMeshAgent.isStopped = true;
                ToAttackState();
            }
        }

        public void OnTriggerEnter(Collider enemy)
        {

        }

        public void ToPatrolState()
        {
            Debug.Log("I can't do it now!");
        }

        public void ToAttackState()
        {
            Debug.Log("I am now attacking the Player");
            enemy.currentState = enemy.attackState;
        }

        public void ToAlertState()
        {
            Debug.Log("Can't see the Player anymore");
            enemy.currentState = enemy.alertState;
        }

        public void ToChaseState()
        {
            Debug.Log("I'M CHASING THE PLAYER!");
        }

    }
}