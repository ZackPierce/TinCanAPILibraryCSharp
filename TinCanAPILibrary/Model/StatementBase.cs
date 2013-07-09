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
    /// Base class for Statements (0.95 and 1.0.x)
    /// </summary>
    public abstract class StatementBase : PartialStatementBase
    {
        #region Fields
        private string id;
        private NullableDateTime stored;
        private Actor authority;

        #endregion

        #region Properties

        /// <summary>
        /// The statements ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The time at which a Statement is stored by the LRS.
        /// </summary>
        public NullableDateTime Stored
        {
            get { return stored; }
            set { stored = value; }
        }

        /// <summary>
        /// The authority for this statement. Agent or Group who is asserting this Statement is true.
        /// </summary>
        public Actor Authority
        {
            get { return authority; }
            set { authority = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public StatementBase() { }

        /// <summary>
        /// Creates a Statement with the minimum suggested properties
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The verb in this statement</param>
        /// <param name="statementTarget">The target of this statement</param>
        public StatementBase(Actor actor, StatementVerb verb, IStatementTarget statementTarget) : base(actor, verb, statementTarget) 
        { }

        /// <summary>
        /// Creates a statement with a verb from the predefined verb enumeration.
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The PredefinedVerb of this statement</param>
        /// <param name="statementTarget">The target statement</param>
        public StatementBase(Actor actor, PredefinedVerb verb, IStatementTarget statementTarget)
            : base(actor, verb, statementTarget)
        {
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Validates the statement, ensuring required fields are used
        /// and any necessary information (such as a result for specific verbs)
        /// is provided and valid.
        /// </summary>
        protected override List<ValidationFailure> validateImplementation(bool earlyReturnOnFailure)
        {
            var failures = base.validateImplementation(earlyReturnOnFailure);
            if (earlyReturnOnFailure && failures.Count > 0)
            {
                return failures;
            }

            if (id == null)
            {
                failures.Add(new ValidationFailure(string.Format("Statement had a null id property. Id should be a UUID string.", id), ValidationLevel.Should));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (!ValidationHelper.IsValidUuid(id))
            {
                failures.Add(new ValidationFailure(string.Format("Statement had an id of {0}, but ids, when present, must UUIDs", id), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (authority != null)
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, authority, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
                var authorityGroup = authority as Group;
                if (authorityGroup != null)
                {
                    var numMembers = authorityGroup.Member == null ? 0 : authorityGroup.Member.Length;
                    if (numMembers != 2)
                    {
                        failures.Add(new ValidationFailure(string.Format("If the authority is a Group, it must contain two Agent members that represent the application and the user, but {0} members were found", numMembers), ValidationLevel.Must));
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

        #region Verb Handling

        /// <summary>
        /// Validates both success and completion
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The statement verb</param>
        /// <param name="expectedSuccess">The expected success value</param>
        /// <param name="expectedCompletion">The expected completion value</param>
        protected void VerifySuccessAndCompletionValues(Result result, string verb, bool expectedSuccess, bool expectedCompletion)
        {
            VerifySuccessValue(result, verb, expectedSuccess);
            VerifyCompletionValue(result, verb, expectedCompletion);
        }
        /// <summary>
        /// Validates expect success values
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The verb for the statement</param>
        /// <param name="expectedSuccess">What value the success should be</param>
        protected void VerifySuccessValue(Result result, string verb, bool expectedSuccess)
        {
            if (result.Success != null && result.Success.Value != expectedSuccess)
            {
                throw new ArgumentException("Specified verb \"" + verb + "\" but with a result success value of " + result.Success.Value, "verb");
            }
        }

        /// <summary>
        /// Validates expected completion values
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The statement verb</param>
        /// <param name="expectedCompletion">What value the completion should be</param>
        protected void VerifyCompletionValue(Result result, string verb, bool expectedCompletion)
        {
            if (result.Completion != null && result.Completion.Value != expectedCompletion)
            {
                throw new ArgumentException("Specified verb \"" + verb + "\" but with a result completion value of " + result.Completion.Value, "verb");
            }
        }
        #endregion


    }
}
