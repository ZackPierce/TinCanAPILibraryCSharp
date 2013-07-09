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
    using RusticiSoftware.TinCanAPILibrary.Helper;

    /// <summary>
    /// TinCan 0.95 Context
    /// </summary>
    public class Context : ContextBase, IValidatable
    {
        #region Fields
        private Statement statement;
        private ContextActivities contextActivities;
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
        /// TODO - better representation as 0.95 StatementRef or 0.95 SubStatement rather than a full 0.95 Statement
        /// </summary>
        public Statement Statement
        {
            get { return statement; }
            set { statement = value; }
        }
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

            if (ValidationHelper.ValidateAndAddFailures(failures, contextActivities, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (statement != null)
            {
                if (statement.Stored != null || statement.Authority != null || statement.Voided != null)
                {
                    failures.Add(new ValidationFailure("Context had a statement property value assumed to be a SubStatement, but its Stored, Authority, or Voided properties were non-null.", ValidationLevel.Must));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
            }
            return failures;
        }
        #endregion

        #region Version Conversion
        /// <summary>
        /// Upgrades a TinCan 0.95 context to a 1.0.0 context.
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.Context(Context source)
        {
            if (source == null)
            {
                return null;
            }
            var upgrade = new RusticiSoftware.TinCanAPILibrary.Model.Context();
            ContextBase.TransferProperties(source, upgrade);
            upgrade.ContextActivities = (RusticiSoftware.TinCanAPILibrary.Model.ContextActivities)source.ContextActivities;
            return upgrade;
        }

        #endregion
    }
}
