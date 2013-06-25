using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class ActivityDefinitionFixture
    {

        [Test]
        public void Validate_produces_no_failures_for_a_correct_ActivityDefinition()
        {
            var definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = new Uri("http://example.com/definitionTypes/defTypeX"),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX")
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_BestPractice_failure_for_a_missing_type_property()
        {
            var definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = null,
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX")
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.BestPractice, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_Must_failure_for_a_non_absolute_IRI_type_property()
        {
            var definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = new Uri("relativeURI", UriKind.Relative),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX")
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_an_invalid_name_property()
        {
            var definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"notAProperLanguageTag", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = new Uri("http://example.com/definitionTypes/defTypeX"),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX")
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_an_invalid_description_property()
        {
            var definition = new ActivityDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"notAProperLanguageTag", "osu"}
                },
                Type = new Uri("http://example.com/definitionTypes/defTypeX"),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX")
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }
    }
}
