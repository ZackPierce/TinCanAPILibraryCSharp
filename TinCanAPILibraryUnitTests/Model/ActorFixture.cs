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
        [Test]
        public void Empty_constructor_does_not_throw_errors()
        {
            Assert.DoesNotThrow(() => new Actor());
        }

        [Test]
        public void Default_actor_ObjectType_is_Agent()
        {
            var actor = new Actor();
            Assert.AreEqual("Agent", actor.ObjectType);
        }

        [Test]
        public void Validate_returns_no_failures_for_simple_valid_Agent()
        {
            var actor = CreateValidAgent();
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_missing_inverse_functional_identifier()
        {
            var actor = new Actor();
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }


        [Test]
        public void Validate_returns_a_failure_for_invalid_mbox()
        {
            var actor = new Actor()
            {
                Mbox = "Not a in the valid mailto:address format"
            };
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_invalid_openid()
        {
            var actor = new Actor()
            {
                Openid = "[]{} not a valid IRI"
            };
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_returns_a_failure_for_an_invalid_account()
        {
            var actor = new Actor()
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

        [Test]
        public void Validate_returns_a_failure_for_CreateInvalidAgent_result()
        {
            var actor = CreateInvalidAgent();
            var failures = new List<ValidationFailure>(actor.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        internal static Actor CreateValidAgent()
        {
            return new Actor()
            {
                Mbox = "mailto:john@example.com"
            };
        }

        internal static Actor CreateInvalidAgent()
        {
            return new Actor()
            {
                Mbox = "NotAValidMailto"
            };
        }
    }
}
