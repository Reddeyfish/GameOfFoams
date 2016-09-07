using UnityEngine;
using System.Collections;

public class ZRotation : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion delta = Quaternion.Euler(0, 0, Time.deltaTime * speed);
        Quaternion result = transform.rotation * delta;
        transform.rotation = result;
	}
}
