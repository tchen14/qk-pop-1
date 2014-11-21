using UnityEngine;
using System.Collections;

public class AI_health : MonoBehaviour {

    public int hurt (int damage) {
        int HP = GetComponent<AI_main> ().AI_HP;
        HP -= damage;
        return HP;
    }
    public int heal (int health) {
        int HP = GetComponent<AI_main> ().AI_HP;
        HP += health;
        return HP;
    }
}
