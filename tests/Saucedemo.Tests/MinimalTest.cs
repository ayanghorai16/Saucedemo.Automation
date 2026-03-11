using NUnit.Framework;
using Allure.NUnit;

namespace Saucedemo.Tests
{
    [AllureNUnit]
    public class MinimalTest
    {
        [Test]
        public void SimpleTest()
        {
            Assert.Pass();
        }
    }
}
