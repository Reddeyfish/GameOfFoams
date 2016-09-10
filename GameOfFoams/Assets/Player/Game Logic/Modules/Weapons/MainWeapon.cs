using UnityEngine;
using System.Collections.Generic;

public class MainWeapon : MonoBehaviour {

    [SerializeField]
    protected Transform impactVFX;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected Vector2 boxHalfDimensions;

    [SerializeField]
    protected float arcRadius;

    [SerializeField]
    protected float totalArcAngleDegrees;

    [SerializeField]
    protected float cooldownSecs;

    IInput input;
    ITeamReference teamReference;
    float readyTime = 0;
    int layermask;

    void Awake()
    {
        layermask = LayerMask.GetMask(Tags.Layers.EnemyColliders);
    }

    void Start()
    {
        input = GetComponentInParent<IInput>();
        teamReference = GetComponentInParent<ITeamReference>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > readyTime)
        {
            if (input.basicAttack)
            {
                Fire();
            }
        }
	}

    void Fire()
    {
        readyTime = Time.time + cooldownSecs;
        HashSet<Collider> hits = RaycastArc(transform.position, transform.forward, boxHalfDimensions, arcRadius, totalArcAngleDegrees, layermask);
        foreach(Collider hit in hits)
        {
            hit.GetComponentInParent<Health>().Damage(damage, teamReference);
            Instantiate(impactVFX, hit.transform.position, Quaternion.identity);
        }
    }

    static HashSet<Collider> RaycastArc(Vector3 origin, Vector3 direction, Vector2 boxHalfDimensions, float radius, float totalArcAngleDegrees, int layermask)
    {
        direction.y = 0;
        direction.Normalize();
        HashSet<Collider> result = new HashSet<Collider>();
        float boxHalfAngleCoverage = Mathf.Rad2Deg * Mathf.Atan2(boxHalfDimensions.x, radius);

        for (float angle = totalArcAngleDegrees / 2; angle >= boxHalfAngleCoverage; angle -= 2 * boxHalfAngleCoverage)
        {
            result.UnionWith(boxCast(origin, direction, boxHalfDimensions, radius, angle, layermask));
            result.UnionWith(boxCast(origin, direction, boxHalfDimensions, radius, -angle, layermask));
        }
        return result;
    }

    static Collider[] boxCast(Vector3 origin, Vector3 centerDirection, Vector2 boxHalfDimensions, float radius, float angleDegrees, int layermask)
    {
        Quaternion rotationOffset = Quaternion.AngleAxis(angleDegrees, Vector3.up);
        Vector3 direction = rotationOffset * centerDirection;
        RaycastHit[] results = Physics.BoxCastAll(origin, boxHalfDimensions, direction, Quaternion.LookRotation(direction), radius, layermask);
        Collider[] returnValue = new Collider[results.Length];
        for (int i = 0; i < results.Length; i++)
        {
            returnValue[i] = results[i].collider;
        }
        return returnValue;
    }
}
