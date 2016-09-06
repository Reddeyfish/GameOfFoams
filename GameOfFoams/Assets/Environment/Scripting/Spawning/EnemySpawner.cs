using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    protected Transform enemyPrefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected float maxHealth = 1f;

    public Transform Build(Vector3 position, Quaternion facing, Transform player)
    {
        Transform result = Instantiate(enemyPrefab, position, facing) as Transform;
        Health health = result.GetComponent<Health>();
        Transform healthBar = Instantiate(healthBarPrefab, health.healthDisplayHolder) as Transform;
        healthBar.Reset();
        health.Construct();
        health.SetMaxHealth(maxHealth);
        result.GetComponent<EnemyNavigation>().Construct(player);
        return result;
    }
}
