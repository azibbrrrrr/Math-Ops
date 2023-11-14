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
            quizManager.score++;

        }
        else
        {
            Debug.Log("Wrong Answer");
        }

        quizManager.nextQuestion();
    }
}
