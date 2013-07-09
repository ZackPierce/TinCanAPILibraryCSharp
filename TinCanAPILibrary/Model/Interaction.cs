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
    public class Interaction : Activity
    {
        private InteractionDefinition definition;

        public new InteractionDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        #region Constructors
        public Interaction() { }

        public Interaction(string id)
            : base(id)
        { }
        #endregion

        public override IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (!ValidationHelper.IsValidAbsoluteIri(Id))
            {
                failures.Add(new ValidationFailure("Activity does not have a valid identifier. The id property must be a non-null absolute IRI", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, definition, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }
            return failures;
        }
    }
}
