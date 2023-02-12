using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{

    public class PatrolState : IEnemyAI
    {
        //ENEMY AI SCRIPT - States

        EnemyStates enemy; //Take EnemyStates Script
        int nextWayPoint = 0; //Take next waypoint Enemy should go to

        public PatrolState(EnemyStates enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateActions()
        {
            Watch();
            Patrol();
        }

        // Function makes the Enemy to see the Player
        // Once Enemy sees the player - he starts to chase him
        void Watch()
        {
            if (enemy.EnemySpotted()) //if enemy spotted Player then change state to chasestate
            {
                Debug.Log("Player (Mine Enemy) Detected!");
                ToChaseState();
            }
        }

        // Patrolling Function. Enemy will go through waypoints to Patrol the area
        void Patrol()
        {
            enemy.navMeshAgent.destination = enemy.waypoints[nextWayPoint].position;
            enemy.navMeshAgent.isStopped = false;
            if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance
                && !enemy.navMeshAgent.pathPending)
            {
                nextWayPoint = (nextWayPoint + 1) % enemy.waypoints.Length; //calculation how many waypoints we have and what next is to follow
            }
        }

        //If Enemy collides with:
        public void OnTriggerEnter(Collider enemy)
        {
            if (enemy.gameObject.CompareTag("Player")) //Player
            {
                ToAlertState(); //He is being alerted
            }
        }

        public void ToPatrolState()
        {
            Debug.Log("I'm Patrolling Now");
        }

        public void ToAttackState()
        {
            enemy.currentState = enemy.attackState;
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