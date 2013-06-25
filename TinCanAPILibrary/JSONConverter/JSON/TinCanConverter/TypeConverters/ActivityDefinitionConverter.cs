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
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class ActivityDefinitionConverter : IJsonTypeConverter
    {
        private Type myType = typeof(ActivityDefinition);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            //Integration.Implementation.LogAudit("TinCanActor Deserialize called", null);
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            string typeField = null;
            if (objMap.Contains("type"))
            {
                typeField = (string)objMap["type"];
            }

            //Avoid infinite loop here, if type is this base class
            Type targetType = typeof(ActivityDefinition_JsonTarget);

            string interactionTypeField = null;
            if (objMap.Contains("interactionType"))
            {
                interactionTypeField = (string)objMap["interactionType"];
            }

            if (typeField == Constants.CmiInteractionActivityType || Constants.CmiInteractionTypes.Contains(interactionTypeField))
            {
                targetType = typeof(InteractionDefinition);
            }

            return converter.DeserializeJSON(value, targetType);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            //Avoid infinite loop here, so we don't ever return just a Uri type
            return new ActivityDefinition_JsonTarget((ActivityDefinition)value);
        }

        // since TinCanActor is now a concrete class, 
        // provide this to reduce to so serialization doesn't get in an infinite loop
        public class ActivityDefinition_JsonTarget : ActivityDefinition
        {
            public ActivityDefinition_JsonTarget() { }
            public ActivityDefinition_JsonTarget(ActivityDefinition activityDef) : base(activityDef) { }
        }
    }
}
