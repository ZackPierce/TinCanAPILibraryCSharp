using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class StatementFixture
    {

        private Statement statement;

        [TearDown]
        public void TearDown()
        {
            statement = null;
        }

        [Test]
        public void Empty_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement());
        }

        [Test]
        public void Parameterized_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), new StatementVerb(), new StatementTarget()));
        }

        [Test]
        public void Predefined_verb_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), PredefinedVerbs.Completed, new StatementTarget()));
        }

        [Test]
        public void Id_setter_does_not_throw_exception_for_non_uuid_string()
        {
            statement = new Statement();
            Assert.DoesNotThrow(() => statement.Id = "Not a proper UUID");
        }

        [Test]
        public void Validate_returns_non_null_enumerable_with_failure_results_when_invalid()
        {
            statement = new Statement();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.GreaterOrEqual(new List<ValidationFailure>(failures).Count, 1, "Expect several errors due to lack of supplied statement information");
        }

        [Test]
        public void Validate_returns_non_null_empty_enumerable_when_valid()
        {
            statement = CreateSimpleValidStatement();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_failure_for_null_id()
        {
            statement = CreateSimpleValidStatement();
            statement.Id = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_failure_for_non_null_non_UUID_id()
        {
            statement = CreateSimpleValidStatement();
            statement.Id = "Not a proper UUID";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_allows_upper_case_UUID_id()
        {
            statement = CreateSimpleValidStatement();
            statement.Id = "E1EEC41F-1E93-4ED6-ACBF-5C4BD0C24269";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        private static Statement CreateSimpleValidStatement()
        {
            var activity = new Activity("http://www.example.com");
            activity.Definition = new ActivityDefinition();
            activity.Definition.Type = new Uri("http://example.com/types/activityTypeA");
            activity.Definition.Name = new LanguageMap();
            activity.Definition.Name.Add("en-US", "TCAPI C# 1.0.0 Library.");
            return new Statement(
                    new Actor("Example", "mailto:test@example.com"),
                    new StatementVerb("http://example.com/doSomething", "en-US", "Do something"),
                    activity
                )
                {
                    Id = "e1eec41f-1e93-4ed6-acbf-5c4bd0c24269"
                };
        }
    }
}
