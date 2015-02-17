using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class EventCouple {

    public List<EventCondition> conditions;

    public List<EventAction> actions;

    public EventCouple() {
        conditions = new List<EventCondition>();
        conditions.Add(new EventCondition());
        actions = new List<EventAction>();
        actions.Add(new EventAction());
    }
}