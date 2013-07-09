using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class InteractionFixture
    {
        [Test]
        public void Validate_produces_no_failure_for_valid_Interaction()
        {
            var interaction = new Interaction("http://example.com/activities/A");
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_null_id()
        {
            var interaction = new Interaction();
            interaction.Id = null;
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_non_IRI_id()
        {
            var interaction = new Interaction();
            interaction.Id = "[]{} not an IRI";
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_relative_IRI_id()
        {
            var interaction = new Interaction();
            interaction.Id = "relativeIRI";
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_invalid_non_null_definition()
        {
            var interaction = new Interaction("http://example.com/activities/A");
            interaction.Definition = new InteractionDefinition()
            {
                Name = new LanguageMap()
                {
                    {"not a valid language tag", "should bubble up"}
                }
            };
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_May_failure_for_definition_with_component_collection_with_wrong_interactionType()
        {
            var interaction = new Interaction("http://example.com/activities/A");
            interaction.Definition = new InteractionDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = new Uri(Constants.CmiInteractionActivityType),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX"),
                InteractionType = "likert",
                Choices = new List<InteractionComponent>()
                {
                    new InteractionComponent()
                    {
                        Id = "componentA",
                        Description = new LanguageMap()
                    }
                }
            };
            var rawResults = interaction.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }
    }
}
