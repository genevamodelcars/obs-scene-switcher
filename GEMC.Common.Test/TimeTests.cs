using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GEMC.Common.Test
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void TestTrivialValues()
        {
            Time a = new Time("00:00:50");
            Time b = new Time("-00:00:50");
            Time c = new Time("00:00:10");
            Time d = new Time("00:00:00");
            Time e = Time.Now();

            Time sum = a + b;

            Assert.AreEqual(0, sum.Seconds);
            Assert.AreEqual(0, sum.Minutes);

            sum = a + c;

            Assert.AreEqual(0, sum.Seconds);
            Assert.AreEqual(1, sum.Minutes);

            sum = d + e;

            Assert.AreEqual(e.Seconds, sum.Seconds);
            Assert.AreEqual(e.Minutes, sum.Minutes);
        }

        [TestMethod]
        public void TestComplexSubtractions()
        {
            Time a = new Time("01:00:00");
            Time b = new Time("-00:35:25");

            Time sum = a + b;

            Assert.AreEqual(35, sum.Seconds);
            Assert.AreEqual(24, sum.Minutes);
            Assert.AreEqual(0, sum.Hours);

            Time c = new Time("01:00:00");
            Time d = new Time("00:35:25");

            Time diff = c - d;

            Assert.AreEqual(35, sum.Seconds);
            Assert.AreEqual(24, sum.Minutes);
            Assert.AreEqual(0, sum.Hours);

            Time e = new Time("00:16:00");
            Time f = new Time("00:35:25");

            diff = e-f;

            Assert.AreEqual(25, diff.Seconds);
            Assert.AreEqual(19, diff.Minutes);
            Assert.AreEqual(0, diff.Hours);
            Assert.AreEqual(true, diff.IsNegative);
        }

        [TestMethod]
        public void TestAdditions()
        {
            Time a = new Time("01:17:45");
            Time b = new Time("02:07:25");

            Time sum = a + b;

            Assert.AreEqual(10, sum.Seconds);
            Assert.AreEqual(25, sum.Minutes);
            Assert.AreEqual(3, sum.Hours);

            Time c = new Time("12:48:49");
            Time d = new Time("00:24:24");

            sum = c + d;

            Assert.AreEqual(13, sum.Seconds);
            Assert.AreEqual(13, sum.Minutes);
            Assert.AreEqual(13, sum.Hours);

        }

        [TestMethod]
        public void TestExceptions()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                Time a = new Time("96:17:45");
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                Time a = new Time("00:87:45");
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                Time a = new Time("00:00:115");
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                Time a = new Time("prout");
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                Time a = new Time(":::");
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                Time a = new Time("ae:32:xy");
            });
        }

        [TestMethod]
        public void TestLowerThan()
        {
            Time a = new Time("00:00:10");
            Time b = new Time("00:00:12");

            bool result = a < b;

            Assert.AreEqual(true, result);

            a = new Time("00:01:10");
            b = new Time("00:02:12");

            result = a < b;

            Assert.AreEqual(true, result);

            a = new Time("01:01:10");
            b = new Time("02:02:12");

            result = a < b;

            Assert.AreEqual(true, result);

            a = new Time("01:01:10");
            b = new Time("00:02:12");

            result = a < b;

            Assert.AreEqual(false, result);

            a = new Time("01:01:10");
            b = new Time("01:01:10");

            result = a < b;

            Assert.AreEqual(false, result);

            a = new Time("00:03:45");
            b = new Time("-00:06:12");

            result = a < b;

            Assert.AreEqual(false, result);

            a = new Time("00:00:00");
            b = new Time("-00:00:00");

            result = a < b;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestGreaterThan()
        {
            Time a = new Time("00:00:10");
            Time b = new Time("00:00:12");

            bool result = a > b;

            Assert.AreEqual(false, result);

            a = new Time("00:01:10");
            b = new Time("00:02:12");

            result = a > b;

            Assert.AreEqual(false, result);

            a = new Time("01:01:10");
            b = new Time("02:02:12");

            result = a > b;

            Assert.AreEqual(false, result);

            a = new Time("01:01:10");
            b = new Time("00:02:12");

            result = a > b;

            Assert.AreEqual(true, result);

            a = new Time("01:01:10");
            b = new Time("01:01:10");

            result = a > b;

            Assert.AreEqual(true, result);

            a = new Time("00:03:45");
            b = new Time("-00:06:12");

            result = a > b;

            Assert.AreEqual(true, result);

            a = new Time("00:00:00");
            b = new Time("-00:00:00");

            result = a > b;

            Assert.AreEqual(true, result);
        }
    }
}
