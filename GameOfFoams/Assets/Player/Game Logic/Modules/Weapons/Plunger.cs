using UnityEngine;
using System.Collections.Generic;

public class Plunger : MonoBehaviour
{
    [SerializeField]
    protected GameObject impactVFX;

    [SerializeField]
    protected float damage = 1;

    [SerializeField]
    protected float stunDuration = 1;

    [SerializeField]
    protected float upwardForce = 10;

    [SerializeField]
    protected float pullForce = 40;

    [SerializeField]
    protected float cooldownSecs = 2;

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
    void Update()
    {
        if (Time.time > readyTime && input.heavyAttack)
        {
            Fire();
        }
    }

    void Fire()
    {
        readyTime = Time.time + cooldownSecs;
        RaycastHit hitInfo;
        if(!Physics.Raycast(transform.position, transform.forward, out hitInfo, 100, layermask))
        {
            Debug.Log("Plunger Hit Nothing");
            return; //nothing hit
        }
        EnemyNavigation enemy = hitInfo.transform.GetComponentInParent<EnemyNavigation>();
        Rigidbody enemyRigidbody = hitInfo.transform.GetComponentInParent<Rigidbody>();
        Health enemyHealth = hitInfo.transform.GetComponentInParent<Health>();

        Debug.Log(hitInfo.transform);

        if(enemy == null || enemyRigidbody == null || enemyHealth == null)
        {
            return; //hit a wall, or something
        }

        if (!enemyHealth.Damage(damage, teamReference))
        {
            return; //target is invulnerable
        }

        Instantiate(impactVFX, hitInfo.point, Quaternion.identity);
        enemy.DisableNavigation(stunDuration);
        enemyRigidbody.velocity -= 20 * transform.forward;
        /*
        enemyRigidbody.AddForce(0, 0, upwardForce, ForceMode.VelocityChange);
        enemyRigidbody.AddForce(pullForce * (transform.position - enemyRigidbody.position).normalized, ForceMode.VelocityChange);
         * */
    }
}
