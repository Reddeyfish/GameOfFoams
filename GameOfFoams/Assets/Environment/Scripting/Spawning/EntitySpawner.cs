using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiates all entities, including enemies and the player
/// </summary>
public class EntitySpawner : MonoBehaviour {

    Transform player;
    Transform queen;

	// Use this for initialization
	void Awake () {
        /*
        PlayerSpawner playerSpawner = GetComponentInChildren<PlayerSpawner>();
        player = playerSpawner.Build(Vector3.zero, Quaternion.identity);

        Vector3 queenPosition = Random.insideUnitCircle * 10;
        Vector3 worldSpaceQueenPosition = new Vector3(queenPosition.x, 0, queenPosition.y);

        QueenSpawner queenSpawner = GetComponentInChildren<QueenSpawner>();
        queen = queenSpawner.Build(worldSpaceQueenPosition, Quaternion.identity);

        //Temporary, until wave spawning logic is set up
        foreach (EnemySpawner enemySpawner in GetComponentsInChildren<EnemySpawner>())
        {
            Vector2 enemyPosition = Random.insideUnitCircle * 30;
            Vector3 worldSpaceEnemyPosition = new Vector3(enemyPosition.x, 0, enemyPosition.y);
            enemySpawner.Build(worldSpaceEnemyPosition, Quaternion.identity, player, queen);
        }

        foreach (BarrierSpawner enemySpawner in GetComponentsInChildren<BarrierSpawner>())
        {
            Vector2 barrierPosition = Random.insideUnitCircle * 15;
            Vector3 worldSpaceBarrierPosition = new Vector3(barrierPosition.x, 0, barrierPosition.y);
            enemySpawner.Build(worldSpaceBarrierPosition, Quaternion.identity);
        }
         * */
	}
}
