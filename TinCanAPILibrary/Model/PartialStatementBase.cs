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
    /// Ancestor base class for statements and substatements.
    /// </summary>
    public abstract class PartialStatementBase
    {
        # region Fields
        private Actor actor;
        private StatementVerb verb;
        private IStatementTarget _object;
        private Result result;
        
        private NullableDateTime timestamp;
        #endregion

        #region Properties

        /// <summary>
        /// representation of the statement verb
        /// </summary>
        public virtual StatementVerb Verb
        {
            get
            {
                return verb;
            }
            set
            {
                verb = value;
            }
        }

        /// <summary>
        /// The statements actor
        /// </summary>
        public Actor Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// The target object of this statement
        /// </summary>
        public IStatementTarget Object
        {
            get { return _object; }
            set { _object = value; }
        }

        /// <summary>
        /// The result of this statement
        /// </summary>
        public Result Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// The timestamp of this statement
        /// </summary>
        public NullableDateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        #endregion

        #region Constructors

        public PartialStatementBase()
        { }

        /// <summary>
        /// Standard core components constructor
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="verb"></param>
        /// <param name="statementTarget"></param>
        public PartialStatementBase(Actor actor, StatementVerb verb, IStatementTarget statementTarget)
        {
            this.actor = actor;
            this.verb = verb;
            this._object = statementTarget;
        }

        /// <summary>
        /// Creates a statement with a verb from the predefined verb enumeration.
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The PredefinedVerb of this statement</param>
        /// <param name="statementTarget">The target statement</param>
        public PartialStatementBase(Actor actor, PredefinedVerb verb, IStatementTarget statementTarget)
            : this(actor, new StatementVerb(verb), statementTarget)
        {
        }
        #endregion

        #region Protected Methods

        protected virtual List<ValidationFailure> validateImplementation(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (ValidationHelper.MandatoryNonNullValidateAndAddFailures(failures, actor, earlyReturnOnFailure, "Statement.actor", ValidationLevel.Must) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (Verb == null)
            {
                failures.Add(new ValidationFailure("Statement does not have a verb", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (verb.IsVoided())
            {
                // This will test for StatementRef OR TargetedStatement because any statement that is being validated has already been promoted.
                bool objectStatementIdentified = ((_object is StatementRef) && !string.IsNullOrEmpty(((StatementRef)_object).Id) ||
                    (_object is Model.TinCan0p90.TargetedStatement) && !string.IsNullOrEmpty(((Model.TinCan0p90.TargetedStatement)_object).Id));
                if (!objectStatementIdentified)
                {
                    failures.Add(new ValidationFailure("Statement has verb 'voided' but does not properly identify a statement as its object", ValidationLevel.Must));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
                else if (ValidationHelper.ValidateAndAddFailures(failures, verb, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (ValidationHelper.ValidateAndAddFailures(failures, verb, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (ValidationHelper.MandatoryNonNullValidateAndAddFailures(failures, _object, earlyReturnOnFailure, "Statement.object", ValidationLevel.Must) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, result, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }
            return failures;
        }

        #endregion
    }
}
