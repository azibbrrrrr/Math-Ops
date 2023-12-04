using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScript : MonoBehaviour
{
    public Text countdownText;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int countdownTime = 3;

        while (countdownTime > 0)
        {
            // Display countdown text
            countdownText.text = countdownTime.ToString();

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease countdown
            countdownTime--;
        }
        // Wait for 1 second after "Go!"
        yield return new WaitForSeconds(1f);

        // Perform actions after "Go!" (e.g., start the game)
        ZombieSpawner spawn = FindObjectOfType<ZombieSpawner>();
        spawn.StartGame();
    }
}