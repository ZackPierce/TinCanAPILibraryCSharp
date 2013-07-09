using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class AttachmentFixture
    {

        [Test]
        public void Validate_produces_no_failures_for_correct_attachment()
        {
            var attachment = CreateValidAttachment();
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_length()
        {
            var attachment = CreateValidAttachment();
            attachment.Length = null;
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_usageType()
        {
            var attachment = CreateValidAttachment();
            attachment.UsageType = null;
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_relative_usageType()
        {
            var attachment = CreateValidAttachment();
            attachment.UsageType = new Uri("whatever", UriKind.Relative);
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_display()
        {
            var attachment = CreateValidAttachment();
            attachment.Display = null;
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_contentType()
        {
            var attachment = CreateValidAttachment();
            attachment.ContentType = null;
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test, Ignore]
        public void Validate_produces_a_failure_for_a_non_InternetMediaType_contentType()
        {
            var attachment = CreateValidAttachment();
            attachment.ContentType = "123NotAMediaType";
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_null_sha2()
        {
            var attachment = CreateValidAttachment();
            attachment.Sha2 = null;
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_a_non_Base64_sha2()
        {
            var attachment = CreateValidAttachment();
            attachment.Sha2 = "#%^not permitted characters abound ()[]";
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidAttachment_output()
        {
            var attachment = CreateInvalidAttachment();
            var failures = new List<ValidationFailure>(attachment.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        internal static Attachment CreateValidAttachment()
        {
            return new Attachment()
            {
                UsageType = new Uri("http://adl.example.com/usageType/A", UriKind.Absolute),
                Display = new LanguageMap(),
                ContentType = "text/plain",
                Length = new NullableInteger(11),
                Sha2 = "uU0nuZNNPgilLlLX2n2r+sSE7+N6U4DukIj3rOLvzek="
            };
        }

        internal static Attachment CreateInvalidAttachment()
        {
            return new Attachment()
            {
                UsageType = null,
                Display = new LanguageMap(),
                ContentType = "text/plain",
                Length = new NullableInteger(11),
                Sha2 = "uU0nuZNNPgilLlLX2n2r+sSE7+N6U4DukIj3rOLvzek="
            };
        }
    }
}
