using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    protected Transform enemyPrefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected Transform[] weapons;

    [SerializeField]
    protected float maxHealth = 1f;

    public Transform Build(Vector3 position, Quaternion facing, Transform player, Transform queen)
    {
        Transform result = Instantiate(enemyPrefab, position, facing) as Transform;
        Health health = result.GetComponent<Health>();
        (Instantiate(healthBarPrefab, health.healthDisplayHolder) as Transform).Reset();
        foreach (Transform deathAction in deathActions)
        {
            (Instantiate(deathAction, health.deathActionsHolder) as Transform).Reset();
        }
        health.Construct();
        health.SetMaxHealth(maxHealth);
        //result.GetComponent<EnemyNavigation>().Construct(player, queen);

        Transform weaponsHolder = result.Find("WeaponsHolder");

        foreach (Transform weapon in weapons)
        {
            (Instantiate(weapon, weaponsHolder) as Transform).Reset();
        }

        return result;
    }
}
