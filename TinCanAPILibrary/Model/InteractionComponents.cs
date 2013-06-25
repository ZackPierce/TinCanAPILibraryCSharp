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
using System.Text.RegularExpressions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class InteractionComponent : IValidatable
    {
        private string id;
        private LanguageMap description;

        private static Regex HasWhitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public LanguageMap Description
        {
            get { return description; }
            //Including for use by deserialization code. Do not use.
            set { description = value; }
        }

        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (id == null)
            {
                failures.Add(new ValidationFailure("id should not be null on Interaction Components.", ValidationLevel.May));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            } else if (HasWhitespace(id)) {
                failures.Add(new ValidationFailure("An interaction component's id value should not have whitespace, but was :" + id, ValidationLevel.May));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, description, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            return failures;
        }

        private static bool HasWhitespace(string candidate)
        {
            return HasWhitespaceRegex.IsMatch(candidate);
        }
    }
}
