using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private int zombieCount = 25;
	[SerializeField] private float spawnAreaSize = 45f;
	[SerializeField] private float minDistanceFromPlayer = 10f;

	private GameObject _player;

	void Start()
	{
		_player = GameObject.Find("Player");
		SpawnZombies();
	}

	void SpawnZombies()
	{
		// Spawn our zombies
		for (int i = 0; i < zombieCount; i++)
		{
			Vector3 spawnPos = GetValidSpawnPosition();
			if (spawnPos != Vector3.zero)
			{
				Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
			}
		}
	}

	Vector3 GetValidSpawnPosition()
	{
		// Try to find a valid position (not inside a building)
		for (int attempts = 0; attempts < 50; attempts++)
		{
			// Generate a random position within the spawn area
			float x = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
			float z = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
			Vector3 position = new Vector3(x, 1.0f, z); // 1.0f is y-height above ground

			// Check if this position is far enough from the player
			if (_player != null && Vector3.Distance(position, _player.transform.position) < minDistanceFromPlayer)
			{
				continue; // Too close to player, try again
			}

			// Check if there's anything above this position (building)
			if (Physics.Raycast(position + Vector3.up * 10, Vector3.down, out RaycastHit hit, 20f))
			{
				// If hit the ground (not a building)
				if (hit.collider.gameObject.layer == 0)
				{ // Default layer
					position.y = hit.point.y + 0.1f; // Place just above ground

					// Finally check if this position is inside any collider (building)
					Collider[] colliders = Physics.OverlapSphere(position, 1.0f);
					bool isValid = true;

					foreach (var collider in colliders)
					{
						// Skip ground/terrain colliders
						if (collider.gameObject.layer != 0)
						{ // Non-default layer
							isValid = false;
							break;
						}
					}

					if (isValid)
					{
						return position;
					}
				}
			}
		}

		// If we couldn't find a valid position after max attempts
		return Vector3.zero;
	}
}