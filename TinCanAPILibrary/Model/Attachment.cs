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
    /// Statement attachment metadata representation
    /// </summary>
    public class Attachment : IValidatable
    {
        #region Fields
        private Uri usageType;
        private LanguageMap display;
        private LanguageMap description;
        private string contentType;
        private NullableInteger length;
        private string sha2;
        private Uri fileUrl;
        #endregion

        #region Properties

        /// <summary>
        /// Identifies the usage of this attachment.
        /// For example: one expected use case for attachments is to include a "completion certificate".
        /// A type IRI corresponding to this usage should be coined, and used with completion certificate attachments.
        /// </summary>
        public Uri UsageType
        {
            get { return usageType; }
            set { usageType = value; }
        }

        /// <summary>
        /// Display name (title) of this attachment.
        /// </summary>
        public LanguageMap Display
        {
            get { return display; }
            set { display = value; }
        }

        /// <summary>
        /// A description of the attachment
        /// </summary>
        public LanguageMap Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The Internet Media Type of the attachment.
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// Length of the associated attachment data in octets
        /// </summary>
        public NullableInteger Length
        {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// The SHA-2 hash of the attachment data. A minimum key size of 256 bits is recommended.
        /// </summary>
        public string Sha2
        {
            get { return sha2; }
            set { sha2 = value; }
        }

        /// <summary>
        /// An IRL at which the attachment data may be retrieved, or from which it used to be retrievable.
        /// </summary>
        public Uri FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        #endregion

        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();

            if (ValidationHelper.MandatoryNonNullValidateAndAddFailures(failures, display, earlyReturnOnFailure, "attachment.display", ValidationLevel.Must) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (ValidationHelper.ValidateAndAddFailures(failures, description, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (usageType == null)
            {
                failures.Add(new ValidationFailure("An attachment's usageType property MUST NOT be null", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (!usageType.IsAbsoluteUri)
            {
                failures.Add(new ValidationFailure(string.Format("An attachment's usageType property MUST be an absolute IRI, but was {0}", usageType), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (length == null)
            {
                failures.Add(new ValidationFailure("An attachment's length property MUST NOT be null", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (sha2 == null)
            {
                failures.Add(new ValidationFailure("An attachment's sha2 property MUST NOT be null", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else if (!ValidationHelper.IsValidBase64(sha2))
            {
                failures.Add(new ValidationFailure(string.Format("An attachment's sha2 property MUST be a Base64 formated string, but was {0}", sha2), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            if (!ValidationHelper.IsValidInternetMediaType(contentType))
            {
                failures.Add(new ValidationFailure(string.Format("An attachment's contentType property MUST be an Internet Media Type, but was {0}", contentType), ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }

            return failures;
        }
    }
}
