using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public GameObject player;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer");
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.Attack();
            // Calculate the time bonus multiplier
            float timeBonusMultiplier = 1 + TimePercentage();

            // Update the player's score with the time bonus
            int timeBonusScore = Mathf.RoundToInt(150 * timeBonusMultiplier);
            quizManager.score += timeBonusScore;

        }
        else
        {
            Debug.Log("Wrong Answer");
        }
        quizManager.removeCurrQuestion();
    }

    private float TimePercentage()
    {
        float totalTime = quizManager.questionTimeLimit;
        float timeRemaining = quizManager.timeRemaining;

        // Calculate the percentage of time remaining
        return timeRemaining / totalTime;
    }

}
