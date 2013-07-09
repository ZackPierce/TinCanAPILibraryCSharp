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
    /// Base class for 0.95 and 1.0.x Context models
    /// </summary>
    public abstract class ContextBase : Extensible
    {
        #region Fields
        private string registration;
        private Actor instructor;
        private Group team;
        private string revision;
        private string platform;
        private string language;
        #endregion

        #region Properties
        /// <summary>
        /// The Registration UUID
        /// </summary>
        public string Registration
        {
            get { return registration; }
            set { registration = value; }
        }

        /// <summary>
        /// The instructor in this context
        /// </summary>
        public Actor Instructor
        {
            get { return instructor; }
            set { instructor = value; }
        }

        /// <summary>
        /// The team in this context
        /// </summary>
        public Group Team
        {
            get { return team; }
            set { team = value; }
        }

        /// <summary>
        /// The revision
        /// </summary>
        public string Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        /// <summary>
        /// The platform
        /// </summary>
        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        /// <summary>
        /// Code representing the language in which the experience being recorded in this Statement (mainly) occurred in, if applicable and known.
        /// RFC 5646
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        #endregion

        /// <summary>
        /// Copies property values from one instance to another.
        /// TODO - deep clone rather than shallow copy
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sink"></param>
        internal static void TransferProperties(ContextBase source, ContextBase sink)
        {
            if (source == null)
            {
                return;
            }
            sink.Registration = source.Registration;
            sink.Instructor = source.Instructor;
            sink.Team = source.Team;
            sink.Revision = source.Revision;
            sink.Platform = source.Platform;
            sink.Language = source.Language;
        }

        #region Protected Methods

        protected List<ValidationFailure> validateImplementation(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (registration != null && !ValidationHelper.IsValidUuid(registration))
            {
                failures.Add(new ValidationFailure(string.Format("Context had a registration of {0}, but registrations, when present, must UUIDs", registration), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            foreach (var validatableKid in new IValidatable[] { instructor, team})
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, validatableKid, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (language != null && !ValidationHelper.IsValidLanguageTag(language))
            {
                failures.Add(new ValidationFailure(string.Format("Context had a language of {0}, but the language property, when present, must be a RFC 5646 compliant language code.", language), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            this.AddExtensionValidationResults(failures, earlyReturnOnFailure);
            return failures;
        }
        #endregion
    }
}
