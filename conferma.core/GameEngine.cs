using conferma.core.Domain;
using conferma.core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace conferma.core
{
    public class GameEngine
    {
        private IScoreRepository scoreRepository;
        public IConfiguration Config { get; }


        public GameEngine(IConfiguration config, IScoreRepository scoreRepository)
        {
            Config = config;
            this.scoreRepository = scoreRepository;
        }

        public void Run()
        {
            var pointsPerGuess = Config.GetValue<int>("pointsPerGuess");
            var pointsToWin = Config.GetValue<int>("pointsToWin");

            Console.WriteLine("---------------------");
            Console.WriteLine("Welcome to The Game!");
            Console.WriteLine("---------------------\r\n");
            string username = SetUsername();
            bool playAgain;

            do
            {
                var game = new Game(username, pointsPerGuess, pointsToWin);
                var current = game.GetNextRandomNumber();
                bool isCorrect;

                do
                {
                    Console.WriteLine($"\r\nThe current number is {current}, will the next one be (h)igher or (l)ower?");
                    var guess = Console.ReadLine();

                    while (string.IsNullOrWhiteSpace(guess) || !(guess.Contains("h", StringComparison.OrdinalIgnoreCase) || guess.Contains("l", StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine("Guess invalid, please re-enter:");
                        guess = Console.ReadLine();
                    }

                    var next = game.GetNextRandomNumber();
                    isCorrect = game.IsCorrect(guess, current, next);

                    if (isCorrect)
                    {
                        game.IncrementScore();
                        Console.WriteLine($"You were correct! Your current score is {game.CurrentPoints}");
                        current = next;

                        if (game.IsComplete)
                        {
                            Console.WriteLine($"\r\nCongratulations, You have won the game!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The number was {next}");
                        Console.WriteLine($"\r\nYour final score is {game.CurrentPoints}, Better luck next time!");
                       
                    }

                } while (!game.IsComplete && isCorrect);

                SaveGame(game);
                ShowLeaderBoard();
                playAgain = PlayAgain();

            } while (playAgain);
        }



        private static string SetUsername()
        {
            Console.WriteLine("Please enter a username:");

            var username = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username invalid, please re-enter:");
                username = Console.ReadLine();
            }

            return username;
        }

        private void SaveGame(Game game)
        {
            var time = game.Complete();

            if (game.CurrentPoints > 0)
            {
                var score = new Score { Username = game.Username, Points = game.CurrentPoints, TimeTaken = time };
                scoreRepository.SaveScore(score);
            }
        }

        private void ShowLeaderBoard()
        {
            var highScores = scoreRepository.GetHighScores();

            if (highScores.Any())
            {
                Console.WriteLine("\r\nLeaderboard");
                Console.WriteLine("---------------------");
                Console.WriteLine("|Username|Score|Time|");
                Console.WriteLine("---------------------");
                foreach (var highscore in highScores)
                {
                    Console.WriteLine($"{highscore.Username} - {highscore.Points} - { highscore.TimeTaken / 1000}s");
                }
            }
            else
            {
                Console.WriteLine("---No scores to show---");
            }
        }

        private bool PlayAgain()
        {
            Console.WriteLine($"\r\nPlay again? (y/n)");
            var input = Console.ReadLine();

            if(input == "y" || input == "Y")
            {
                return true;
            }

            return false;
        }
    }
}
