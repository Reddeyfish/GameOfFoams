using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseData
{
    public List<IBaseBuildingData> buildings = new List<IBaseBuildingData>();

    [SerializeField]
    public Transform[] prefabBuildings;
}

public interface IBaseBuildingData
{
    /// <summary>
    /// Creates, initializes, and returns the Building.
    /// </summary>
    /// <returns>The created and initialized building.</returns>
    Transform Build();
}

public abstract class BasicBaseBuildingData : MonoBehaviour, IBaseBuildingData
{
    [SerializeField]
    public Transform prefab;

    [SerializeField]
    public Vector3 position = Vector3.zero;

    public virtual Transform Build()
    {
        Transform result = Instantiate(prefab);
        result.position = position;
        return result;
    }
}

public abstract class RotationBaseBuildingData : BasicBaseBuildingData
{
    [SerializeField]
    public Vector3 forward = Vector3.forward;

    public override Transform Build()
    {
        Transform result = base.Build();
        result.rotation = Quaternion.LookRotation(forward);
        return result;
    }
}

[System.Serializable]
public abstract class HealthBaseBuildingData : RotationBaseBuildingData
{

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

public class BaseManagement : MonoBehaviour {

    [SerializeField]
    protected BaseData baseData;
    public static BaseData BaseData { get { return GameManagement.Main.BaseManagement.baseData; } }

    List<Transform> buildings = null;

    public static bool baseBuilt { get { return Main.buildings != null; } }

    public static BaseManagement Main { get { return GameManagement.Main.BaseManagement; } }

    public void BuildBase()
    {
        Assert.IsFalse(baseBuilt);
        buildings = new List<Transform>(BaseData.buildings.Count + BaseData.prefabBuildings.Length);
        foreach (IBaseBuildingData building in BaseData.buildings)
        {
            buildings.Add(building.Build());
        }

        foreach (Transform building in BaseData.prefabBuildings)
        {
            IBaseBuildingData data = building.GetComponent<IBaseBuildingData>();
            Assert.IsNotNull(data);
            buildings.Add(data.Build());
        }
    }

    public void DestroyBase()
    {
        if (!baseBuilt)
        {
            return;
        }

        foreach (Transform building in buildings)
        {
            if (building != null)
            {
                Destroy(building.gameObject);
            }
        }
        buildings = null;
    }

    public void RebuildBase()
    {
        DestroyBase();
        BuildBase();
    }

    public void AddBuiltBuilding(Transform building)
    {
        Assert.IsTrue(baseBuilt);
        buildings.Add(building);
    }
}
