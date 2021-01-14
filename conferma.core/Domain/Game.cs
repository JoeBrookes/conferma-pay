using System;
using System.Diagnostics;

namespace conferma.core.Domain
{
    public class Game
    {
        private readonly int pointsPerGuess;
        private readonly int pointsToWin;
        private int currentRandomNumber = 0;
        private Random randomNumberGenerator;
        private Stopwatch timer;


        public string Username { get; private set; }
        public int CurrentPoints { get; private set; }
        public bool IsComplete => CurrentPoints >= pointsToWin;


        public Game(string username, int pointsPerGuess, int pointsToWin)
        {
            Username = username;
            this.pointsPerGuess = pointsPerGuess;
            this.pointsToWin = pointsToWin;
            randomNumberGenerator = new Random();
            currentRandomNumber = randomNumberGenerator.Next(1, 100);
            timer = new Stopwatch();
            timer.Start();
        }

        public bool IsCorrect(string guess, int currentValue, int nextValue)
        {
            var correct = false;

            if (guess == "h" || guess == "H")
            {
                if (nextValue > currentValue)
                {
                    correct = true;
                }
            }

            if (guess == "l" || guess == "L")
            {
                if (nextValue < currentValue)
                {
                    correct = true;
                }
            }

            return correct;
        }

        public void IncrementScore()
        {
            CurrentPoints += pointsPerGuess;
        }

        public int GetNextRandomNumber()
        {
            var value = randomNumberGenerator.Next(1, 100);

            //incase we get the same number
            while (value == currentRandomNumber)
            {
                value = randomNumberGenerator.Next(1, 100);
            }

            currentRandomNumber = value;
            return currentRandomNumber;
        }

        public long Complete()
        {
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
    }
}
