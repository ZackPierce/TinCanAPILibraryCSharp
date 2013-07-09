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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RusticiSoftware.TinCanAPILibrary.Helper;

    /// <summary>
    /// Class represents the context of a TinCan Statement
    /// </summary>
    public class Context : ContextBase, IValidatable
    {
        #region Fields
        private ContextActivities contextActivities;
        private StatementRef statement;
        #endregion

        #region Properties


        /// <summary>
        /// The Activities in this Context
        /// </summary>
        public ContextActivities ContextActivities
        {
            get { return contextActivities; }
            set { contextActivities = value; }
        }

        /// <summary>
        /// Another Statement, which should be considered as context for this Statement.
        /// </summary>
        public StatementRef Statement
        {
            get { return statement; }
            set { statement = value; }
        }
        #endregion

        #region Constructor
        public Context() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates the context
        /// </summary>
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = base.validateImplementation(earlyReturnOnFailure);
            if (earlyReturnOnFailure && failures.Count > 0)
            {
                return failures;
            }

            foreach (var validatableKid in new IValidatable[] { contextActivities, statement })
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, validatableKid, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            return failures;
        }
        #endregion

        #region Version Conversion
        /// <summary>
        /// Downgrades a TinCan 1.0.0 context to a 0.95 context.
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context(Context source)
        {
            if (source == null)
            {
                return null;
            }
            var downgrade = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context();
            ContextBase.TransferProperties(source, downgrade);
            downgrade.ContextActivities = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.ContextActivities)source.ContextActivities;
            return downgrade;
        }

        #endregion
    }
}
