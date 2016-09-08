using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class InputToMovement : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 movementInput;
    IInput input;

    [SerializeField]
    protected float baseSpeed;
    [System.NonSerialized]
    public float speedMultiplier = 1;
    public float speed { get { return baseSpeed * speedMultiplier; } }

    [SerializeField]
    protected float acceleration;
    [SerializeField]
    protected Transform cameraRotator;

    // Use this for initialization
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        input = GetComponent<IInput>();
    }

    void Update()
    {

        Vector3 movementInput = input.movementInput;
        Vector3 rigidbodyXZ = rigid.velocity;
        rigidbodyXZ.y = 0;
        Vector3 movementXZ = Vector3.MoveTowards(rigidbodyXZ, movementInput * speed, Time.deltaTime * acceleration);
        Vector3 newVel = new Vector3(movementXZ.x, rigid.velocity.y, movementXZ.z);

        rigid.velocity = newVel;

        // horizontal mouse look rotates transform around y axis
        Vector3 angles = rigid.rotation.eulerAngles;
        angles.y += input.horizontalCameraInput;
        Quaternion rotation = Quaternion.Euler(angles);
        rigid.rotation = rotation;

        // vertical mouse look rotates camera around x axis
        angles = cameraRotator.rotation.eulerAngles;
        if (angles.x > 180.0f) angles.x -= 360.0f;
        angles.x -= input.verticalCameraInput;
        angles.x = Mathf.Clamp(angles.x, -89.0f, 89.0f);
        rotation = Quaternion.Euler(angles);
        cameraRotator.rotation = rotation;
    }
}