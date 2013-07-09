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
namespace RusticiSoftware.TinCanAPILibrary
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RusticiSoftware.TinCanAPILibrary.Model;
    using RusticiSoftware.TinCanAPILibrary.Json;
    using System.Collections;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Handles special case for ContextActivities properties that may either be 
    /// single Activity objects or arrays of Activity objects.
    /// </summary>
    public class ContextActivitiesConverter : IJsonTypeConverter
    {
        private readonly Type singleType = typeof(ContextActivities);
        private readonly Type arrayType = typeof(ContextActivities[]);

        public Type GetTargetClass()
        {
            return singleType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            if (objMap == null)
            {
                return null;
            }
            var contextActivities = new ContextActivities();
            contextActivities.Parent = singleOrArrayToArray(objMap["parent"]);
            contextActivities.Category = singleOrArrayToArray(objMap["category"]);
            contextActivities.Grouping = singleOrArrayToArray(objMap["grouping"]);
            contextActivities.Other = singleOrArrayToArray(objMap["other"]);

            return contextActivities;
        }

        private Activity[] singleOrArrayToArray(object propertyValue)
        {
            if (propertyValue == null)
            {
                return null;
            }
            
            if (propertyValue is JArray)
            {
                var result = new List<Activity>();
                foreach (var token in (propertyValue as JArray))
                {
                    result.Add(token.ToObject<Activity>()); // TODO - pull through custom serialization settings
                }
                return result.ToArray();
            }
            else
            {
                return new Activity[] { (propertyValue as JToken).ToObject<Activity>() }; // TODO - pull through custom serialization settings
            }
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return value;
        }

    }
}
