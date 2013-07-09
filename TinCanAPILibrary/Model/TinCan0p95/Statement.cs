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
    /// Tin Can 0.95 version of Statement
    /// </summary>
    public class Statement : StatementBase, IValidatable
    {
        #region Fields
        private NullableBoolean voided;
        private Context context;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public NullableBoolean Voided
        {
            get { return voided; }
            set { voided = value; }
        }

        /// <summary>
        /// TinCan 0.95 Context information for this statement
        /// </summary>
        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        #region Public Methods
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = base.validateImplementation(earlyReturnOnFailure);
            if (earlyReturnOnFailure && failures.Count > 0)
            {
                return failures;
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, context, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }
            return failures;
        }

        #endregion

        #region Legacy
        /// <summary>
        /// Handles verbs with special requirements
        /// TODO - Legacy Cleanup - find out where the requirements are listed and what versions they are relevant under.
        /// </summary>
        public void HandleSpecialVerbs()
        {
            if (this.Verb == null)
            {
                return;
            }
            if (this.Verb.Equals("passed"))
            {
                Result = (Result == null) ? new Result() : Result;
                VerifySuccessAndCompletionValues(Result, "passed", true, true);
                Result.Success = true;
                Result.Completion = true;
            }
            else if (this.Verb.Equals("failed"))
            {
                Result = (Result == null) ? new Result() : Result;
                VerifySuccessAndCompletionValues(Result, "failed", false, true);
                Result.Success = false;
                Result.Completion = true;
            }
            else if (this.Verb.Equals("completed"))
            {
                Result = (Result == null) ? new Result() : Result;
                VerifyCompletionValue(Result, "completed", true);
                Result.Completion = true;
            }
        }

        #endregion

        #region TinCan 0.90 Downgrade
        /// <summary>
        /// Demotes a TinCan 0.95 Statement to TinCan 0.9
        /// </summary>
        /// <param name="source">A TinCan 0.95 Statement</param>
        /// <returns>The TinCan 0.90 representation of the statement</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Statement(Statement source)
        {
            if (source == null)
            {
                return null;
            }
            var result = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Statement();
            result.Id = source.Id;
            result.Actor = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Actor)source.Actor;
            result.Verb = ((PredefinedVerb)source.Verb);
            result.InProgress = false;
            result.Object = source.Object;
            result.Result = source.Result;
            result.Context = source.Context;
            result.Timestamp = source.Timestamp;
            if (source.Authority != null)
            {
                result.Authority = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Actor)source.Authority;
            }
            result.Voided = source.Voided;

            return result;
        }
        #endregion

        #region TinCan 1.0.0 upgrade
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.Statement(Statement source)
        {
            if (source == null)
            {
                return null;
            }
            var upgrade = new RusticiSoftware.TinCanAPILibrary.Model.Statement();
            upgrade.Actor = source.Actor;
            upgrade.Authority = source.Authority;
            upgrade.Context = (RusticiSoftware.TinCanAPILibrary.Model.Context)source.Context;
            upgrade.Id = source.Id;
            upgrade.Object = source.Object;
            upgrade.Result = source.Result;
            upgrade.Stored = source.Stored;
            upgrade.Timestamp = source.Timestamp;
            upgrade.Verb = source.Verb;
            return upgrade;
        }
        #endregion
    }
}
