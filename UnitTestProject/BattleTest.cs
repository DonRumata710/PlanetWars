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
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 1, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 150 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 200 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 150 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 0, 200 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 0, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 150 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 200 } }), result = false },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 150 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }), result = true },
                new TestCase1 { army1 = new Fleet(0, new Dictionary<int, int>{ { 2, 200 } }),
                                army2 = new Fleet(0, new Dictionary<int, int>{ { 2, 100 } }), result = true }
            };

            foreach (TestCase1 testCase in testSet)
            {
                Assert.IsTrue(BattleLogic.ArrangeBattle(testCase.army1, testCase.army2) == testCase.result);
            }
        }
    }
}
