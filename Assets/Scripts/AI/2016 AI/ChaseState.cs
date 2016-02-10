using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState
{
    private readonly StatePatternEnemy enemy;
    private int sightAngle = 50;

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
        Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;
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
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.Resume ();
    }
}
