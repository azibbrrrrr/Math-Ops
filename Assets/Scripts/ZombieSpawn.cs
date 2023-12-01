using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Reference to your zombie prefab.
    public Transform teleportDoor; // The teleport door's position.
    public Transform destination; // The position where zombies will end up.
    public float moveSpeed = 1.5f; // Speed at which zombies move.
    public QuizManager quizManager;
    public int numberOfZombies;

    private Dictionary<GameObject, ZombieData> zombieDataMap = new Dictionary<GameObject, ZombieData>();
    private Queue<GameObject> spawnedZombies = new Queue<GameObject>();

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
            if (spawnedZombies.Count < 2)
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
        }

        // The game is over, handle this as needed.
    }

    private void SpawnZombie()
    {
        // Instantiate a zombie as a child of the canvas.
        GameObject zombieObject = Instantiate(zombiePrefab, GetComponent<Transform>());

        // Set the zombie's initial position to the teleport door's position.
        zombieObject.transform.position = teleportDoor.position;

        // Generate a random speed for the zombie.
        float speed = Random.Range(moveSpeed * 0.5f, moveSpeed * 1.5f);

        // Create ZombieData to store zombie-specific data
        ZombieData zombieData = new ZombieData(zombieObject, speed);

        // Add zombie and its corresponding ZombieData to the dictionary
        zombieDataMap.Add(zombieObject, zombieData);

        // Add the spawned zombie to the queue
        spawnedZombies.Enqueue(zombieObject);

        // Move the zombie from the teleport door to the destination.
        StartCoroutine(MoveZombie(zombieData));
    }

    public void HitByBullet(GameObject zombieObject)
    {
        if (zombieDataMap.TryGetValue(zombieObject, out var zombieData))
        {
            zombieData.IsHit = true;
            Debug.Log("Zombie get hit");
        }

        // Remove the destroyed zombie from the queue
        if (spawnedZombies.Count > 0)
        {
            spawnedZombies.Dequeue();
        }
    }

    private IEnumerator MoveZombie(ZombieData zombieData)
    {
        Vector3 targetPosition = destination.position;

        while (!zombieData.IsHit && Vector3.Distance(zombieData.Zombie.transform.position, targetPosition) > 0.1f)
        {
            zombieData.Zombie.transform.position = Vector3.MoveTowards(zombieData.Zombie.transform.position, targetPosition, zombieData.Speed * Time.deltaTime);
            yield return null; // important to yield control back to Unity
        }

        // Destroy the zombie only if it hasn't been hit.
        if (!zombieData.IsHit)
        {
            Destroy(zombieData.Zombie);
            Debug.Log("Zombie Destroyed");
        }
    }

    // Class to store zombie-specific data
    public class ZombieData
    {
        public GameObject Zombie { get; }
        public float Speed { get; }
        public bool IsHit { get; set; }

        public ZombieData(GameObject zombie, float speed)
        {
            Zombie = zombie;
            Speed = speed;
            IsHit = false;
        }
    }
}
