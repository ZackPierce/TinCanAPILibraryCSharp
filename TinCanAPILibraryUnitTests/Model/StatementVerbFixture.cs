using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class StatementVerbFixture
    {

        [Test]
        public void Validate_produces_no_failure_for_a_valid_verb()
        {
            var verb = CreateValidVerb();
            var failures = verb.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 0);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_id()
        {
            var verb = new StatementVerb("http://example.com/doSomething", "en-US", "Do something");
            verb.Id = null;
            var failures = verb.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 1);
        }

        [Test]
        public void Validate_produces_a_should_failure_for_a_null_display()
        {
            var verb = new StatementVerb("http://example.com/doSomething", "en-US", "Do something");
            verb.Display = null;
            var failures = new List<ValidationFailure>(verb.Validate(earlyReturnOnFailure: false));
            Assert.NotNull(failures);
            Assert.AreEqual(failures.Count, 1);
            Assert.AreEqual(ValidationLevel.Should, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_must_failure_for_an_invalid_display()
        {
            var verb = new StatementVerb("http://example.com/doSomething", "en-US", "Do something");
            verb.Display = new LanguageMap()
                {
                    {"notAValidLanguageTag", "hello"}
                };
            var failures = new List<ValidationFailure>(verb.Validate(earlyReturnOnFailure: false));
            Assert.NotNull(failures);
            Assert.AreEqual(failures.Count, 1);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidVerb_output()
        {
            var verb = CreateInvalidVerb();
            var failures = new List<ValidationFailure>(verb.Validate(earlyReturnOnFailure: false));
            Assert.NotNull(failures);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        internal static StatementVerb CreateValidVerb()
        {
            return new StatementVerb("http://example.com/doSomething", "en-US", "Do something");
        }

        internal static StatementVerb CreateInvalidVerb()
        {
            return new StatementVerb()
                {
                    Display = new LanguageMap()
                };
        }
    }
}
