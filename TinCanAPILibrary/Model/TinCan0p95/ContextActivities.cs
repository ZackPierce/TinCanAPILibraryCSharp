#region License
/*
Copyright 2012 Rustici Software, Measured Progress

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

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RusticiSoftware.TinCanAPILibrary.Model;
    using RusticiSoftware.TinCanAPILibrary.Helper;

    /// <summary>
    /// TinCan 0.95 ContextActivities
    /// </summary>
    public class ContextActivities : IValidatable
    {
        #region Fields
        private Activity parent;
        private Activity grouping;
        private Activity other;
        #endregion

        #region Properties
        public Activity Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Activity Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

        public Activity Other
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
            foreach (IValidatable validatableKid in new IValidatable[] { parent, grouping, other })
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, validatableKid, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            return failures;
        }

        #region Version Conversion
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.ContextActivities(ContextActivities source)
        {
            if (source == null)
            {
                return null;
            }
            var sink = new RusticiSoftware.TinCanAPILibrary.Model.ContextActivities();
            if (source.grouping != null)
            {
                sink.Grouping = new Activity[] { source.grouping };
            }
            if (source.Other != null)
            {
                sink.Other = new Activity[] { source.Other };
            }
            if (source.parent != null)
            {
                sink.Parent = new Activity[] { source.Parent };
            }
            return sink;
        }
        #endregion
    }
}
