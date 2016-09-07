using UnityEngine;
using System.Collections;

public class QueenSpawner : MonoBehaviour
{

    [SerializeField]
    protected Transform queenPrefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected float maxHealth = 1f;

    public Transform Build(Vector3 position, Quaternion facing)
    {
        Transform result = Instantiate(queenPrefab, position, facing) as Transform;
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
