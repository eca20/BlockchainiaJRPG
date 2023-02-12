using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{
    public class AlertState : IEnemyAI
    {

        //Alert State for Enemy

        EnemyStates enemy; //EnemyStates script
        float timer = 0; //Timer for this State

        public AlertState(EnemyStates enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateActions()
        {
            Search();
            Watch();

            // Look around only once Enemy finds last known Player's location (where Player shot or has been seen).
            if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
                LookAround();
        }

        // Seeing Player Function
        // Once Enemy sees player - he sets Player as new target
        // Now Enemy starts chasing the Player
        void Watch()
        {
            if (enemy.EnemySpotted())
            {
                enemy.navMeshAgent.destination = enemy.lastKnownPosition;
                ToChaseState();
            }
        }

        // Functionality of Looking Around for the Player
        // Once Enemy gets to the last known Player's position, he waits the some time and then comes back to patrolling
        void LookAround()
        {
            timer += Time.deltaTime;
            if (timer >= enemy.stayAlertTime)
            {
                timer = 0;
                ToPatrolState();
            }
        }

        // Function that sets last known Player position as the searching area target
        void Search()
        {
            enemy.navMeshAgent.destination = enemy.lastKnownPosition;
            enemy.navMeshAgent.isStopped = false;
        }

        public void OnTriggerEnter(Collider enemy)
        {

        }

        public void ToPatrolState()
        {
            enemy.currentState = enemy.patrolState;
        }

        public void ToAttackState()
        {
            Debug.Log("Can't do it now!");
        }

        public void ToAlertState()
        {
            Debug.Log("I'm already in this state");
        }

        public void ToChaseState()
        {
            enemy.currentState = enemy.chaseState;
        }
    }

}