using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerPrefab;

    public Transform Construct(Vector3 playerPosition, Quaternion playerFacing)
    {
        return Instantiate(playerPrefab, playerPosition, playerFacing) as Transform;
    }
}
