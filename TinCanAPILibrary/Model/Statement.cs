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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RusticiSoftware.TinCanAPILibrary.Helper;

    /// <summary>
    /// Model corresponding to a 1.0.x Experience API Statement
    /// </summary>
    public class Statement : StatementBase, IValidatable
    {
        private string version;
        private Context context;
        private Attachment[] attachments;

        /// <summary>
        /// Either 0.9, 0.90, 0.95, or a Semantic Versioning 1.0.0-compliant value corresponding to
        /// the xAPI version of the statement, greater than or equal to 1.0.0.
        /// Per xAPI 1.0.x, Client applications should not be setting the Statement version,
        /// but must set it to "1.0.0" if they do.
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// Context information for this statement
        /// </summary>
        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Headers for attachments to the Statement
        /// </summary>
        public Attachment[] Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        public Statement()
        { }

        public Statement(Actor actor, StatementVerb verb, IStatementTarget statementTarget) :
            base(actor, verb, statementTarget)
        { }

        public Statement(Actor actor, PredefinedVerb verb, IStatementTarget statementTarget) :
            base(actor, verb, statementTarget)
        { }

        #region Public Methods
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = base.validateImplementation(earlyReturnOnFailure);

            if (version != null && !(version == "0.9" || version == "0.90" || version == "0.95" || ValidationHelper.IsValidSemanticVersion1p0p0(version)))
            {
                failures.Add(new ValidationFailure(string.Format("If present, the Statement's version property must either conform to SemVer 1.0.0, or be a legacy value of '0.9', '0.90', or '0.95', but instead was found to be {0}", version), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, attachments, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (context != null)
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, context, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }

                if (Object is Actor || Object is Group)
                {
                    if (context.Revision != null)
                    {
                        failures.Add(new ValidationFailure("The context.revision property MUST NOT be used if the Statement's Object is an Agent or Group.", ValidationLevel.Must));
                        if (earlyReturnOnFailure)
                        {
                            return failures;
                        }
                    }
                    if (context.Platform != null)
                    {
                        failures.Add(new ValidationFailure("The context.platform property MUST NOT be used if the Statement's Object is an Agent or Group.", ValidationLevel.Must));
                        if (earlyReturnOnFailure)
                        {
                            return failures;
                        }
                    }
                }
            }
            return failures;
        }
        #endregion

        #region TinCan previous versions downgrade
        /// <summary>
        /// Demotes a TinCan 1.0.0 Statement to TinCan 0.9
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source">A TinCan 1.0.0 Statement</param>
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
            result.Context = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context)source.Context;
            result.Timestamp = source.Timestamp;
            if (source.Authority != null)
            {
                result.Authority = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Actor)source.Authority;
            }
            result.Voided = false; // 1.0.x has no explicit property for "Voided"

            return result;
        }

        /// <summary>
        /// Demotes a TinCan 1.0.0 Statement to TinCan 0.95
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source">A TinCan 1.0.0 Statement</param>
        /// <returns>The TinCan 0.95 representation of the statement</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement(Statement source)
        {
            if (source == null)
            {
                return null;
            }
            var downgrade = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement();
            downgrade.Actor = source.Actor;
            downgrade.Authority = source.Authority;
            downgrade.Context = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context)source.Context;
            downgrade.Id = source.Id;
            downgrade.Object = source.Object;
            downgrade.Result = source.Result;
            downgrade.Stored = source.Stored;
            downgrade.Timestamp = source.Timestamp;
            downgrade.Verb = source.Verb;
            downgrade.Voided = false; // 1.0.x has no explicit property for "Voided"
            return downgrade;
        }
        #endregion
    }
}
