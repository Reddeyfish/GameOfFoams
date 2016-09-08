using UnityEngine;
using System.Collections;

public class BarrierSpawner : MonoBehaviour
{

    [SerializeField]
    protected Transform barrierPrefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected float maxHealth = 1f;

    public Transform Build(Vector3 position, Quaternion facing)
    {
        Transform result = Instantiate(barrierPrefab, position, facing) as Transform;
        Health health = result.GetComponent<Health>();
        (Instantiate(healthBarPrefab, health.healthDisplayHolder) as Transform).Reset();
        foreach (Transform deathAction in deathActions)
        {
            (Instantiate(deathAction, health.deathActionsHolder) as Transform).Reset();
        }
        health.Construct();
        health.SetMaxHealth(maxHealth);

        return result;
    }
}
