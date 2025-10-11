using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using StateMachines.Presets;
using UnityEditor;
using UnityEngine;
using ValueType = StateMachines.Presets.ValueType;

namespace StateMachines.Editor
{
    [CustomPropertyDrawer(typeof(ConditionPreset))]
    public class ConditionDrawer : PropertyDrawer
    {
        private static readonly Dictionary<ConditionPreset, bool> FoldoutCache = new Dictionary<ConditionPreset, bool>();
        private const float Spacing = 0f;
        private const bool FoldoutDefault = false;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var condition = GetTargetObjectOfProperty(property) as ConditionPreset;
            
            label.text = GetConditionDisplayName(condition);
            
            EditorGUI.BeginProperty(position, label, property);

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var rect = new Rect(position.x, position.y, position.width, lineHeight);

            var foldout = GetFoldout(condition);
            var newFoldout = EditorGUI.Foldout(rect, foldout, label, true);
            if (foldout != newFoldout)
            {
                foldout = newFoldout;
                SetFoldout(condition, foldout);
            }
            
            if (foldout)
            {
                rect.y += lineHeight + Spacing;
                var valueProp = property.FindPropertyRelative(nameof(ConditionPreset.ValuePreset));
                EditorGUI.PropertyField(rect, valueProp);

                
                rect.y += lineHeight + Spacing;
                var operationProp = property.FindPropertyRelative(nameof(ConditionPreset.Operation));
                EditorGUI.PropertyField(rect, operationProp);
                
                rect.y += lineHeight + Spacing;
                var valueRef = valueProp.objectReferenceValue as ValuePreset;
                if (valueRef)
                {
                    switch (valueRef.ValueType)
                    {
                        case ValueType.Bool:
                            var boolProp = property.FindPropertyRelative(nameof(ConditionPreset.BoolValue));
                            EditorGUI.PropertyField(rect, boolProp, new GUIContent("Value"));
                            break;
                        case ValueType.Int:
                            var intProp = property.FindPropertyRelative(nameof(ConditionPreset.IntValue));
                            EditorGUI.PropertyField(rect, intProp, new GUIContent("Value"));
                            break;
                        case ValueType.Float:
                            var floatProp = property.FindPropertyRelative(nameof(ConditionPreset.FloatValue));
                            EditorGUI.PropertyField(rect, floatProp, new GUIContent("Value"));
                            break;
                        case ValueType.String:
                            var stringProp = property.FindPropertyRelative(nameof(ConditionPreset.StringValue));
                            EditorGUI.PropertyField(rect, stringProp, new GUIContent("Value"));
                            break;
                    }
                }
                else
                {
                    EditorGUI.LabelField(rect, $"(no {nameof(ValuePreset)} assigned)");
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var condition = GetTargetObjectOfProperty(property) as ConditionPreset;
            var foldout = GetFoldout(condition);
            
            if (!foldout) return EditorGUIUtility.singleLineHeight;
            
            return (EditorGUIUtility.singleLineHeight + 2f) * 4;
        }
        
        private static bool GetFoldout(ConditionPreset condition)
        {
            if (FoldoutCache.TryGetValue(condition, out var foldout))
            {
                return foldout;
            }

            SetFoldout(condition, FoldoutDefault);
            return FoldoutDefault;
        }
        
        private static void SetFoldout(ConditionPreset condition, bool foldout)
        {
            FoldoutCache[condition] = foldout;
        }

        private static string GetConditionDisplayName(ConditionPreset condition)
        {
            var dataName = condition.ValuePreset ? condition.ValuePreset.ValueName : "No Data";

            var valueName = "Unknown";
            
            if (condition.ValuePreset)
            {
                if (condition.ValuePreset.ValueType == ValueType.Bool)
                {
                    valueName = $"{condition.BoolValue}";
                }
                else if (condition.ValuePreset.ValueType == ValueType.Int)
                {
                    valueName = $"{condition.IntValue}";
                }
                else if (condition.ValuePreset.ValueType == ValueType.Float)
                {
                    valueName = $"{condition.FloatValue}";
                }
                else if (condition.ValuePreset.ValueType == ValueType.String)
                {
                    valueName = $"{condition.StringValue}";
                }
            }

            var displayName = $"\"{dataName}\" {GetOperatorName(condition.Operation)} \"{valueName}\"";

            return displayName;
        }
        
        private static string GetOperatorName(OperatorType operatorType)
        {
            switch (operatorType)
            {
                case OperatorType.NotEqual: return "!=";
                case OperatorType.Less: return "<";
                case OperatorType.LessOrEquals: return "<=";
                case OperatorType.Equals: return "==";
                case OperatorType.GreaterOrEquals: return ">=";
                case OperatorType.Greater: return ">";
                default: throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
            }
        }
        
        private static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            object obj = prop.serializedObject.targetObject;
            var parts = prop.propertyPath.Replace(".Array.data[", "[").Split('.');

            foreach (var part in parts)
            {
                if (part.Contains("["))
                {
                    var fieldName = part.Substring(0, part.IndexOf("[", StringComparison.Ordinal));
                    var index = int.Parse(part.Substring(part.IndexOf("[", StringComparison.Ordinal) + 1).Replace("]", ""));
                    obj = GetValue(obj, fieldName, index);
                }
                else
                {
                    obj = GetValue(obj, part);
                }
            }

            return obj;
        }

        private static object GetValue(object source, string fieldName)
        {
            if (source == null) return null;
            
            var type = source.GetType();
            while (type != null)
            {
                var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field != null) return field.GetValue(source);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue(object source, string fieldName, int index)
        {
            if (GetValue(source, fieldName) is not IEnumerable enumerable) return null;

            var i = 0;
            foreach (var element in enumerable)
            {
                if (i == index) return element;
                i++;
            }
            return null;
        }
    }
}