﻿#region License
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
    /// <summary>
    /// Definition of an activity, a core statement piece.
    /// </summary>
    public class ActivityDefinition : Extensible, IValidatable
    {
        /// <summary>
        /// A collection of language codes and their activity name.
        /// </summary>
        private LanguageMap name;

        public LanguageMap Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// A collection of language codes and their activity description.
        /// </summary>
        private LanguageMap description;

        public LanguageMap Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// The type of activity
        /// </summary>
        private Uri type;

        public Uri Type
        {
            get { return type; }
            set { type = value; }
        }

        private Uri moreInfo;

        public Uri MoreInfo
        {
            get { return moreInfo; }
            set { moreInfo = value; }
        }

        private string interactionType;

        public virtual string InteractionType
        {
            get { return interactionType; }
            set { interactionType = value; }
        }

        public ActivityDefinition()
        {
        }

        /// <summary>
        /// Simplified constructor to create an activity with a
        /// single language code, name, and description.
        /// </summary>
        /// <param name="name">The Activity Name</param>
        /// <param name="description">The Activity Description</param>
        /// <param name="languageCode">The Activity language code</param>
        /// <param name="type">The Activity Type</param>
        /// <param name="interactionType">The Interaction Type</param>
        public ActivityDefinition(string name, string description,
            string languageCode, Uri type,
            string interactionType) : this()
        {
            this.name.Add(languageCode, name);
            this.description.Add(languageCode, description);
            this.type = type;
            this.interactionType = interactionType;
        }

        public ActivityDefinition(LanguageMap name,
            LanguageMap description,
            Uri type, string interactionType)
        {
            this.name = name;
            this.description = description;
            this.type = type;
            this.interactionType = interactionType;
        }

        public ActivityDefinition(ActivityDefinition activityDefinition)
        {
            this.Extensions = activityDefinition.Extensions;
            this.Name = activityDefinition.Name;
            this.Description = activityDefinition.Description;
            this.Type = activityDefinition.Type;
            this.InteractionType = activityDefinition.InteractionType;
            this.moreInfo = activityDefinition.moreInfo;
        }

        public virtual bool Update(ActivityDefinition def)
        {
            bool updated = false;
            if (def == null)
            {
                return false;
            }

            if (def.Type != null && !def.type.Equals(this.type))
            {
                this.Type = def.Type;
                updated = true;
            }

            if (def.Name != null && def.Name.Count > 0 && !CommonFunctions.AreDictionariesEqual(this.name, def.name))
            {
                this.name = CommonFunctions.Merge<LanguageMap, string, string>(this.name, def.Name);
                updated = true;
            }
            if (def.description != null && def.description.Count > 0 && !CommonFunctions.AreDictionariesEqual(this.description, def.description))
            {
                this.description = CommonFunctions.Merge<LanguageMap, string, string>(this.description, def.description);
                updated = true;
            }
            if (def.InteractionType != null && !def.InteractionType.Equals(this.InteractionType))
            {
                this.InteractionType = def.InteractionType;
                updated = true;
            }

            if (def.moreInfo != null && !def.moreInfo.Equals(this.moreInfo))
            {
                this.moreInfo = def.moreInfo;
                updated = true;
            }

            return updated;
        }

        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (ValidationHelper.ValidateAndAddFailures(failures, this.name, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, this.description, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (this.type == null)
            {
                failures.Add(new ValidationFailure("The type property was null, but is recommended to be a non-null absolute IRI string.", ValidationLevel.BestPractice));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (!ValidationHelper.IsValidAbsoluteIri(this.type.ToString()))
            {
                failures.Add(new ValidationFailure("The type property, if present, must be an absolute IRI string.", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (this.moreInfo != null && !ValidationHelper.IsValidAbsoluteIri(this.moreInfo.ToString()))
            {
                // TODO - IRL check on top of IRI check
                failures.Add(new ValidationFailure("The type property, if present, must be an absolute IRL string.", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            this.AddExtensionValidationResults(failures, earlyReturnOnFailure);

            return failures;
        }
    }
}
