using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public int TotalQuestions;
    private List<QuestionAndAnswers> QnA;
    private bool _questionsAvailable = true;
    public GameObject[] options; //buttons
    public int currentQuestion;
    public int questionNo;
    public Text queNo;

    public TMP_Text QuestionTxt;
    public Text scoreTxt;
    public int score; //player's score
    public GameObject gameScore;

    private void Start()
    {
        gameScore.SetActive(false);
        QnA = new List<QuestionAndAnswers>(); // Initialize the QnA list
        for (int i = 0; i < TotalQuestions; i++)
        {
            QnA.Add(MathQuizGenerator.CreateMathQuestion());
        }
        generateQuestion(); // get a question from a list of all questions
    }

    void GameOver()
    {
        scoreTxt.text = score + "/" + TotalQuestions;
        gameScore.SetActive(true);
    }

    public void retry()
    {
        gameScore.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextQuestion()
    {
        if (currentQuestion >= 0 && currentQuestion < QnA.Count)
        {
            QnA.RemoveAt(currentQuestion);
            generateQuestion();
        }
    }

    // set asnwers for each button
    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

    }

    // get a question from a list of all questions
    void generateQuestion()
    {
        questionNo++;
        queNo.text = "Question " + questionNo;
        if (QnA.Count > 0)
        {
            currentQuestion = UnityEngine.Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else
        {
            // No more questions, handle end of the quiz
            QuestionTxt.text = "Quiz Completed!";
            _questionsAvailable = false;
            GameOver();
            // You can add additional logic here to handle the end of the quiz.
        }
    }

    void print_ListQnA()
    {
        foreach (QuestionAndAnswers item in QnA)
        {
            Console.WriteLine("Question: " + item.Question);
            Console.WriteLine("Answers:");

            foreach (string answer in item.Answers)
            {
                Console.WriteLine(answer);
            }

            Console.WriteLine("Correct Answer: " + item.CorrectAnswer);
            Console.WriteLine(); // Empty line for separation
        }

    }

    public bool QuestionsAvailable
    {
        get { return _questionsAvailable; }
        set { _questionsAvailable = value; }
    }

}
