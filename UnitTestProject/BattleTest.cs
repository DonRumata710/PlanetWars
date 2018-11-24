using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Server.GameLogic;


namespace LogicTests
{
    [TestClass]
    public class BattleTest
    {
        struct TestCase1
        {
            public Fleet army1;
            public Fleet army2;
            public bool result;
        }


        [TestMethod]
        public void TestMethod1()
        {
            TestCase1[] testSet = new TestCase1[]
            {
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 150 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 200 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 150 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 200 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 150 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 200 } }, null), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 150 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 200 } }, null),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }, null), result = true }
            };

            foreach (TestCase1 testCase in testSet)
            {
                Assert.IsTrue(BattleLogic.ArrangeBattle(testCase.army1, testCase.army2) == testCase.result);
                Assert.IsTrue(testCase.result ? testCase.army2.IsEmpty : testCase.army1.IsEmpty);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            int player1 = 1;
            int player2 = 2;

            Planet planet = new Planet(6, player1);

            Fleet fleet1 = new Fleet(player1, new Dictionary<int, int> { { 0, 100 } }, planet);
            planet.AppendArmy(fleet1);

            Fleet fleet2 = new Fleet(player2, new Dictionary<int, int> { { 2, 100 } }, null);

            fleet2.Attack(planet, 0.0);

            Assert.IsTrue(planet.Owner == player2);
            Assert.IsTrue(fleet1.IsEmpty);

            fleet1 = new Fleet(player1, new Dictionary<int, int> { { 0, 100 } }, null);
            fleet1.Attack(planet, 0.0);

            Assert.IsTrue(planet.Owner == player2);
            Assert.IsTrue(fleet1.IsEmpty);
        }
    }
}
