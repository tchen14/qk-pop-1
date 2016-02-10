﻿using UnityEngine;
using System.Collections;

public class DazedState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    public DazedState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {

    }

    public void ToChaseState()
    {

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

    }

    public void ToKOState()
    {

    }

    public void ToWalkState()
    {

    }
}
