using UnityEngine;
using System.Collections;

public class LootDeath : MonoBehaviour, IDeathAction {

    [SerializeField]
    protected Transform lootPrefab;

    [SerializeField]
    protected float speed = 5;

    [SerializeField]
    public int value = 1;

    public void Die()
    {
        Transform lootTransform = Instantiate(lootPrefab, this.transform.position, Quaternion.AngleAxis(360 * Random.value, Vector3.up)) as Transform;
        Loot loot = lootTransform.GetComponentInChildren<Loot>();
        loot.value = value;
        Rigidbody lootRigidbody = lootTransform.GetComponent<Rigidbody>();
        Vector3 lootVelocity = speed * Random.insideUnitSphere;
        lootVelocity.y = Mathf.Abs(lootVelocity.y);
        lootRigidbody.velocity = lootVelocity;
    }
}
