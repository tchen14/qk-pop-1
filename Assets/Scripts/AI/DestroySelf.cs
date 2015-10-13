using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "AI")
        {
            Destroy(this);
        }
    }
}
