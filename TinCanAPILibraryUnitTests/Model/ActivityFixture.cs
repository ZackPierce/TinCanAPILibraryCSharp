using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class ActivityFixture
    {

        [Test]
        public void Validate_produces_no_failure_for_valid_activity()
        {
            var activity = CreateValidActivity();
            var rawResults = activity.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_null_id()
        {
            var activity = new Activity();
            activity.Id = null;
            var rawResults = activity.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_non_IRI_id()
        {
            var activity = new Activity();
            activity.Id = "[]{} not an IRI";
            var rawResults = activity.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_relative_IRI_id()
        {
            var activity = new Activity();
            activity.Id = "relativeIRI";
            var rawResults = activity.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_invalid_non_null_definition()
        {
            var activity = new Activity("http://example.com/activities/A");
            activity.Definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"not a valid language tag", "should bubble up"}
                },
                Type = new Uri("http://adl.example.com/activityType/A/B", UriKind.Absolute)
            };
            var rawResults = activity.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        internal static Activity CreateValidActivity()
        {
            return new Activity()
            {
                Id = "http://example.com/activities/A",
                Definition = new ActivityDefinition()
                {
                    Name = new LanguageMap()
                    {
                        {"en-US", "Name A"}
                    },
                    Type = new Uri("http://adl.example.com/activityType/A/B", UriKind.Absolute)
                }
            };
        }

        internal static Activity CreateInvalidActivity()
        {
            return new Activity()
            {
                Id = null
            };
        }
    }
}
