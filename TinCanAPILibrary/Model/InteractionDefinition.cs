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
    public class InteractionDefinition : ActivityDefinition
    {
        private List<string> correctResponsesPattern;
        private InteractionType interactionType = new InteractionType(InteractionTypeValue.Undefined);

        //Each of these is optional, depending on the type of interaction
        private List<InteractionComponent> choices;
        private List<InteractionComponent> scale;
        private List<InteractionComponent> source;
        private List<InteractionComponent> target;
        private List<InteractionComponent> steps;

        private Dictionary<InteractionComponentName, List<InteractionComponent>> componentSets
        {
            get
            {
                return new Dictionary<InteractionComponentName, List<InteractionComponent>>()
                    {
                        {InteractionComponentName.Choices, choices},
                        {InteractionComponentName.Scale, scale},
                        {InteractionComponentName.Source, source},
                        {InteractionComponentName.Target, target},
                        {InteractionComponentName.Steps, steps}
                    };
            }
        }

        public InteractionDefinition() { }

        public InteractionDefinition(ActivityDefinition def)
        {
            this.Update(def);
        }

        public override bool Update(ActivityDefinition activityDef)
        {
            bool updated = base.Update(activityDef);
            if (!(activityDef is InteractionDefinition))
            {
                return updated;
            }

            InteractionDefinition def = (InteractionDefinition)activityDef;

            // TODO - deep copy rather than shallow
            if (NotNullAndNotEqual(def.CorrectResponsesPattern, this.CorrectResponsesPattern))
            {
                this.CorrectResponsesPattern = def.CorrectResponsesPattern;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Choices, this.Choices))
            {
                this.Choices = def.Choices;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Scale, this.Scale))
            {
                this.Scale = def.Scale;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Source, this.Source))
            {
                this.Source = def.Source;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Target, this.Target))
            {
                this.Target = def.Target;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Steps, this.Steps))
            {
                this.Steps = def.Steps;
                updated = true;
            }

            return updated;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        protected bool NotNullAndNotEqual<T>(List<T> list1, List<T> list2)
        {
            return list1 != null && list1.Count > 0 && !CommonFunctions.AreListsEqualIgnoringOrder(list1, list2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is InteractionDefinition))
            {
                return false;
            }
            InteractionDefinition otherDef = (InteractionDefinition)obj;
            return base.Equals(obj)
                        && CommonFunctions.AreListsEqualIgnoringOrder(correctResponsesPattern, otherDef.correctResponsesPattern)
                        && CommonFunctions.AreListsEqualIgnoringOrder(this.Choices, otherDef.Choices)
                        && CommonFunctions.AreListsEqualIgnoringOrder(this.Scale, otherDef.Scale)
                        && CommonFunctions.AreListsEqualIgnoringOrder(this.Source, otherDef.Source)
                        && CommonFunctions.AreListsEqualIgnoringOrder(this.Target, otherDef.Target)
                        && CommonFunctions.AreListsEqualIgnoringOrder(this.Steps, otherDef.Steps);
        }

        public override IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = (List<ValidationFailure>)base.Validate(earlyReturnOnFailure);
            if (failures.Count > 0 && earlyReturnOnFailure)
            {
                return failures;
            }

            

            if (this.Type != null && this.Type.ToString() != Constants.CmiInteractionActivityType)
            {
                failures.Add(new ValidationFailure("Interaction activities should have a type property of value 'http://adlnet.gov/expapi/activities/cmi.interaction', but found a type property of: " + this.Type, ValidationLevel.Should));
            }

            if (ValidationHelper.AddFailureIfContainsNullMembers(failures, this.CorrectResponsesPattern, ValidationLevel.May, earlyReturnOnFailure) && earlyReturnOnFailure)
            {
                return failures;
            }

            if (this.interactionType.Value == InteractionTypeValue.Undefined)
            {
                failures.Add(new ValidationFailure("Interaction activities must have a valid interactionType", ValidationLevel.Must));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else
            {

                foreach (var nameAndComponents in componentSets)
                {
                    var components = nameAndComponents.Value;
                    if (components == null)
                    {
                        continue;
                    }
                    if (!IsValidComponentForInteractionType(this.interactionType.Value, nameAndComponents.Key))
                    {
                        failures.Add(new ValidationFailure(string.Format("component array named {0} is not permitted for an interaction type of {1}", nameAndComponents.Key, this.interactionType.Value), ValidationLevel.May));
                        if (earlyReturnOnFailure)
                        {
                            return failures;
                        }
                    }

                    var ids = new Dictionary<string, bool>();
                    foreach (var interactionComponent in components)
                    {
                        if (interactionComponent == null)
                        {
                            failures.Add(new ValidationFailure(string.Format("member of the {0} collection was found to be null.", nameAndComponents.Key), ValidationLevel.May));
                            if (earlyReturnOnFailure)
                            {
                                return failures;
                            }
                        }
                        if (ValidationHelper.ValidateAndAddFailures(failures, interactionComponent, earlyReturnOnFailure) && earlyReturnOnFailure)
                        {
                            return failures;
                        }

                        if (ids.ContainsKey(interactionComponent.Id))
                        {
                            failures.Add(new ValidationFailure(string.Format("members of the {0} collection had the redundant id: {1}", nameAndComponents.Key, interactionComponent.Id), ValidationLevel.May));
                            if (earlyReturnOnFailure)
                            {
                                return failures;
                            }
                        }
                        else
                        {
                            ids[interactionComponent.Id] = true;
                        }
                    }
                }
            }

            return failures;
        }

        public static bool IsValidComponentForInteractionType(InteractionTypeValue interactionType, InteractionComponentName componentName)
        {
            if (interactionType == InteractionTypeValue.Choice || interactionType == InteractionTypeValue.Sequencing)
            {
                return componentName == InteractionComponentName.Choices;
            }
            else if (interactionType == InteractionTypeValue.Likert)
            {
                return componentName == InteractionComponentName.Scale;
            }
            else if (interactionType == InteractionTypeValue.Matching)
            {
                return componentName == InteractionComponentName.Source || componentName == InteractionComponentName.Target;
            }
            else if (interactionType == InteractionTypeValue.Performance)
            {
                return componentName == InteractionComponentName.Steps;
            }
            return false;
        }

        protected void CheckComponentSet(InteractionComponentName componentName, List<InteractionComponent> componentList)
        {
            if (componentList == null)
            {
                return;
            }
            if (!IsValidComponentForInteractionType(this.interactionType.Value, componentName))
            {
                throw new ArgumentException(componentName.ToString().ToLower() + " is not a valid interaction component for the given interactionType", "componentName");
            }
        }

        #region Properties

        public override string InteractionType
        {
            get { return (interactionType.Value == InteractionTypeValue.Undefined) ? null : interactionType.ToString(); }
            set { this.interactionType = value == null ? new InteractionType(InteractionTypeValue.Undefined) : new InteractionType(value); }
        }

        public List<string> CorrectResponsesPattern
        {
            get { return correctResponsesPattern; }
            set { correctResponsesPattern = value; }
        }

        public List<InteractionComponent> Choices
        {
            get { return choices; }
            set { choices = value; }
        }
        public List<InteractionComponent> Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public List<InteractionComponent> Source
        {
            get { return source; }
            set { source = value; }
        }
        public List<InteractionComponent> Target
        {
            get { return target; }
            set { target = value; }
        }
        public List<InteractionComponent> Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        #endregion
    }
}
