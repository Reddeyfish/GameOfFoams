using UnityEngine;
using System.Collections;

public class DespawnDeath : MonoBehaviour, IDeathAction {
    public void Die()
    {
        Destroy(this.transform.root.gameObject);
    }
}
