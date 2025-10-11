using StateMachines.Presets;
using UnityEditor;
using UnityEngine;

namespace StateMachines.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        private static StateMachineEditor _editor;

        private static StateMachinePreset _machine;
        
        [MenuItem("Samurai/State Machine Window")]
        public static void ShowWindow()
        {
            if (_editor != null) _editor.Close();
            
            _editor = GetWindow<StateMachineEditor>();
            
            _editor.Show();
        }

        public void SetState(StatePreset state)
        {
            StateEditor.ShowWindow(state);
        }
        
        private void OnGUI()
        {
            _machine = (StateMachinePreset)EditorGUILayout.ObjectField("Machine", _machine, typeof(StateMachinePreset), false);
            EditorGUILayout.Separator();
            
            if (!_machine) return;
            
            foreach (var state in _machine.States)
            {
                if (state && GUILayout.Button(state.name))
                {
                    SetState(state);
                }
            }
        }
    }

    public class StateEditor : EditorWindow
    {
        private static StateEditor _editor;
        private static StatePreset _state;
        
        public static void ShowWindow(StatePreset state)
        {
            if (_editor == null)
            {
                _editor = GetWindow<StateEditor>();
                
                _editor.Show();
            }

            _state = state;
            
            _editor.Repaint();
        }

        private void OnGUI()
        {
            if (!_state) return;
            
            EditorGUILayout.ObjectField("State", _state, typeof(StatePreset), false);
            EditorGUILayout.Separator();
            
            foreach (var transition in _state.Transitions)
            {
                if (transition != null && transition.Target && GUILayout.Button(transition.Target.name))
                {
                    _state = transition.Target;
                }
            }
        }
    }
}
