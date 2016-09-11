using UnityEngine;
using System.Collections;

[System.Serializable]
public class QueenBaseBuildingData : RotationBaseBuildingData {

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    public float maxHealth = 1f;

    public override Transform Build()
    {
        Transform result = base.Build();

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
