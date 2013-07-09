namespace TinCanAPILibraryUnitTests.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

    [TestFixture]
    public class ContextFixture
    {
        [Test]
        public void Validate_produces_no_failures_for_valid_Context()
        {
            var context = CreateValidContext();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_non_UUID_registration()
        {
            var context = new Context()
            {
                Registration = "Not a valid UUID"
            };
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_bubbles_failures_from_invalid_instructor()
        {
            var context = CreateValidContext();
            context.Instructor = ActorFixture.CreateInvalidAgent();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_bubbles_failures_from_invalid_team()
        {
            var context = CreateValidContext();
            context.Team = GroupFixture.CreateInvalidGroup();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_bubbles_failures_from_invalid_contextActivities()
        {
            var context = CreateValidContext();
            context.ContextActivities = ContextActivitiesFixture.CreateInvalidContextActivities();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_language_is_not_RFC_5646_compliant()
        {
            var context = CreateValidContext();
            context.Language = "not a valid language tag";
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_bubbles_failures_from_invalid_StatementRef()
        {
            var context = CreateValidContext();
            context.Statement = StatementRefFixture.CreateInvalidStatementRef();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidContext_output()
        {
            var context = CreateInvalidContext();
            var rawResults = context.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        internal static Context CreateValidContext()
        {
            return new Context()
            {
                Registration = "620F22AD-4F42-4345-90FD-7DF51A2C4473"
            };
        }

        internal static Context CreateInvalidContext()
        {
            return new Context()
            {
                Registration = "Not a valid UUID"
            };
        }
    }
}
