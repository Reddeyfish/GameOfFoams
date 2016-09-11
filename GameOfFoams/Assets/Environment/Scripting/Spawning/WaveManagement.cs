using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public List<IEnemyData> enemies = new List<IEnemyData>();

    [SerializeField]
    public Transform[] prefabEnemies;
}

public interface IEnemyData
{
    Transform Build();
}

public abstract class BasicEnemyData : MonoBehaviour, IEnemyData
{
    [SerializeField]
    public Transform prefab;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected Transform[] weapons;

    [SerializeField]
    protected float maxHealth = 1f;

    public virtual Transform Build()
    {
        Transform result = Instantiate(prefab);

        Vector2 position = 10 * Random.insideUnitCircle;
        Vector3 worldPosition = new Vector3(position.x, 0, position.y);
        result.position = worldPosition;

        Health health = result.GetComponent<Health>();
        (Instantiate(healthBarPrefab, health.healthDisplayHolder) as Transform).Reset();
        foreach (Transform deathAction in deathActions)
        {
            (Instantiate(deathAction, health.deathActionsHolder) as Transform).Reset();
        }
        health.Construct();
        health.SetMaxHealth(maxHealth);

        Transform weaponsHolder = result.Find("WeaponsHolder");

        foreach (Transform weapon in weapons)
        {
            (Instantiate(weapon, weaponsHolder) as Transform).Reset();
        }

        return result;
    }
}

public class WaveManagement : MonoBehaviour
{

    [SerializeField]
    protected WaveData waveData;
    public static WaveData WaveData { get { return GameManagement.Main.WaveManagement.waveData; } }

    List<Transform> spawnedEnemies = null;

    public static bool waveSpawned { get { return Main.spawnedEnemies != null; } }

    public static WaveManagement Main { get { return GameManagement.Main.WaveManagement; } }

    /// <summary>
    /// Todo: spawn points?
    /// </summary>
    public void BuildWave()
    {
        Assert.IsFalse(waveSpawned);
        spawnedEnemies = new List<Transform>(WaveData.enemies.Count + WaveData.prefabEnemies.Length);
        foreach (IBaseBuildingData building in WaveData.enemies)
        {
            spawnedEnemies.Add(building.Build());
        }

        foreach (Transform building in WaveData.prefabEnemies)
        {
            IEnemyData data = building.GetComponent<IEnemyData>();
            Assert.IsNotNull(data);
            spawnedEnemies.Add(data.Build());
        }
    }

    public void DestroyWave()
    {
        if (!waveSpawned)
        {
            return;
        }

        foreach (Transform enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        spawnedEnemies = null;
    }

    public void RebuildBase()
    {
        DestroyWave();
        BuildWave();
    }
}
