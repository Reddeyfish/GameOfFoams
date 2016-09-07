using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerPrefab;

    [SerializeField]
    protected Transform basicAttackPrefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected float maxHealth = 1f;

    [SerializeField]
    protected Bindings bindings;

    public Transform Build(Vector3 playerPosition, Quaternion playerFacing)
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

        Health health = result.GetComponent<Health>();
        health.healthDisplayHolder = (Instantiate(healthBarPrefab) as Transform);
        health.healthDisplayHolder.Reset();
        foreach (Transform deathAction in deathActions)
        {
            (Instantiate(deathAction, health.deathActionsHolder) as Transform).Reset();
        }
        health.Construct();
        health.SetMaxHealth(maxHealth);

        (Instantiate(basicAttackPrefab, result.Find("WeaponsHolder")) as Transform).Reset();
        return result;
    }
}
