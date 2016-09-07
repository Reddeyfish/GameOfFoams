using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour {

    NavMeshAgent navAgent;

    Transform player;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
	}

    public EnemyNavigation Construct(Transform player)
    {
        this.player = player;
        return this;
    }

    /// <summary>
    /// To be called by whatever basic AI system we set up
    /// </summary>
    public void SeekPlayer()
    {
        navAgent.SetDestination(player.position);
    }

    public void ClearSeeking()
    {
        navAgent.ResetPath();
    }
}
