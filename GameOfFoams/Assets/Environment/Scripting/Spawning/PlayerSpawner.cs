using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerPrefab;

    [SerializeField]
    protected Transform basicAttackPrefab;

    [SerializeField]
    protected Bindings bindings;

    public Transform Construct(Vector3 playerPosition, Quaternion playerFacing)
    {
        Transform result = Instantiate(playerPrefab, playerPosition, playerFacing) as Transform;
        IInput input;
        switch (bindings.inputType)
        {
            case Bindings.InputType.MOUSE:
                input = result.AddComponent<MouseInput>();
                break;
            case Bindings.InputType.CONTROLLER:
                input = result.AddComponent<ControllerInput>();
                break;
            default:
                input = result.AddComponent<NullInput>();
                break;

        }
        input.Construct(bindings);
        (Instantiate(basicAttackPrefab, result.Find("WeaponsHolder")) as Transform).Reset();
        return result;
    }
}
