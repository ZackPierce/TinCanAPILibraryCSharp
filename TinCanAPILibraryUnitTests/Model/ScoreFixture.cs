using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class ScoreFixture
    {
        [Test]
        public void Validate_passes_empty_score_with_no_failures()
        {
            var score = new Score();
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_reports_failure_for_min_max_mismatch()
        {
            var score = new Score() { 
                Min = 2,
                Max = 1
            };
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failure_for_scaled_greater_than_1()
        {
            var score = new Score()
            {
                Scaled = 2.0
            };
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failure_for_scaled_less_than_0()
        {
            var score = new Score()
            {
                Scaled = -2.0
            };
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failure_for_raw_less_than_min()
        {
            var score = new Score()
            {
                Raw = 1,
                Min = 2
            };
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failure_for_raw_greater_than_max()
        {
            var score = new Score()
            {
                Raw = 2,
                Max = 1
            };
            var failures = new List<ValidationFailure>(score.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }
    }
}
