using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class MouseInput : AbstractAxisInput
{
	public override void Construct(Bindings bindings)
    {
        base.Construct(bindings);
        Assert.AreEqual(bindings.inputType, Bindings.InputType.MOUSE);
    }

    public override bool basicAttack
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    public override bool heavyAttack
    {
        get
        {
            return Input.GetMouseButton(1);
        }
    }
}

