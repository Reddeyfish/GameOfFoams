using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class CatTail : MonoBehaviour, ITeamReference {

    [SerializeField]
    protected GameObject impactVFX;

    [SerializeField]
    protected float stunDuration;

    public Team Team { get { return Team.PLAYER; } }

    Collider coll;

    void Start() {
        coll = GetComponent<Collider>();
    }

	void OnTriggerEnter (Collider other) {
        EnemyNavigation enemy = other.transform.GetComponentInParent<EnemyNavigation>();
        Rigidbody enemyRigidbody = other.transform.GetComponentInParent<Rigidbody>();
        Health enemyHealth = other.transform.GetComponentInParent<Health>();

        if (enemy == null || enemyRigidbody == null || enemyHealth == null) {
            return; //hit a wall, or something
        }

        if (!enemyHealth.Damage(0, this)) {
            return; //target is invulnerable
        }

        Instantiate(impactVFX, other.transform.position, Quaternion.identity);
        enemy.DisableNavigation(stunDuration);
        enemyRigidbody.velocity += 3 * transform.up;

        Physics.IgnoreCollision(other, coll);
	}
}
