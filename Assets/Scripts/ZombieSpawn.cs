using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Reference to your zombie prefab.
    public Transform teleportDoor; // The teleport door's position.
    public Transform destination; // The position where zombies will end up.
    public float minSpawnInterval = 1f; // Minimum time between zombie spawns in seconds.
    public float maxSpawnInterval = 5f; // Maximum time between zombie spawns in seconds.
    public float moveSpeed = 2f; // Speed at which zombies move.
    public QuizManager quizManager;

    private bool isSpawning = false;

    private void Start()
    {
        StartSpawningZombies();
    }

    private void StartSpawningZombies()
    {
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        int numberOfQuestions = quizManager.TotalQuestions;

        for (int i = 0; i < numberOfQuestions; i++)
        {
            if (quizManager != null && quizManager.QuestionsAvailable)
            {
                // Instantiate a zombie as a child of the canvas.
                GameObject zombie = Instantiate(zombiePrefab, GetComponent<Transform>());

                // Set the zombie's initial position to the teleport door's position.
                zombie.transform.position = teleportDoor.position;

                // Move the zombie from the teleport door to the destination.
                StartCoroutine(MoveZombie(zombie));

                // Randomly determine the spawn interval for the next zombie.
                float randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
                yield return new WaitForSeconds(randomSpawnInterval);
            }
        }
    }

    private IEnumerator MoveZombie(GameObject zombie)
    {
        Vector3 targetPosition = destination.position;

        while (Vector3.Distance(zombie.transform.position, targetPosition) > 0.1f)
        {
            // Move the zombie toward the destination.
            zombie.transform.position = Vector3.MoveTowards(zombie.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the zombie is precisely at the destination.
        zombie.transform.position = targetPosition;

        // Destroy the zombie when it reaches the destination or based on certain conditions.
        Destroy(zombie);
    }
}
