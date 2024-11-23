using System;

public static class MathProblemGenerator
{
    private static Random rand = new Random();

    public static string currentProblem;
    public static string currentAnswer;

    public static string GenerateAlgebraProblem()
    {
        int a = rand.Next(1, 10);
        int b = rand.Next(1, 10);
        int c = rand.Next(1, 10) + b; // Ensure c > b

        currentProblem = $"Solve for x (answer in fractions if necessary and Don't Simplify for fractions): {a}x + {b} = {c}";
        if ((c - b) % a == 0)
        {
            currentAnswer = (c - b) / a + "";
        }

        else
        {
            currentAnswer = (c - b) + "/" + a;
        }

        return currentProblem;
    }

    public static string GenerateCalculusProblem()
    {
        int a = rand.Next(1, 5);
        int n = rand.Next(2, 5);

        currentProblem = $"Compute the derivative: d/dx ({a}x^{n})";
        if ((n - 1) == 0)
        {
            currentAnswer = (a * n) + "";
        }

        else if ((n - 1) == 1)
        {
            currentAnswer = (a * n) + "x";
        }

        else
        {
            currentAnswer = (a * n) + "x^" + (n - 1);
        }

        return currentProblem;
    }

    public static bool CheckAnswer(string userInput)
    {
        return userInput == currentAnswer;
    }
}
