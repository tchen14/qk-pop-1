using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EventCouple {

    public PopEvent popEvent;
    public List<EventCondition> conditions;
    public List<EventAction> actions;

    public enum AndOrCompare { EveryCondition, OneOrMore, ExactlyOne }
    public AndOrCompare andOrCompare = AndOrCompare.EveryCondition;

    public EventCouple(PopEvent newPopEvent) {
        popEvent = newPopEvent;
        conditions = new List<EventCondition>();
        conditions.Add(new EventCondition());
        actions = new List<EventAction>();
        actions.Add(new EventAction());
    }
}