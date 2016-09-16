using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyNavigation : MonoBehaviour {

    NavMeshAgent navAgent;
    Rigidbody rigid;

    Coroutine restoreNavigationRoutine = null;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
	}

    /// <summary>
    /// To be called by whatever basic AI system we set up
    /// </summary>
    public void SeekPlayer()
    {
        navAgent.SetDestination(PlayerManagement.Player.position);
    }

    public void SeekQueen()
    {
        navAgent.SetDestination(Queen.Main.position);
    }

    public void Seek(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    public void ClearSeeking()
    {
        navAgent.ResetPath();
    }

    public void DisableNavigation(float duration)
    {
        if (restoreNavigationRoutine != null)
        {
            StopCoroutine(restoreNavigationRoutine);
        }
        restoreNavigationRoutine = StartCoroutine(RestoreNavigationRoutine(duration));
    }

    IEnumerator RestoreNavigationRoutine(float duration)
    {
        navAgent.updatePosition = false;
        rigid.isKinematic = false;
        yield return new WaitForSeconds(duration);
        navAgent.updatePosition = true;
        rigid.isKinematic = true;
        restoreNavigationRoutine = null;
    }
}
