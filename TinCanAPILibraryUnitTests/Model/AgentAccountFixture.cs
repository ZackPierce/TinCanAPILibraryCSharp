using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class AgentAccountFixture
    {

        [Test]
        public void Validate_produces_no_failures_for_a_correct_instance()
        {
            var account = new AgentAccount()
            {
                    HomePage = "http://example.com",
                    Name = "bob"
            };
            var failures = new List<ValidationFailure>(account.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_Name()
        {
            var account = new AgentAccount()
            {
                HomePage = "http://example.com",
                Name = null
            };
            var failures = new List<ValidationFailure>(account.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_HomePage()
        {
            var account = new AgentAccount()
            {
                HomePage = null,
                Name = "bob"
            };
            var failures = new List<ValidationFailure>(account.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_non_IRI_HomePage()
        {
            var account = new AgentAccount()
            {
                HomePage = "[]{}{not a proper IRI}",
                Name = "bob"
            };
            var failures = new List<ValidationFailure>(account.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_relative_IRI_HomePage()
        {
            var account = new AgentAccount()
            {
                HomePage = "relativeIRI",
                Name = "bob"
            };
            var failures = new List<ValidationFailure>(account.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }
    }
}
