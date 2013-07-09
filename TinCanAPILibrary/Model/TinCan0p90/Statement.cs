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

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90
{
    /// <summary>
    /// TinCan 0.90 version of Statement
    /// </summary>
    public class Statement : IValidatable
    {
        #region Fields
        private string id;
        private Actor actor;
        private PredefinedVerb verb;
        private bool inProgress;
        private IStatementTarget _object;
        private Result result;
        private RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context context;
        private NullableDateTime timestamp;
        private Actor authority;
        private NullableBoolean voided;
        #endregion

        #region Properties
        /// <summary>
        /// representation of the statement verb
        /// </summary>
        public PredefinedVerb Verb
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
        /// The statements ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    id = null;
                }
                else
                {
                    string normalized = value.ToLower();
                    if (!ValidationHelper.IsValidUuid(normalized))
                    {
                        throw new ArgumentException("Statement ID must be UUID", "value");
                    }
                    id = normalized;
                }
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
        /// Returns the statement verb in its internal enum format
        /// </summary>
        /// <returns>Statement verb as an enum</returns>
        public PredefinedVerb GetVerbAsEnum()
        {
            return verb;
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
        /// Context information for this statement
        /// </summary>
        public RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Context Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// The timestamp of this statement
        /// </summary>
        public NullableDateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// The authority for this statement
        /// </summary>
        public Actor Authority
        {
            get { return authority; }
            set { authority = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NullableBoolean Voided
        {
            get { return voided; }
            set { voided = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool InProgress
        {
            get { return inProgress; }
            set { inProgress = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Statement() { }

        /// <summary>
        /// Creates a Statement with the minimum suggested properties
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The verb in this statement</param>
        /// <param name="statementTarget">The target of this statement</param>
        public Statement(Actor actor, PredefinedVerb verb, IStatementTarget statementTarget)
        {
            this.actor = actor;
            this.verb = verb;
            this._object = statementTarget;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates the statement, ensuring required fields are used
        /// and any necessary information (such as a result for specific verbs)
        /// is provided and valid.
        /// </summary>
        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (actor == null && verb != PredefinedVerb.Voided)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have an actor", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (Verb == PredefinedVerb.None)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have a verb", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (_object == null)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have an object", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (verb == PredefinedVerb.Voided)
            {
                bool objectStatementIdentified = (_object is TargetedStatement) && !string.IsNullOrEmpty(((TargetedStatement)_object).Id);
                if (!objectStatementIdentified)
                {
                    failures.Add(new ValidationFailure("Statement " + id + " has verb 'voided' but does not properly identify a statement as its object", ValidationLevel.Must));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
            }

            object[] children = new object[] { actor, verb, _object, result, context, timestamp, authority };
            foreach (object o in children)
            {
                if (o != null && o is IValidatable)
                {
                    failures.AddRange(((IValidatable)o).Validate(earlyReturnOnFailure));
                    if (earlyReturnOnFailure && failures.Count > 0)
                    {
                        return failures;
                    }
                }
            }
            return failures;
        }
        #endregion

        #region Verb Handling
        /// <summary>
        /// Handles verbs with special requirements
        /// </summary>
        public void HandleSpecialVerbs()
        {
            if (this.Verb.Equals("passed"))
            {
                result = (result == null) ? new Result() : result;
                VerifySuccessAndCompletionValues(result, "passed", true, true);
                result.Success = true;
                result.Completion = true;
            }
            else if (this.Verb.Equals("failed"))
            {
                result = (result == null) ? new Result() : result;
                VerifySuccessAndCompletionValues(result, "failed", false, true);
                result.Success = false;
                result.Completion = true;
            }
            else if (this.Verb.Equals("completed"))
            {
                result = (result == null) ? new Result() : result;
                VerifyCompletionValue(result, "completed", true);
                result.Completion = true;
            }
        }

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

        #region TinCan 0.95 Promotion
        /// <summary>
        /// Promotes a TinCan 0.9 statement to a 0.95 statement.  This can be a lossy conversion.
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source">A TinCan 0.9 Statement</param>
        /// <returns>A representation of the statement for TinCan 0.95</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement(Statement source)
        {
            if (source == null)
            {
                return null;
            }
            var result = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement();
            result.Id = source.Id;
            result.Actor = (Model.Actor)source.Actor;
            result.Verb = new Model.StatementVerb(source.verb);
            result.Object = source.Object;
            result.Result = source.Result;
            result.Context = source.Context;
            result.Timestamp = source.Timestamp;
            if (source.Authority != null)
            {
                result.Authority = (Model.Actor)source.Authority;
            }
            result.Voided = source.Voided;

            return result;
        }
        #endregion

        #region TinCan 1.0.0 Promotion

        /// <summary>
        /// Promotes a TinCan 0.9 statement to a 1.0.0 statement.  This can be a lossy conversion.
        /// 
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.Statement(Statement source)
        {
            if (source == null)
            {
                return null;
            }
            return (RusticiSoftware.TinCanAPILibrary.Model.Statement)(RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement)source;
        }
        #endregion
    }
}