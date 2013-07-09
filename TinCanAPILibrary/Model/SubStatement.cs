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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SubStatement : PartialStatementBase, IStatementTarget
    {
        # region Properties
        public string ObjectType
        {
            get { return "SubStatement"; }
        }
        #endregion

        # region Constructors
        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public SubStatement() { }

        /// <summary>
        /// Creates a SubStatement with the minimum suggested properties
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The verb in this statement</param>
        /// <param name="statementTarget">The target of this statement</param>
        public SubStatement(Actor actor, StatementVerb verb, IStatementTarget statementTarget) : base(actor, verb, statementTarget) 
        { }

        /// <summary>
        /// Creates a Sub-Statement with a verb from the predefined verb enumeration.
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The PredefinedVerb of this statement</param>
        /// <param name="statementTarget">The target statement</param>
        public SubStatement(Actor actor, PredefinedVerb verb, IStatementTarget statementTarget)
            : base(actor, verb, statementTarget)
        {
        }
        #endregion
        #endregion

        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = base.validateImplementation(earlyReturnOnFailure);
            if (this.Object is SubStatement)
            {
                failures.Add(new ValidationFailure("A Sub-Statement must not contain a Sub-Statement of its own in the object property.", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            return failures;
        }

        
    }
}
