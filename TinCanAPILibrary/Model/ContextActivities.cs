#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
using System;
using System.Collections.Generic;

using System.Text;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// TinCan 1.0.0 ContextActivities
    /// TODO - handle special case deserialization where the properties may be single Activity objects rather than arrays.
    /// </summary>
    public class ContextActivities : IValidatable
    {
        #region Fields
        private Activity[] parent;
        private Activity[] grouping;
        private Activity[] category;
        private Activity[] other;
        #endregion

        private const string conversionDataLossWarningTemplate = "Could not convert RusticiSoftware.TinCanAPILibrary.Model.ContextActivities to RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.ContextActivities without losing data from the {0} property, which had {1} members";

        #region Properties
        public Activity[] Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Activity[] Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

        public Activity[] Category
        {
            get { return category; }
            set { category = value; }
        }

        public Activity[] Other
        {
            get { return other; }
            set { other = value; }
        }
        #endregion

        #region Constructor
        public ContextActivities() { }
        #endregion

        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            foreach (IValidatable[] validatableKidSet in new IValidatable[][] { parent, grouping, category, other })
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, validatableKidSet, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            return failures;
        }

        #region Version Conversion
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.ContextActivities(ContextActivities source)
        {
            if (source == null)
            {
                return null;
            }
            var sink = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.ContextActivities();
            if (source.grouping != null && source.grouping.Length > 0)
            {
                sink.Grouping = source.grouping[0];
                if (source.grouping.Length > 1)
                {
                    throw new ArgumentException(string.Format(conversionDataLossWarningTemplate, "grouping", source.grouping.Length));
                }
            }
            if (source.other != null && source.other.Length > 0)
            {
                sink.Other = source.Other[0];
                if (source.Other.Length > 1)
                {
                    throw new ArgumentException(string.Format(conversionDataLossWarningTemplate, "other", source.Other.Length));
                }
            }
            if (source.parent != null && source.parent.Length > 0)
            {
                sink.Parent = source.parent[0];
                if (source.Parent.Length > 1)
                {
                    throw new ArgumentException(string.Format(conversionDataLossWarningTemplate, "parent", source.parent.Length));
                }
            }
            if (source.category != null && source.category.Length > 0)
            {
                throw new ArgumentException(string.Format(conversionDataLossWarningTemplate, "category", source.grouping.Length));
            }
            return sink;
        }
        #endregion
    }
}
