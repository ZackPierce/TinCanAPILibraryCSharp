using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class LanguageMapFixture
    {

        [Test]
        public void Validate_produces_no_errors_for_empty()
        {
            var languageMap = new LanguageMap();
            var failures = languageMap.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 0);
        }

        [Test]
        public void Validate_produces_no_errors_for_simple_valid_entry()
        {
            var languageMap = new LanguageMap()
                {
                    {"en-US" , "hello"}
                };
            var failures = languageMap.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 0);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_value_entry()
        {
            var languageMap = new LanguageMap()
                {
                    {"en-US" , null}
                };
            var failures = languageMap.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 1);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_non_language_tag_key()
        {
            var languageMap = new LanguageMap()
                {
                    {"notALanguageTag" , "hello"}
                };
            var failures = languageMap.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 1);
        }
    }
}
