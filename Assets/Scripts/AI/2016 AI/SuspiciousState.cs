using UnityEngine;
using System.Collections;

public class SuspiciousState : IEnemyState
{
    private readonly StatePatternEnemy enemy;
    private float searchTimer;

    public SuspiciousState (StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        searchTimer = 0f;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        searchTimer = 0f;
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
        Debug.Log("Cant transition into itself");
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
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
            {
            Debug.Log("works");
            enemy.chaseTarget = hit.transform;
            ToChaseState();
            }
    }

    private void Search()
    {
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.Stop();
        enemy.transform.Rotate(0, enemy.searchingTurnSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime;
        if (searchTimer >= enemy.searchingDuration)
        {
            //ToDefaultState();
            ToPatrolState();
        }

    }

}
