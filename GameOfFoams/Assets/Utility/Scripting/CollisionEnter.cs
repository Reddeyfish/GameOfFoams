using UnityEngine;
using System.Collections;

public class CollisionEnter : MonoBehaviour {

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log(this.transform);
    }
}
