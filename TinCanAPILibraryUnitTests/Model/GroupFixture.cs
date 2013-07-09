using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class GroupFixture
    {
        [Test]
        public void Groups_display_an_ObjectType_of_Group()
        {
            Assert.AreEqual("Group", new Group().ObjectType);
        }

        [Test]
        public void Validate_produces_no_failures_for_a_valid_identified_Group_with_members()
        {
            var group = new Group()
            {
                Mbox = "mailto:group@example.com",
                Member = new Actor[] 
                { 
                    new Actor() 
                    {
                        Mbox = "mailto:memberA@example.com"
                    }
                }
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_no_failures_for_a_valid_identified_Group_with_no_members()
        {
            var group = new Group()
            {
                Mbox = "mailto:group@example.com",
                Member = new Actor[] { }
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_no_failures_for_a_valid_anonymous_Group()
        {
            var group = new Group()
            {
                Member = new Actor[] 
                { 
                    new Actor() 
                    {
                        Mbox = "mailto:memberA@example.com"
                    }
                }
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_an_anonymous_with_no_members_items()
        {
            var group = new Group()
            {
                Member = new Actor[] { }
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: true));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_an_anonymous_with_no_members_array()
        {
            var group = new Group()
            {
                Member = null
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(1, failures.Count);
        }

        [Test]
        public void Validate_produces_no_failures_for_an_identified_with_no_members_array()
        {
            var group = new Group()
            {
                Mbox = "mailto:someGroup@example.com",
                Member = null
            };
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(0, failures.Count);
        }

        [Test]
        public void Validate_produces_a_failure_for_CreateInvalidGroup_output()
        {
            var group = CreateInvalidGroup();
            var failures = new List<ValidationFailure>(group.Validate(earlyReturnOnFailure: false));
            Assert.AreEqual(1, failures.Count);
        }

        internal static Group CreateInvalidGroup()
        {
            return new Group()
            {
                Member = new Actor[] { }
            };
        }
    }
}
