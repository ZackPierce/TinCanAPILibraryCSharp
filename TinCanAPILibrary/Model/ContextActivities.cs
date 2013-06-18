﻿#region License
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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ContextActivities : IValidatable
    {
        #region Fields
        private TinCanActivity parent;
        private TinCanActivity grouping;
        private TinCanActivity other;
        #endregion

        #region Properties
        public TinCanActivity Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public TinCanActivity Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

        public TinCanActivity Other
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
            //Validate children
            object[] children = new object[] { parent, grouping, other };
            var failures = new List<ValidationFailure>();
            foreach (object child in children)
            {
                if (child != null && child is IValidatable)
                {
                    failures.AddRange(((IValidatable)child).Validate(earlyReturnOnFailure));
                    if (earlyReturnOnFailure && failures.Count > 0)
                    {
                        return failures;
                    }
                }
            }
            return failures;
        }
    }
}
