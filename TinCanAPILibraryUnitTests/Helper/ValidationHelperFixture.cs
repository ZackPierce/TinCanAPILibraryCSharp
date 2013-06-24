using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace TinCanAPILibraryUnitTests.Helper
{
    [TestFixture]
    public class ValidationHelperFixture
    {
        [Test]
        public void IsValidEmailAddress_returns_true_for_easy_correct_ascii_addresses()
        {
            Assert.True(ValidationHelper.IsValidEmailAddress("david.jones@proseware.com"));
            Assert.True(ValidationHelper.IsValidEmailAddress("d.j@server1.proseware.com"));
            Assert.True(ValidationHelper.IsValidEmailAddress("jones@ms1.proseware.com"));
            Assert.True(ValidationHelper.IsValidEmailAddress("js#internal@proseware.com"));
            Assert.True(ValidationHelper.IsValidEmailAddress("j_9@[129.126.118.1]"));
            Assert.True(ValidationHelper.IsValidEmailAddress("j.s@server1.proseware.com"));
        }

        [Test]
        public void IsValidEmailAddress_returns_false_for_easy_incorrect_ascii_addresses()
        {
            Assert.False(ValidationHelper.IsValidEmailAddress("j.@server1.proseware.com"));
            Assert.False(ValidationHelper.IsValidEmailAddress("j..s@proseware.com"));
            Assert.False(ValidationHelper.IsValidEmailAddress("js@proseware..com"));
        }

        [Test]
        [Ignore("TODO - Improve email validation")]
        public void IsValidEmailAddress_returns_false_for_harder_incorrect_ascii_addresses()
        {
            Assert.False(ValidationHelper.IsValidEmailAddress("js@proseware.com9"));
            Assert.False(ValidationHelper.IsValidEmailAddress("j@proseware.com9"));
            Assert.False(ValidationHelper.IsValidEmailAddress("js*@proseware.com"));

        }

        [Test]
        public void IsValidMailtoIRI_returns_true_for_easy_correct_ascii_addresses()
        {
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:david.jones@proseware.com"));
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:d.j@server1.proseware.com"));
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:jones@ms1.proseware.com"));
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:js#internal@proseware.com"));
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:j_9@[129.126.118.1]"));
            Assert.True(ValidationHelper.IsValidMailtoIRI("mailto:j.s@server1.proseware.com"));
        }

        [Test]
        public void IsValidMailtoIRI_returns_false_for_null_input()
        {
            Assert.False(ValidationHelper.IsValidMailtoIRI(null));
        }

        [Test]
        public void IsValidMailtoIRI_returns_false_for_valid_email_address_without_mailto_prefix()
        {
            Assert.False(ValidationHelper.IsValidMailtoIRI("david.jones@proseware.com"));
        }

        [Test]
        public void IsValidMailtoIRI_returns_false_for_easy_incorrect_ascii_addresses()
        {
            Assert.False(ValidationHelper.IsValidMailtoIRI("mailto:j.@server1.proseware.com"));
            Assert.False(ValidationHelper.IsValidMailtoIRI("mailto:j..s@proseware.com"));
            Assert.False(ValidationHelper.IsValidMailtoIRI("mailto:js@proseware..com"));
        }

        [Test]
        public void IsValidAbsoluteIRI_returns_true_for_valid_IRI()
        {
            Assert.True(ValidationHelper.IsValidAbsoluteIri("http://example.com/exIRI"));
        }

        [Test]
        public void IsValidAbsoluteIRI_returns_false_for_null_input()
        {
            Assert.False(ValidationHelper.IsValidAbsoluteIri(null));
        }

        [Test]
        public void IsValidAbsoluteIRI_returns_false_for_empty_IRI()
        {
            Assert.False(ValidationHelper.IsValidAbsoluteIri(""));
        }

        [Test]
        public void IsValidAbsoluteIRI_returns_false_for_invalid_IRI()
        {
            Assert.False(ValidationHelper.IsValidAbsoluteIri("[]{strange non IRI stuff[][]{}}"));
        }

        [Test]
        public void IsValidAbsoluteIRI_returns_false_for_relative_IRI()
        {
            Assert.False(ValidationHelper.IsValidAbsoluteIri("somethingRelative"));
        }

        [Test]
        public void IsValidLanguageTag_returns_true_for_valid_tags()
        {
            Assert.True(ValidationHelper.IsValidLanguageTag("en-US"));
            Assert.True(ValidationHelper.IsValidLanguageTag("en"));
            Assert.True(ValidationHelper.IsValidLanguageTag("fr-CA"));
            Assert.True(ValidationHelper.IsValidLanguageTag("es-419"));
            Assert.True(ValidationHelper.IsValidLanguageTag("zh-Hans"));
        }

        [Test]
        public void IsValidLanguageTag_returns_false_for_bad_tags()
        {
            Assert.False(ValidationHelper.IsValidLanguageTag("not even close"));
            Assert.False(ValidationHelper.IsValidLanguageTag("en-NotACountry"));
        }

    }
}
