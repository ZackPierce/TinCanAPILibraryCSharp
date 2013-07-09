using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class ResultFixture
    {

        [Test]
        public void Validate_passes_empty_Result()
        {
            var result = new Result();
            var failures = new List<ValidationFailure>(result.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_reports_failures_within_extensions()
        {
            var result = new Result();
            result.Extensions = new Dictionary<Uri, object>
            {
                {new Uri("relative", UriKind.Relative), "something"}
            };
            var failures = new List<ValidationFailure>(result.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failures_within_score_property()
        {
            var result = new Result();
            result.Score = new Score()
            {
                Min = 2,
                Max = 1
            };
            var failures = new List<ValidationFailure>(result.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_reports_failures_for_CreateInvalidResult_result()
        {
            var result = CreateInvalidResult();
            var failures = new List<ValidationFailure>(result.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        internal static Result CreateInvalidResult()
        {
            return new Result()
            {
                Score = new Score()
                {
                    Min = 2,
                    Raw = 1
                }
            };
        }
    }
}
