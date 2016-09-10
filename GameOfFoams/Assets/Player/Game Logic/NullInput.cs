using UnityEngine;
using System.Collections;

public interface IWeaponsInput //for AI control of player modules?
{
    bool basicAttack { get; }
    bool heavyAttack { get; }
}

public interface IInput : IWeaponsInput
{
    void Construct(Bindings bindings);
    Vector3 movementInput { get; }
    float horizontalCameraInput { get; }
    float verticalCameraInput { get; }
    bool sprinting { get; }
    bool interacting { get; }
    bool beanBag { get; }
    bool taunting { get; }
    bool potion { get; }
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

    public string sprintingAxis = Tags.Axis.sprint;
    public string interactAxis = Tags.Axis.interact;
    public string beanBagAxis = Tags.Axis.beanBag;
    public string tauntAxis = Tags.Axis.taunt;
    public string potionAxis = Tags.Axis.potion;
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

    public bool heavyAttack
    {
        get
        {
            return false;
        }
    }

    public bool sprinting
    {
        get
        {
            return false;
        }
    }

    public bool interacting
    {
        get
        {
            return false;
        }
    }

    public bool beanBag
    {
        get
        {
            return false;
        }
    }

    public bool taunting
    {
        get
        {
            return false;
        }
    }

    public bool potion
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

    public abstract bool heavyAttack
    {
        get;
    }

    public bool sprinting
    {
        get
        {
            return Input.GetAxis(bindings.sprintingAxis) > 0.5f;
        }
    }

    public bool interacting
    {
        get
        {
            return Input.GetAxis(bindings.interactAxis) > 0.5f;
        }
    }

    public bool beanBag
    {
        get
        {
            return Input.GetAxis(bindings.beanBagAxis) > 0.5f;
        }
    }

    public bool taunting
    {
        get
        {
            return Input.GetAxis(bindings.tauntAxis) > 0.5f;
        }
    }

    public bool potion
    {
        get
        {
            return Input.GetAxis(bindings.potionAxis) > 0.5f;
        }
    }
}