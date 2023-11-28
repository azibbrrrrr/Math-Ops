using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Reference to your zombie prefab.
    public Transform teleportDoor; // The teleport door's position.
    public Transform destination; // The position where zombies will end up.
    // public float minSpawnInterval = 3f; // Minimum time between zombie spawns in seconds.
    // public float maxSpawnInterval = 5f; // Maximum time between zombie spawns in seconds.
    public float moveSpeed = 1.5f; // Speed at which zombies move.
    public QuizManager quizManager;
    public int numberOfZombies;

    private Coroutine zombieSpawningCoroutine;

    private void Start()
    {
        StartSpawningZombies();
    }

    private void StartSpawningZombies()
    {
        if (zombieSpawningCoroutine == null)
        {
            zombieSpawningCoroutine = StartCoroutine(SpawnZombies());
        }
    }

    private IEnumerator SpawnZombies()
    {
        while (quizManager.QuestionsAvailable)
        {
            SpawnZombie();

            // Wait for 3 seconds before enabling the quiz canvas again.
            yield return new WaitForSeconds(3f);

            // Check if the game is still answering a question before generating the next question.
            while (quizManager.AnsweringQuestion)
            {
                yield return null;
            }

            // Generate the next question.
            quizManager.generateQuestion();
        }

        // The game is over, handle this as needed.
    }

    private void SpawnZombie()
    {
        // Instantiate a zombie as a child of the canvas.
        GameObject zombie = Instantiate(zombiePrefab, GetComponent<Transform>());
        // Set the zombie's initial position to the teleport door's position.
        zombie.transform.position = teleportDoor.position;

        // Move the zombie from the teleport door to the destination.
        StartCoroutine(MoveZombie(zombie));
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
