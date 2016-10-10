using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BeanBag : MonoBehaviour, Throwable {

    [SerializeField]
    protected Transform impactVFX;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float damage;

    Rigidbody rigid;
    ITeamReference teamReference;

	void Awake () {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = speed * transform.forward;
        Destroy(this.gameObject, 20);
	}

    public void Instantiate(ITeamReference teamReference) {
        this.teamReference = teamReference;
    }

    void OnTriggerEnter(Collider col) {
        EnemyNavigation enemy = col.transform.GetComponentInParent<EnemyNavigation>();
        Rigidbody enemyRigidbody = col.transform.GetComponentInParent<Rigidbody>();
        Health enemyHealth = col.transform.GetComponentInParent<Health>();

        if (enemy == null || enemyRigidbody == null || enemyHealth == null) {
            return; //hit a wall, or something
        }

        if (!enemyHealth.Damage(damage, teamReference)) {
            return; //target is invulnerable
        }

        Instantiate(impactVFX, col.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
    }
}
