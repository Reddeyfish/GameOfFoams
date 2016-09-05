using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiates all entities, including enemies and the player
/// </summary>
public class EntitySpawner : MonoBehaviour {

    [SerializeField]
    protected Transform playerSpawnerPrefab;

    /// <summary>
    /// Temporary, until we create the wave spawning logic.
    /// </summary>
    [SerializeField]
    protected Transform enemyPrefab;

    PlayerSpawner playerSpawner;

	// Use this for initialization
	void Awake () {
        playerSpawner = (Instantiate(playerSpawnerPrefab, this.transform) as Transform).GetComponent<PlayerSpawner>().Construct(Vector3.zero, Quaternion.identity);

        Vector2 enemyPosition = Random.insideUnitCircle * 30;
        Vector3 worldSpaceEnemyPosition = new Vector3(enemyPosition.x, 0, enemyPosition.y);

        //Temporary, until wave spawning logic is set up
        (Instantiate(enemyPrefab, worldSpaceEnemyPosition, Quaternion.identity) as Transform).GetComponent<EnemyNavigation>().Construct(playerSpawner.player);
	}
}
