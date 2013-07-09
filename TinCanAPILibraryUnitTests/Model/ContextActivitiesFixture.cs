namespace TinCanAPILibraryUnitTests.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

    [TestFixture]
    public class ContextActivitiesFixture
    {
        [Test]
        public void Validate_produces_no_failures_for_simple_valid_ContextActivities()
        {
            var context = CreateValidContextActivities();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidContextActivities_output()
        {
            var context = CreateInvalidContextActivities();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        internal static ContextActivities CreateValidContextActivities()
        {
            return new ContextActivities()
            {
                Parent = new Activity[]
                {
                    ActivityFixture.CreateValidActivity()
                }
            };
        }

        internal static ContextActivities CreateInvalidContextActivities()
        {
            return new ContextActivities()
            {
                Category = new Activity[]
                {
                    ActivityFixture.CreateInvalidActivity()
                }
            };
        }
    }
}
