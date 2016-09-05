using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerPrefab;

    private Transform _player;
    public Transform player { get { return _player; } }

    public PlayerSpawner Construct(Vector3 playerPosition, Quaternion playerFacing)
    {
        _player = Instantiate(playerPrefab, playerPosition, playerFacing) as Transform;
        return this;
    }
}
