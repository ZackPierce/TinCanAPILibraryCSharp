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
    public class StatementRef : IStatementTarget
    {
        #region Constants
        protected static readonly string OBJECT_TYPE = "StatementRef";
        #endregion

        #region Fields
        private string id;
        #endregion

        #region Properties
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string ObjectType
        {
            get { return OBJECT_TYPE; }
        }
        #endregion

        #region Constructor

        public StatementRef()
        { }

        public StatementRef(string id)
        {
            this.id = id;
        }
        #endregion

        #region Public Methods
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (id == null)
            {
                failures.Add(new ValidationFailure(string.Format("StatementRef had a null id property. Id should be a UUID string.", id), ValidationLevel.Should));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (!ValidationHelper.IsValidUuid(id))
            {
                failures.Add(new ValidationFailure(string.Format("StatementRef had an id of {0}, but ids, when present, must UUIDs", id), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            return failures;
        }
        #endregion
    }
}
