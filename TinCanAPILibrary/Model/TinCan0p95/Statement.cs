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

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95
{
    using System;
    using System.Collections.Generic;
        using System.Text;
    using RusticiSoftware.TinCanAPILibrary.Model;

    /// <summary>
    /// Tin Can 0.95 version of Statement
    /// </summary>
    public class Statement : StatementBase
    {
        private NullableBoolean voided;

        /// <summary>
        /// 
        /// </summary>
        public NullableBoolean Voided
        {
            get { return voided; }
            set { voided = value; }
        }

        #region TinCan 0.90 Downgrade
        /// <summary>
        /// Demotes a TinCan 0.95 Statement to TinCan 0.9
        /// </summary>
        /// <param name="source">A TinCan 0.95 Statement</param>
        /// <returns>The TinCan 0.90 representation of the statement</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Statement(Statement source)
        {
            var result = new RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Statement();
            result.Id = source.Id;
            result.Actor = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Actor)source.Actor;
            result.Verb = ((RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.StatementVerb)source.GetVerbAsEnum()).ToString().ToLower();
            result.InProgress = false;
            result.Object = source.Object;
            result.Result = source.Result;
            result.Context = source.Context;
            result.Timestamp = source.Timestamp;
            if (source.Authority != null)
            {
                result.Authority = (RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Actor)source.Authority;
            }
            result.Voided = source.Voided;

            return result;
        }
        #endregion

        #region TinCan 1.0.0 upgrade
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.Statement(Statement source)
        {
            var upgrade = new RusticiSoftware.TinCanAPILibrary.Model.Statement();
            upgrade.Actor = source.Actor;
            upgrade.Authority = source.Authority;
            upgrade.Context = source.Context;
            upgrade.Id = source.Id;
            upgrade.Object = source.Object;
            upgrade.Result = source.Result;
            upgrade.Stored = source.Stored;
            upgrade.Timestamp = source.Timestamp;
            upgrade.Verb = source.Verb;
            return upgrade;
        }
        #endregion
    }
}
