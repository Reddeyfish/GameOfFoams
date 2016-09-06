using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiates all entities, including enemies and the player
/// </summary>
public class EntitySpawner : MonoBehaviour {

    Transform player;

	// Use this for initialization
	void Awake () {
        PlayerSpawner playerSpawner = GetComponentInChildren<PlayerSpawner>();
        player = playerSpawner.Construct(Vector3.zero, Quaternion.identity);

        //Temporary, until wave spawning logic is set up
        foreach (EnemySpawner enemySpawner in GetComponentsInChildren<EnemySpawner>())
        {
            Vector2 enemyPosition = Random.insideUnitCircle * 30;
            Vector3 worldSpaceEnemyPosition = new Vector3(enemyPosition.x, 0, enemyPosition.y);
            enemySpawner.Build(worldSpaceEnemyPosition, Quaternion.identity, player);
        }
	}
}
