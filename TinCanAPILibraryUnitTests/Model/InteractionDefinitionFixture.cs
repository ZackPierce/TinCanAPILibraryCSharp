using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class InteractionDefinitionFixture
    {

        [Test]
        public void Validate_passes_valid_InteractionDefinition()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice"
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failrue_for_a_null_interactionType()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = null
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_failure_for_an_undefined_interactionType()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "undefined"
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Must, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_should_failure_for_a_non_cmi_type()
        {
            var definition = new InteractionDefinition()
            {
                Name = new LanguageMap()
                {
                    {"en-US", "hello"}
                },
                Description = new LanguageMap()
                {
                    {"jp", "osu"}
                },
                Type = new Uri("http://example.com/Not_the_standard_cmi_type"),
                MoreInfo = new Uri("http://example.com/definitionTypes/defTypeX"),
                InteractionType = "choice"
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.Should, failures[0].Level);
        }

        [Test]
        public void Validate_produces_a_may_failure_for_a_null_correctResponsesPattern_member()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice",
                CorrectResponsesPattern = new List<string>()
            };
            definition.CorrectResponsesPattern.Add(null);

            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }

        [Test]
        public void Validate_allows_valid_component_array()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice",
                Choices = new List<InteractionComponent>()
                {
                    new InteractionComponent()
                    {
                        Id = "componentA",
                        Description = new LanguageMap()
                    }
                }
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_May_failure_for_component_collection_with_wrong_interactionType()
        {
            var definition = new InteractionDefinition()
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
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }

        [Test]
        public void Validate_produces_May_failure_for_component_collection_with_null_member()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice",
                Choices = new List<InteractionComponent>()
                {
                    null
                }
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }

        [Test]
        public void Validate_produces_May_failure_for_component_collection_with_invalid_member()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice",
                Choices = new List<InteractionComponent>()
                {
                    new InteractionComponent()
                    {
                        Id = "look some whitespace",
                        Description = new LanguageMap()
                    }
                }
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }

        [Test]
        public void Validate_produces_May_failure_for_component_collection_with_redundant_ids()
        {
            var definition = new InteractionDefinition()
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
                InteractionType = "choice",
                Choices = new List<InteractionComponent>()
                {
                    new InteractionComponent()
                    {
                        Id = "validIdA",
                        Description = new LanguageMap()
                    },
                    new InteractionComponent()
                    {
                        Id = "validIdA",
                        Description = new LanguageMap()
                    }
                }
            };
            var rawResults = definition.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }
    }
}
