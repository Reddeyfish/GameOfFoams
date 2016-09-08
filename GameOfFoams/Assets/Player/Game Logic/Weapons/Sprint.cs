using UnityEngine;
using System.Collections;

public class Sprint : MonoBehaviour {

    [SerializeField]
    protected float speedMultiplier = 2;

    InputToMovement movement;
    IInput input;

    void Start()
    {
        input = GetComponentInParent<IInput>();
        movement = GetComponentInParent<InputToMovement>();
    }

    void Update()
    {
        if (input.sprinting)
        {
            movement.speedMultiplier = speedMultiplier;
        }
        else
        {
            movement.speedMultiplier = 1f;
        }
    }
}
