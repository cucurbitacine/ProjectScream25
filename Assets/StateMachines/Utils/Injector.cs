using System;
using System.Reflection;
using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Utils
{
    public static class Injector
    {
        private const BindingFlags Binding = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        
        public static void InjectComponents(object target, GameObject gameObject)
        {
            var componentType = typeof(Component);
            
            var fields = target.GetType().GetFields(Binding);

            foreach (var field in fields)
            {
                var inject = field.GetCustomAttribute<InjectComponentAttribute>();

                if (inject == null) continue;
                if (!componentType.IsAssignableFrom(field.FieldType)) continue;

                Component component = null;
                
                if (inject.Source == ComponentSource.Self)
                {
                    component = gameObject.GetComponent(field.FieldType);
                }
                else if (inject.Source == ComponentSource.Children)
                {
                    component = gameObject.GetComponentInChildren(field.FieldType);
                }
                else if (inject.Source == ComponentSource.Parent)
                {
                    component = gameObject.GetComponentInParent(field.FieldType);
                }
                
                field.SetValue(target, component);
            }
        }
        
        public static void InjectFeature(object target, StateMachineData stateMachine, int stateId)
        {
            var featureType = typeof(IFeatureProcess);
            
            var fields = target.GetType().GetFields(Binding);
            foreach (var field in fields)
            {
                var inject = field.GetCustomAttribute<InjectProcessAttribute>();
                
                if (inject == null) continue;
                if (!featureType.IsAssignableFrom(field.FieldType)) continue;

                stateMachine.TryGetFeature(stateId, field.FieldType, out var feature);
                field.SetValue(target, feature);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InjectComponentAttribute : Attribute
    {
        public readonly ComponentSource Source;
        
        public InjectComponentAttribute(ComponentSource source)
        {
            Source = source;
        }
        
        public InjectComponentAttribute() : this(ComponentSource.Self)
        {
        }
    }
    
    public enum ComponentSource
    {
        Self,
        Children,
        Parent,
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectProcessAttribute : Attribute
    {
    }
}