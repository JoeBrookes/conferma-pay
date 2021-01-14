using conferma.core.Domain;
using System;
using Xunit;

namespace conferma.tests
{
    public class GameTests
    {
        [Fact]
        public void Correct_High_Guess_Returns_True()
        {
            var game = new Game("test-user", 1, 10);

            var result = game.IsCorrect("h", 1, 2);

            Assert.True(result);
        }

        [Fact]
        public void Incorrect_High_Guess_Returns_False()
        {
            var game = new Game("test-user", 1, 10);

            var result = game.IsCorrect("h", 2, 1);

            Assert.False(result);
        }

         [Fact]
        public void Correct_Low_Guess_Returns_True()
        {
            var game = new Game("test-user", 1, 10);

            var result = game.IsCorrect("l", 50, 40);

            Assert.True(result);
        }

        [Fact]
        public void Incorrect_Low_Guess_Returns_False()
        {
            var game = new Game("test-user", 1, 10);

            var result = game.IsCorrect("l", 20, 30);

            Assert.False(result);
        }

        [Fact]
        public void IsComplete_Flag_Returns_Correct_Value()
        {
            var game = new Game("test-user", 2, 5);

            Assert.Equal(0, game.CurrentPoints);
            Assert.False(game.IsComplete);

            game.IncrementScore();

            Assert.Equal(2, game.CurrentPoints);
            Assert.False(game.IsComplete);

            game.IncrementScore();

            Assert.Equal(4, game.CurrentPoints);
            Assert.False(game.IsComplete);

            game.IncrementScore();

            Assert.Equal(6, game.CurrentPoints);
            Assert.True(game.IsComplete);
        }
    }
}
