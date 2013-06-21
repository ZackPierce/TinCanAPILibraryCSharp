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
    public class Statement : StatementBase, IValidatable
    {
        private string version;

        /// <summary>
        /// Either 0.9, 0.95, or a Semantic Versioning 1.0.0-compliant value corresponding to
        /// the xAPI version of the statement, greater than or equal to 1.0.0.
        /// Per xAPI 1.0.x, Client applications should not be setting the Statement version,
        /// but must set it to "1.0.0" if they do.
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }


        public Statement() 
        { }

        public Statement(Actor actor, StatementVerb verb, StatementTarget statementTarget) :
            base(actor, verb, statementTarget)
        { }

        public Statement(Actor actor, PredefinedVerbs verb, StatementTarget statementTarget) :
            base(actor, verb, statementTarget)
        { }

        #region TinCan previous versions downgrade
        /// <summary>
        /// Demotes a TinCan 1.0.0 Statement to TinCan 0.9
        /// </summary>
        /// <param name="source">A TinCan 1.0.0 Statement</param>
        /// <returns>The TinCan 0.90 representation of the statement</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p90.Statement(Statement source)
        {
            throw new NotImplementedException(); // TODO
        }

        /// <summary>
        /// Demotes a TinCan 1.0.0 Statement to TinCan 0.95
        /// </summary>
        /// <param name="source">A TinCan 1.0.0 Statement</param>
        /// <returns>The TinCan 0.95 representation of the statement</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator RusticiSoftware.TinCanAPILibrary.Model.TinCan0p95.Statement(Statement source)
        {
            throw new NotImplementedException(); // TODO
        }
        #endregion
    }
}
