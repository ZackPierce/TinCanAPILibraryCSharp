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
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Definition of any extensible TinCanAPI Statement.
    /// Allows for arbitrary key/value pairs to be attached to
    /// an object.
    /// </summary>
    public abstract class Extensible
    {
        private Dictionary<Uri, object> extensions;
        
        public Dictionary<Uri, object> Extensions
        {
            get { return extensions; }
            set { extensions = value; }
        }

        public Extensible() { }
        public Extensible(Extensible extensible)
        {
            this.extensions = extensible.extensions;
        }

        public List<ValidationFailure> ValidateExtensions(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (extensions != null)
            {
                foreach (var kvp in this.extensions)
                {
                    if (!ValidationHelper.IsValidAbsoluteIri(kvp.Key.ToString()))
                    {
                        failures.Add(new ValidationFailure("Extension object keys must be absolute IRIs, but instead found a property named: " + kvp.Key, ValidationLevel.Must));
                        if (earlyReturnOnFailure)
                        {
                            return failures;
                        }
                    }
                }
            }
            return failures;
        }
    }
}
