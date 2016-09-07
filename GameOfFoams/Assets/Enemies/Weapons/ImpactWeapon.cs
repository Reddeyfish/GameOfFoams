using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class ImpactWeapon : MonoBehaviour {

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float cooldown;

    ITeamReference teamReference;

    Queue<KeyValuePair<float, Health>> immuneTargets = new Queue<KeyValuePair<float, Health>>();

    void Start()
    {
        teamReference = GetComponentInParent<ITeamReference>();
        Assert.IsNotNull(teamReference);
    }

    void OnTriggerEnter(Collider coll)
    {
        Health target = coll.transform.GetComponentInParent<Health>();
        if (target != null && !isTargetImmune(target))
        {
            if (target.Damage(damage, teamReference))
            {
                KeyValuePair<float, Health> immunity = new KeyValuePair<float, Health>(Time.time + cooldown, target);
                immuneTargets.Enqueue(immunity);
            }
        }
    }

    void purgeImmuneTargets()
    {
        while (immuneTargets.Count > 0 && immuneTargets.Peek().Key < Time.time)
        {
            immuneTargets.Dequeue();
        }
    }

    bool isTargetImmune(Health target) //use secondary set data structure?
    {
        purgeImmuneTargets();
        foreach (KeyValuePair<float, Health> pair in immuneTargets)
        {
            if (target == pair.Value)
            {
                return true;
            }
        }
        return false;
    }
}
