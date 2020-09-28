using System;
using DogRaceNew.RaceEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetGameTesting
{
    [TestClass]
    public class MyUnitTest
    {
        [TestMethod]
        public void TestFactory()
        {
            Punter punter = Factory.GetAPunter("AI");
            bool result = punter is AI;
            Assert.AreEqual(result, true);
        }
    }
}
