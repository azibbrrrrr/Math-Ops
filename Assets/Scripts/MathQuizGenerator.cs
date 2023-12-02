using System;
using UnityEngine;

public class MathQuizGenerator : MonoBehaviour
{
    public static QuestionAndAnswers CreateMathQuestion()
    {
        // Generate random numbers and operator
        int num1 = UnityEngine.Random.Range(1, 11); // Random number between 1 and 10
        int num2 = UnityEngine.Random.Range(1, 11); // Random number between 1 and 10
        char operatorChar;
        int operatorIndex = UnityEngine.Random.Range(0, 4); // 0: +, 1: -, 2: *, 3: /

        switch (operatorIndex)
        {
            case 1:
                operatorChar = '-';
                break;
            case 2:
                operatorChar = '*';
                break;
            case 3:
                operatorChar = '/';
                // Ensure that the division is exact and the result is an integer
                while (num1 % num2 != 0)
                {
                    num1 = UnityEngine.Random.Range(1, 11);
                    num2 = UnityEngine.Random.Range(1, 11);
                }
                break;
            default:
                operatorChar = '+';
                break;
        }

        QuestionAndAnswers qa = new QuestionAndAnswers();
        qa.Question = $"{num1} {operatorChar} {num2} = ?";
        qa.Answers = new string[4];

        // Note: The CorrectAnswer property is 1-based (starts from 1)
        int correctAnswer = UnityEngine.Random.Range(1, 5); // 1, 2, 3, or 4
        qa.CorrectAnswer = correctAnswer;

        for (int i = 1; i <= 4; i++)
        {
            if (i == correctAnswer)
            {
                // Store the correct answer at the (i - 1) index in the array
                qa.Answers[i - 1] = CalculateAnswer(num1, num2, operatorChar).ToString();
            }
            else
            {
                // Store wrong answers at their respective indices in the array
                qa.Answers[i - 1] = GenerateWrongAnswer(num1, num2, operatorChar).ToString();
            }
        }

        return qa;
    }

    private static int CalculateAnswer(int num1, int num2, char operatorChar)
    {
        switch (operatorChar)
        {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
            case '/':
                return num1 / num2;
            default:
                throw new ArgumentException("Invalid operator: " + operatorChar);
        }
    }

    private static string GenerateWrongAnswer(int num1, int num2, char operatorChar)
    {
        int correctAnswer = CalculateAnswer(num1, num2, operatorChar);
        int wrongAnswer = correctAnswer;
        while (wrongAnswer == correctAnswer)
        {
            wrongAnswer = UnityEngine.Random.Range(correctAnswer - 10, correctAnswer + 11);
        }
        return wrongAnswer.ToString();
    }
}
