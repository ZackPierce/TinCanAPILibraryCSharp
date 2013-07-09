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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Actor : IStatementTarget
    {
        #region Constants
        protected static readonly string OBJECT_TYPE = "Agent";

        private const string account_key = "account";
        private const string mbox_key = "mbox";
        private const string mbox_sha1sum_key = "mbox_sha1sum";
        private const string openid_key = "openid";
        #endregion

        #region Fields
        private string name;
        private string mbox;
        private string mbox_sha1sum;
        private string openid;
        private AgentAccount account;
        #endregion

        #region Properties
        /// <summary>
        /// ObjectType accessor
        /// </summary>
        public virtual string ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        /// <summary>
        /// Array of names for the actor
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Array of mailboxes for the actor
        /// </summary>
        public string Mbox
        {
            get { return mbox; }
            set { mbox = value; }
        }

        /// <summary>
        /// Array of email sha1sums for the actor
        /// </summary>
        public string Mbox_sha1sum
        {
            get { return mbox_sha1sum; }
            set
            {
                mbox_sha1sum = value.ToLower();
            }
        }

        /// <summary>
        /// Array of OpenIDs for the actor
        /// </summary>
        public string Openid
        {
            get { return openid; }
            set
            {
                openid = value.ToLower();
            }
        }

        /// <summary>
        /// A list of accounts for the actor
        /// </summary>
        public AgentAccount Account
        {
            get { return account; }
            set { account = value; }
        }

        #endregion

        #region Constructor
        public Actor() { }
        public Actor(string name, string email)
        {
            this.name = name;
            this.Mbox = email;
        }

        public Actor(string name, AgentAccount account)
        {
            this.name = name;
            this.account = account;
        }

        public Actor(Actor src)
        {
            this.name = src.Name;
            this.Mbox = src.mbox;
            this.mbox_sha1sum = src.mbox_sha1sum;
            this.openid = src.openid;
            this.account = src.account;
        }

        protected uint NumberOfInverseFunctionalIdentifiers
        {
            get
            {
                uint properties = 0;
                if (mbox != null)
                {
                    properties++;
                }
                if (mbox_sha1sum != null)
                {
                    properties++;
                }
                if (openid != null)
                {
                    properties++;
                }
                if (account != null)
                {
                    properties++;
                }
                return properties;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates that the object abides by its rules
        /// </summary>
        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            return ValidateAsAgent(earlyReturnOnFailure);
        }

        /// <summary>
        /// Gets the Hash Code of the object.  However, due to the odd nature of the
        /// Actor object, the properties of a Hash cannot be fulfilled, so it is
        /// not recommended to use this object in a HashTable.
        /// </summary>
        /// <returns>0.  object is not effectively hashable</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        #endregion

        #region Protectected Methods

        private IEnumerable<ValidationFailure> ValidateAsAgent(bool earlyReturnOnFailure)
        {
            return ValidateInverseFunctionalIdentifiers(earlyReturnOnFailure);
        }

        protected List<ValidationFailure> ValidateInverseFunctionalIdentifiers(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (mbox != null && !ValidationHelper.IsValidMailtoIRI(mbox))
            {
                failures.Add(new ValidationFailure(string.Format("mbox must have the format 'mailto:(email address)', but was {0}", mbox), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (openid != null && !ValidationHelper.IsValidAbsoluteIri(openid))
            {
                failures.Add(new ValidationFailure(string.Format("If present, openid must be a valid IRI, but was {0}", openid), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, account, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            var numProperties = NumberOfInverseFunctionalIdentifiers;
            if (numProperties != 1)
            {
                failures.Add(new ValidationFailure("Exactly 1 inverse functional properties must be defined.  However, " + numProperties + " are defined.", ValidationLevel.Must));
            }
            return failures;
        }
        #endregion

        #region TinCan 0.90 Conversion
        /// <summary>
        /// Down-converts to a TinCan 0.90 Actor
        /// </summary>
        /// <param name="source">A tincan 0.95 Actor</param>
        /// <returns>A tincan 0.90 actor</returns>
        /// <remarks></remarks>
        public static explicit operator Model.TinCan0p90.Actor(Actor source)
        {
            return new Model.TinCan0p90.Actor(source.Name, source.Mbox, source.Mbox_sha1sum, source.Openid, source.Account);
        }
        #endregion
    }
}