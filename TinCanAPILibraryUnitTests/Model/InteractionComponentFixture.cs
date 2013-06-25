using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class InteractionComponentFixture
    {
        [Test]
        public void Validate_produces_no_failure_for_valid_component()
        {
            var component = new InteractionComponent()
            {
                Id = "simpleIdRelativeUri",
                Description = new LanguageMap()
            };
            var rawResults = component.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_May_failure_for_id_with_whitespace()
        {
            var component = new InteractionComponent()
            {
                Id = "my id with whitespace",
                Description = new LanguageMap()
            };
            var rawResults = component.Validate(earlyReturnOnFailure: true);
            Assert.NotNull(rawResults);
            var failures = new List<ValidationFailure>(rawResults);
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(ValidationLevel.May, failures[0].Level);
        }
    }
}
