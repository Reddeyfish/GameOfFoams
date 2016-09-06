using UnityEngine;
using System.Collections;

public interface IInput
{
    void Construct(Bindings bindings);
    Vector3 movementInput { get; }
    float horizontalCameraInput { get; }
    float verticalCameraInput { get; }
    bool basicAttack { get; }
}

[System.Serializable]
public class Bindings
{
    public enum InputType { MOUSE, CONTROLLER };

    public InputType inputType = InputType.MOUSE;

    public string horizontalMovementAxis = Tags.Axis.horizontal;
    public string verticalMovementAxis = Tags.Axis.vertical;

    public string horizontalCameraAxis = Tags.Axis.horizontalMouse;
    public string verticalCameraAxis = Tags.Axis.verticalMouse;
}

/// <summary>
/// Represents an input that is completely disabled
/// </summary>
public class NullInput : MonoBehaviour, IInput {
	public void Construct(Bindings bindings)
    {
    }

    public Vector3 movementInput
    {
        get
        {
            return Vector3.zero;
        }
    }

    public float horizontalCameraInput {
        get
        {
            return 0;
        }
    }

    public float verticalCameraInput
    {
        get
        {
            return 0;
        }
    }

    public bool basicAttack
    {
        get
        {
            return false;
        }
    }
}

public abstract class AbstractAxisInput : MonoBehaviour, IInput
{

    protected Bindings bindings;

	public virtual void Construct(Bindings bindings)
    {
        this.bindings = bindings;
    }

    public Vector3 movementInput
    {
        get
        {
            return (transform.right * Input.GetAxis(bindings.horizontalMovementAxis)) + (transform.forward * Input.GetAxis(bindings.verticalMovementAxis));
        }
    }

    public float horizontalCameraInput
    {
        get
        {
            return Input.GetAxis(bindings.horizontalCameraAxis);
        }
    }

    public float verticalCameraInput
    {
        get
        {
            return Input.GetAxis(bindings.verticalCameraAxis);
        }
    }

    public abstract bool basicAttack
    {
        get;
    }
}