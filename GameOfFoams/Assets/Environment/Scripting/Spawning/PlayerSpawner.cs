using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerPrefab;

    [SerializeField]
    protected Transform basicAttackPrefab;

    public Transform Construct(Vector3 playerPosition, Quaternion playerFacing)
    {
        Transform result = Instantiate(playerPrefab, playerPosition, playerFacing) as Transform;
        (Instantiate(basicAttackPrefab, result.Find("WeaponsHolder")) as Transform).Reset();
        return result;
    }
}
