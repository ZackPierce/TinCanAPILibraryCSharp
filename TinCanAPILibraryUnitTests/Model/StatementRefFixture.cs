namespace TinCanAPILibraryUnitTests.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

    [TestFixture]
    public class StatementRefFixture
    {
        [Test]
        public void Validate_produces_no_failures_for_valid_StatementRef()
        {
            var statementRef = CreateValidStatementRef();
            var rawResults = statementRef.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_id()
        {
            var statementRef = new StatementRef()
            {
                Id = null
            };
            var rawResults = statementRef.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_non_UUID_id()
        {
            var statementRef = new StatementRef()
            {
                Id = "not a valid UUID"
            };
            var rawResults = statementRef.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidStatementRef_output()
        {
            var statementRef = CreateInvalidStatementRef();
            var rawResults = statementRef.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        internal static StatementRef CreateValidStatementRef()
        {
            return new StatementRef()
            {
                Id = "BFA54C2F-AF1D-493A-9951-5D6011791003"
            };
        }

        internal static StatementRef CreateInvalidStatementRef()
        {
            return new StatementRef()
            {
                Id = "not a valid UUID"
            };
        }
    }
}
