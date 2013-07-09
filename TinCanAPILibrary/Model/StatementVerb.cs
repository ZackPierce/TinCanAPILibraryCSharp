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
    public class StatementVerb : IValidatable
    {
        private string id;
        private LanguageMap display;

        private const string ExperiencedId = "http://adlnet.gov/expapi/verbs/experienced";
        private const string AttendedId = "http://adlnet.gov/expapi/verbs/attended";
        private const string AttemptedId = "http://adlnet.gov/expapi/verbs/attempted";
        private const string CompletedId = "http://adlnet.gov/expapi/verbs/completed";
        private const string PassedId = "http://adlnet.gov/expapi/verbs/passed";
        private const string FailedId = "http://adlnet.gov/expapi/verbs/failed";
        private const string AnsweredId = "http://adlnet.gov/expapi/verbs/answered";
        private const string InteractedId = "http://adlnet.gov/expapi/verbs/interacted";
        private const string ImportedId = "http://adlnet.gov/expapi/verbs/imported";
        private const string CreatedId = "http://adlnet.gov/expapi/verbs/created";
        private const string SharedId = "http://adlnet.gov/expapi/verbs/shared";
        private const string VoidedId = "http://adlnet.gov/expapi/verbs/voided";

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public LanguageMap Display
        {
            get { return display; }
            set { display = value; }
        }

        /// <summary>
        /// Creates a new StatementVerb that is empty.  Used by the JSON Serializer.
        /// </summary>
        public StatementVerb()
        {
        }

        /// <summary>
        /// Creates a new statement verb with the provided ID and Display fields
        /// </summary>
        /// <param name="id"></param>
        /// <param name="display"></param>
        public StatementVerb(Uri id, LanguageMap display)
        {
            this.id = id.ToString();
            this.display = display;
        }

        /// <summary>
        /// Creates a new statement verb with the provided ID and creates a language map with a single entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locale"></param>
        /// <param name="name"></param>
        public StatementVerb(Uri id, string locale, string name)
        {
            this.id = id.ToString();
            display = new LanguageMap();
            display[locale] = name;
        }

        /// <summary>
        /// Creates a new statement verb given a URI string that is validated and a language map with a single entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locale"></param>
        /// <param name="name"></param>
        public StatementVerb(string id, string locale, string name)
        {
            if (IsUri(id))
            {
                this.id = id;
            }
            else
            {
                throw new ArgumentException("The URI " + id + " is malformed.", "id");
            }
            display = new LanguageMap();
            display[locale] = name;
        }

        /// <summary>
        /// Creates a Statement Verb from the predefined list of verbs
        /// </summary>
        /// <param name="verb"></param>
        public StatementVerb(PredefinedVerb verb)
        {
            
            this.display = new LanguageMap();
            if (verb != PredefinedVerb.None)
            {
                this.id = "http://adlnet.gov/expapi/verbs/" + verb.ToString().ToLower();
                this.display["en-US"] = verb.ToString().ToLower();
            }
        }


        /// <summary>
        /// TODO - consider re-using ValidationHelper.IsValidAbsoluteIri instead
        /// Note that present implementation allows Relative Uris, not permitted in 1.0.x
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private bool IsUri(string source)
        {
            if (!string.IsNullOrEmpty(source) && Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
            {
                Uri tempValue;
                return Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out tempValue);
            }
            return false;
        }

        public bool IsVoided()
        {
            if (id == VoidedId)
            {
                return true;
            }
            if (display == null)
            {
                return false;
            }
            foreach (string s in display.Values)
            {
                if (s.ToLower().Equals("voided"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Demotes a 0.95 verb to a 0.90 verb.
        /// </summary>
        /// <param name="verb">A 0.95 verb.  It MUST have an id or an en-US entry in the display field.</param>
        /// <returns></returns>
        /// <remarks>If no valid id or en-US entry is in the display map, this method will return PredefinedVerb.Undefined.
        /// The core verbs from 0.90 are adl provided verbs in 0.95 to maintain some form of verb mapping.</remarks>
        public static explicit operator PredefinedVerb(StatementVerb verb)
        {
            var enumById = ParsePredefinedVerb(verb.id);
            if (enumById != PredefinedVerb.None)
            {
                return enumById;
            }

            string foundEnUs;
            if (verb.display != null && verb.display.TryGetValue("en-US", out foundEnUs))
            {
                return ParsePredefinedVerb(foundEnUs);
            }
            return PredefinedVerb.None;
        }

        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (id == null)
            {
                failures.Add(new ValidationFailure("Verb id must not be null", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (display == null)
            {
                failures.Add(new ValidationFailure("The display property was null, but should be used by all Statements", ValidationLevel.Should));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else
            {
                if (ValidationHelper.ValidateAndAddFailures(failures, display, earlyReturnOnFailure) && earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            return failures;
        }

        internal static PredefinedVerb ParsePredefinedVerb(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return PredefinedVerb.None;
            }

            switch (text.ToLowerInvariant())
            {
                case "experienced":
                case ExperiencedId:
                    return PredefinedVerb.Experienced;
                case "attended":
                case AttendedId:
                    return PredefinedVerb.Attended;
                case "attempted":
                case AttemptedId:
                    return PredefinedVerb.Attempted;
                case "completed":
                case CompletedId:
                    return PredefinedVerb.Completed;
                case "passed":
                case PassedId:
                    return PredefinedVerb.Passed;
                case "failed":
                case FailedId:
                    return PredefinedVerb.Failed;
                case "answered":
                case AnsweredId:
                    return PredefinedVerb.Answered;
                case "interacted":
                case InteractedId:
                    return PredefinedVerb.Interacted;
                case "imported":
                case ImportedId:
                    return PredefinedVerb.Imported;
                case "created":
                case CreatedId:
                    return PredefinedVerb.Created;
                case "shared":
                case SharedId:
                    return PredefinedVerb.Shared;
                case "voided":
                case VoidedId:
                    return PredefinedVerb.Voided;
                default:
                    return PredefinedVerb.None;
            }
        }
    }
}
