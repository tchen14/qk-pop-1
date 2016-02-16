using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState
{
    private readonly StatePatternEnemy enemy;
    private int sightAngle = 50;
    private float chaseTimer;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {

    }

    public void ToChaseState()
    {
        Debug.Log("Cant transition into itself");
    }

    public void ToGuardState()
    {

    }

    public void ToDazedState()
    {

    }

    public void ToDistractedState()
    {

    }

    public void ToSuspiciousState()
    {
        enemy.currentState = enemy.suspiciousState;
        chaseTimer = 0f;
    }

    public void ToKOState()
    {

    }

    public void ToWalkState()
    { 

    }

    private void Look()
    {
        RaycastHit hit;
        //Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;
        Vector3 enemyToTarget = enemy.chaseTarget.position;
        if (Vector3.Angle(enemy.chaseTarget.position - enemy.transform.position, enemy.transform.forward) < sightAngle)
        {
            if (Physics.Raycast(enemy.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
                enemy.chaseTarget = hit.transform;
        }
        else
        {
            Debug.Log(enemy.navMeshAgent.pathStatus);
            ToSuspiciousState();
        }
    }

    private void Chase()
    {
        //ai slows down the longer they chase until they reach 0 
        //speed -= chasetimer
        chaseTimer += Time.deltaTime;
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.Resume ();
    }
}
