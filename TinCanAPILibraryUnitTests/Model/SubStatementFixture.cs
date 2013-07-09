using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class SubStatementFixture
    {

        [Test]
        public void Validate_passes_valid_substatement()
        {
            var subStatement = CreateSimpleValidSubStatement();
            var failures = subStatement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(0, new List<ValidationFailure>(failures).Count);
        }

        [Test]
        public void Validate_produces_error_for_nested_substatement()
        {
            var subStatement = CreateSimpleValidSubStatement();
            subStatement.Object = CreateSimpleValidSubStatement();
            var failures = subStatement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(1, new List<ValidationFailure>(failures).Count);
        }

        private static SubStatement CreateSimpleValidSubStatement()
        {
            var activity = new Activity("http://www.example.com");
            activity.Definition = new ActivityDefinition();
            activity.Definition.Type = new Uri("http://example.com/types/activityTypeA");
            activity.Definition.Name = new LanguageMap();
            activity.Definition.Name.Add("en-US", "TCAPI C# 1.0.0 Library.");
            return new SubStatement(
                    new Actor("Example", "mailto:test@example.com"),
                    new StatementVerb("http://example.com/doSomething", "en-US", "Do something"),
                    activity
                );
        }
    }
}
