using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour, ITeamReference {

    [SerializeField]
    protected float radius;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float delay;

    [SerializeField]
    protected GameObject impactVFX;

    public Team Team { get { return Team.PLAYER; } }

    void Start() {
        GetComponent<Interactable>().InteractEventPublisher += OnInteract;
    }

    public void OnInteract() {
        Callback.FireAndForget(() => {
            foreach (Collider other in Physics.OverlapSphere(transform.position, radius)) {
                EnemyNavigation enemy = other.transform.GetComponentInParent<EnemyNavigation>();
                Rigidbody enemyRigidbody = other.transform.GetComponentInParent<Rigidbody>();
                Health enemyHealth = other.transform.GetComponentInParent<Health>();

                if (enemy == null || enemyRigidbody == null || enemyHealth == null) {
                    continue; //hit a wall, or something
                }

                if (!enemyHealth.Damage(damage, this)) {
                    continue; //target is invulnerable
                }

                Instantiate(impactVFX, other.transform.position, Quaternion.identity);
            }
        }, delay, this);
    }
}
