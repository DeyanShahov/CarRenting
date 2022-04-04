using Xunit;

namespace CarRenting.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1, 1);
        }

        [Theory]
        [InlineData(1, 4, 5)]
        [InlineData(1, 2, 3)]
        public void Test2(int x, int y, int sum)
        {
            Assert.Equal(sum, x + y);
        }
    }
}