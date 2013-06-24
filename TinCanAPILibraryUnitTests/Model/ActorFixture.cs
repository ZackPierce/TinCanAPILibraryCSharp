using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class ActorFixture
    {
        private Actor actor;

        [TearDown]
        public void TearDown()
        {
            actor = null;
        }

        [Test]
        public void Empty_constructor_does_not_throw_errors()
        {
            Assert.DoesNotThrow(() => new Actor());
        }

        [Test]
        public void Default_actor_ObjectType_is_Agent()
        {
            actor = new Actor();
            Assert.AreEqual("Agent", actor.ObjectType);
        }

        [Test]
        public void Validate_returns_no_failures_for_simple_valid_Agent()
        {
            actor = new Actor();
            actor.Mbox = "mailto:john@example.com";
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_missing_inverse_functional_identifier()
        {
            actor = new Actor();
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }


        [Test]
        public void Validate_returns_a_failure_for_invalid_mbox()
        {
            actor = new Actor()
            {
                Mbox = "Not a in the valid mailto:address format"
            };
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_invalid_openid()
        {
            actor = new Actor()
            {
                Openid = "[]{} not a valid IRI"
            };
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_an_invalid_account()
        {
            actor = new Actor()
            {
                Account = new AgentAccount()
                {
                    HomePage = "notAProperAbsoluteIRI",
                    Name = "AllowableName"
                }
            };
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }
    }
}
