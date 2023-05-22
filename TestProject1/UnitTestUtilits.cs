namespace BinaryTrey.Tests
{
    public class CTimerTests
    {
        [Test]
        public void Timer_ShouldReturnElapsedTimeAndResult()
        {
            // Arrange
            Func<int> function = () => 2 + 2;

            // Act
            var result = CTimer.Timer(function);

            // Assert
            Assert.That(result.Item1, Is.Not.Null);
            Assert.That(result.Item2, Is.EqualTo(4));
        }

        [Test]
        public void Timer_ShouldMeasureElapsedTime()
        {
            // Arrange
            Action function = () => { System.Threading.Thread.Sleep(5); };

            // Act
            var result = CTimer.Timer(function);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<string>());
        }
    }
}
