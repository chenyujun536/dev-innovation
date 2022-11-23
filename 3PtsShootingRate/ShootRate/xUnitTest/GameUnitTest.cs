using ShootRate.Models;
using Xunit;

namespace xUnitTest;
public class GameUnitTest
{
    [Fact]
    public void Game_HitsInDisplayFormat()
    {
        //Game game = new Game()
        //{
        //    Date = DateTime.Now,
        //    Tries = 36,
        //    Hits = ",0,3,1,3,45,65"
        //};

        //Assert.True(0 == game.HitsInDisplayFormat.First<int>());

        string[] results = new[]
        {
            "1,2,3,4",
            "1,2,3"
        };

        var sum = results.SelectMany(x => x.Split(',')).Select(x => Convert.ToInt32(x)).Sum();

        Assert.Equal(16, sum);

    }
}