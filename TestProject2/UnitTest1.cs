namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var value = 20;

            Assert.AreEqual(20, value);
        }
    }
}