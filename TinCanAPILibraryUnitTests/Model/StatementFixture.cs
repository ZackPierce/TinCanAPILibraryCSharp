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
        [Test]
        public void Empty_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement());
        }

        [Test]
        public void Parameterized_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), new StatementVerb(), new StatementRef("hardcodedid")));
        }

        [Test]
        public void Predefined_verb_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), PredefinedVerb.Completed, new StatementRef("hardcodedid")));
        }

        [Test]
        public void Id_setter_does_not_throw_exception_for_non_uuid_string()
        {
            var statement = new Statement();
            Assert.DoesNotThrow(() => statement.Id = "Not a proper UUID");
        }

        [Test]
        public void Validate_returns_non_null_enumerable_with_failure_results_when_invalid()
        {
            var statement = new Statement();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.GreaterOrEqual(new List<ValidationFailure>(failures).Count, 1, "Expect several errors due to lack of supplied statement information");
        }

        [Test]
        public void Validate_returns_non_null_empty_enumerable_when_valid()
        {
            var statement = CreateSimpleValidStatement();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_failure_for_null_id()
        {
            var statement = CreateSimpleValidStatement();
            statement.Id = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_failure_for_non_null_non_UUID_id()
        {
            var statement = CreateSimpleValidStatement();
            statement.Id = "Not a proper UUID";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_allows_upper_case_UUID_id()
        {
            var statement = CreateSimpleValidStatement();
            statement.Id = "E1EEC41F-1E93-4ED6-ACBF-5C4BD0C24269";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failure_for_valid_SemVer_version()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "1.0.0";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failure_for_legacy_0p9_version()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "0.9";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failure_for_legacy_0p90_version()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "0.90";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failure_for_legacy_0p95_version()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "0.95";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_non_SemVer_version()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "Completely Wrong";
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_version_using_features_not_in_SemVer_1p0p0()
        {
            var statement = CreateSimpleValidStatement();
            statement.Version = "1.0.0-alpha.1"; // Allowed in SemVer 2.0.0
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_actor_is_null()
        {
            var statement = CreateSimpleValidStatement();
            statement.Actor = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_actor_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Actor = ActorFixture.CreateInvalidAgent();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_verb_is_null()
        {
            var statement = CreateSimpleValidStatement();
            statement.Verb = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_verb_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Verb = StatementVerbFixture.CreateInvalidVerb();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_object_is_null()
        {
            var statement = CreateSimpleValidStatement();
            statement.Object = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_object_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Object = ActivityFixture.CreateInvalidActivity();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failures_when_result_is_null_because_that_property_is_optional()
        {
            var statement = CreateSimpleValidStatement();
            statement.Result = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_result_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Result = ResultFixture.CreateInvalidResult();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failures_when_context_is_null_because_that_property_is_optional()
        {
            var statement = CreateSimpleValidStatement();
            statement.Context = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_context_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Context = ContextFixture.CreateInvalidContext();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failures_when_authority_is_null_because_that_property_is_optional()
        {
            var statement = CreateSimpleValidStatement();
            statement.Authority = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_authority_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Authority = ActorFixture.CreateInvalidAgent();
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_authority_is_a_group_with_other_than_two_members()
        {
            var statement = CreateSimpleValidStatement();
            statement.Authority = new Group()
                {
                    Mbox = "mailto:someGroup@example.com"
                };
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failures_when_attachments_is_null_because_that_property_is_optional()
        {
            var statement = CreateSimpleValidStatement();
            statement.Attachments = null;
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_no_failures_when_attachments_is_empty_because_that_property_is_optional()
        {
            var statement = CreateSimpleValidStatement();
            statement.Attachments = new Attachment[] { };
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_a_failure_when_attachments_member_has_invalid_data()
        {
            var statement = CreateSimpleValidStatement();
            statement.Attachments = new Attachment[] { AttachmentFixture.CreateInvalidAttachment() };
            var failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }


        private static Statement CreateSimpleValidStatement()
        {
            var activity = new Activity("http://www.example.com");
            activity.Definition = new ActivityDefinition();
            activity.Definition.Type = new Uri("http://example.com/types/activityTypeA");
            activity.Definition.Name = new LanguageMap();
            activity.Definition.Name.Add("en-US", "TCAPI C# 1.0.0 Library.");
            return new Statement(
                    ActorFixture.CreateValidAgent(),
                    new StatementVerb("http://example.com/doSomething", "en-US", "Do something"),
                    activity
                )
                {
                    Id = "e1eec41f-1e93-4ed6-acbf-5c4bd0c24269"
                };
        }
    }
}
