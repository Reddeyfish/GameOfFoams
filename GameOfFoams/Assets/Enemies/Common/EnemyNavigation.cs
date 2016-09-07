using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour {

    NavMeshAgent navAgent;

    Transform player;
    Transform queen;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
	}

    public EnemyNavigation Construct(Transform player, Transform queen)
    {
        this.player = player;
        this.queen = queen;
        return this;
    }

    /// <summary>
    /// To be called by whatever basic AI system we set up
    /// </summary>
    public void SeekPlayer()
    {
        navAgent.SetDestination(player.position);
    }

    public void SeekQueen()
    {
        navAgent.SetDestination(queen.position);
    }

    public void ClearSeeking()
    {
        navAgent.ResetPath();
    }
}
