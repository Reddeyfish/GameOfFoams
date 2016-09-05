using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class InputToMovement : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 movementInput;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float acceleration;
    [SerializeField]
    protected Transform cameraRotator;

    // Use this for initialization
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Vector3 movementInput = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical")); //pull from input script
        Vector3 rigidbodyXZ = rigid.velocity;
        rigidbodyXZ.y = 0;
        Vector3 movementXZ = Vector3.MoveTowards(rigidbodyXZ, movementInput * speed, Time.deltaTime * acceleration);
        Vector3 newVel = new Vector3(movementXZ.x, rigid.velocity.y, movementXZ.z);
        
        Vector3 start = transform.position + Vector3.up * 0.5f;

        rigid.velocity = newVel;

        // horizontal mouse look rotates transform around y axis
        Vector3 angles = rigid.rotation.eulerAngles;
        angles.y += Input.GetAxis("HorizontalMouse");
        Quaternion rotation = Quaternion.Euler(angles);
        rigid.rotation = rotation;

        // vertical mouse look rotates camera around x axis
        angles = cameraRotator.rotation.eulerAngles;
        if (angles.x > 180.0f) angles.x -= 360.0f;
        angles.x -= Input.GetAxis("VerticalMouse");
        angles.x = Mathf.Clamp(angles.x, -89.0f, 89.0f);
        rotation = Quaternion.Euler(angles);
        cameraRotator.rotation = rotation;
    }
}