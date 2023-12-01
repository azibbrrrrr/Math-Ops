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
    private bool _answeringQuestion = false;
    public GameObject[] options; //buttons
    public int currentQuestion;
    public int questionNo;
    public Text queNo;
    public TMP_Text QuestionTxt;
    public Text timeLimitTxt;
    public Text scoreTxt;
    public Text PlayerPoints;
    public Text points_gain;
    public Text streak_bonus;
    public GameObject pointsGain;
    public int score; //player's score
    private int streakCounter = 0;
    private int streakThreshold = 3;
    public Text streakNo;
    public GameObject gameScore;
    public GameObject quizCanvas;
    public float questionTimeLimit = 30f; // Time limit for answering each question
    public float timeRemaining; // Remaining time for the current question
    public GameObject player;
    private Material playerMaterial;

    private void Start()
    {
        // gameScore.SetActive(false);
        quizCanvas.SetActive(false);
        playerMaterial = player.GetComponent<Renderer>().material;
        ResetPlayerShader();
        QnA = new List<QuestionAndAnswers>(); // Initialize the QnA list
        for (int i = 0; i < TotalQuestions; i++)
        {
            QnA.Add(MathQuizGenerator.CreateMathQuestion());
        }
    }

    private void ResetPlayerShader()
    {
        playerMaterial.DisableKeyword("OUTBASE_ON");
    }

    private void SetPlayerOnStreakFire()
    {
        playerMaterial.EnableKeyword("OUTBASE_ON");
    }

    private void Update()
    {
        PlayerPoints.text = score.ToString();
        if (QnA.Count <= 0)
        {
            QuestionTxt.text = "Quiz Completed!";
            _questionsAvailable = false;
            GameOver();
        }
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

    public void removeCurrQuestion()
    {
        if (currentQuestion >= 0 && currentQuestion < QnA.Count)
        {
            QnA.RemoveAt(currentQuestion);
            ResumeGame();
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
    public void generateQuestion()
    {
        questionNo++;
        queNo.text = "Question " + questionNo;
        if (QnA.Count > 0)
        {
            currentQuestion = UnityEngine.Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();

            // Set the flag to indicate that the player is answering a question
            _answeringQuestion = true;
            EnableQuizCanvas();
            // Start the timer for answering the question
            StartCoroutine(AnswerTimer());
            PauseGame();
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


    private IEnumerator AnswerTimer()
    {
        float timer = questionTimeLimit;
        while (timer > 0f && _answeringQuestion)
        {
            // Update UI or do other things related to the timer (if needed)
            yield return null;
            string timerFormatted = FormatTimer(timer);
            timeLimitTxt.text = timerFormatted;

            timeRemaining = timer;

            timer -= Time.unscaledDeltaTime;

            // Ensure the timer doesn't go below 0
            timer = Mathf.Max(timer, 0f);
        }
    }

    private string FormatTimer(float timeInSeconds)
    {
        // Convert the time to a TimeSpan to easily format minutes and seconds.
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);

        // Use string interpolation to format the timer as "m:ss".
        string formattedTime = $"{timeSpan.Minutes:D}:{timeSpan.Seconds:D2}";

        return formattedTime;
    }

    void PauseGame()
    {
        // Pause the game by setting Time.timeScale to 0.
        Time.timeScale = 0f;
        _answeringQuestion = true;
    }

    void ResumeGame()
    {
        // Resume the game by setting Time.timeScale back to 1.
        Time.timeScale = 1f;
        quizCanvas.SetActive(false);
        _answeringQuestion = false;
    }

    void EnableQuizCanvas()
    {
        quizCanvas.SetActive(true);
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

    public void AnswerCorrect()
    {

        streakCounter++;
        streakNo.text = streakCounter.ToString();
        if (streakCounter >= streakThreshold)
        {
            SetPlayerOnStreakFire();
        }
        // Shoot zombie
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.Attack();
        IncreaseScore();
    }

    // Call this method when the player answers incorrectly
    public void AnswerIncorrect()
    {
        streakCounter = 0;
        streakNo.text = streakCounter.ToString();
        ResetPlayerShader();
    }

    private void IncreaseScore()
    {
        float timeBonusMultiplier = 1 + TimePercentage();
        int timeBonusScore = Mathf.RoundToInt(100 * timeBonusMultiplier);

        points_gain.text = $"+{timeBonusScore} XP";

        int questionScore = CalculateQuestionScore(timeBonusScore);

        pointsGain.GetComponent<points_gain>().Triggeranimator();

        score += questionScore;
    }

    private int CalculateQuestionScore(int timeBonusScore)
    {
        int streakBonus = (streakCounter >= streakThreshold) ? streakCounter * 20 : 0;
        streak_bonus.text = (streakBonus > 0) ? $"+{streakBonus} XP" : " ";

        return timeBonusScore + streakBonus;
    }


    private float TimePercentage()
    {
        // Calculate the percentage of time remaining
        return timeRemaining / questionTimeLimit;
    }

    public bool QuestionsAvailable
    {
        get { return _questionsAvailable; }
        set { _questionsAvailable = value; }
    }

    public bool AnsweringQuestion
    {
        get { return _answeringQuestion; }
        set { _answeringQuestion = value; }
    }

}
