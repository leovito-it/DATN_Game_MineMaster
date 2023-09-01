using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Sound
{
    public SoundName Name;
    public AudioClip Clip;

    public Sound()
    {
        Name = new SoundName();
        Clip = null;
    }
}

public static class SoundExtensions
{
    public static AudioClip Get(this SoundName name)
    {
        return Sounds.Get(name);
    }
}

public class Sounds : ScriptableObject
{
    public const string SOUND = "SOUNDS";

    public static Sounds Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Sounds>("Sounds"); // Load the ScriptableObject from Resources
            }
            return _instance;
        }
    }

    private static Sounds _instance;

    [HideInInspector]
    public List<Sound> List = new();

    public static AudioClip Get(SoundName name)
    {
        Sound sound = Instance.List.Find((x) => x.Name == name);
        return sound?.Clip;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Sounds))]
    public class SoundsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Sounds sounds = (Sounds)target;

            serializedObject.Update();

            int total = Enum.GetNames(typeof(SoundName)).Length; ;

            bool needUpdate = sounds.List.Count != total;
            serializedObject.Update();

            if (needUpdate)
            {
                if (total > sounds.List.Count)
                {
                    int count = total - sounds.List.Count;
                    for (int j = 0; j < count; j++)
                    {
                        sounds.List.Add(new());
                    }
                }

                if (total > sounds.List.Count)
                    sounds.List.RemoveRange(total, sounds.List.Count - total);
            }

            DrawDefaultInspector();

            EditorGUILayout.LabelField("=== Audio Dictionary ===");


            for (int i = 0; i < total; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string[] audioEnumNames = System.Enum.GetNames(typeof(SoundName));

                if (i < audioEnumNames.Length)
                {
                    EditorGUILayout.LabelField($"{i + 1}.", GUILayout.MaxWidth(20));
                    EditorGUILayout.LabelField(audioEnumNames[i].Format_LableStyle(), GUILayout.MaxWidth(150));

                    sounds.List[i].Name = (SoundName)i;
                    sounds.List[i].Clip = (AudioClip)EditorGUILayout.ObjectField(sounds.List[i].Clip, typeof(AudioClip),
                    false, GUILayout.MaxWidth(300));
                }
                else
                {
                    GUIStyle alertStyle = new();
                    alertStyle.normal.textColor = Color.red;

                    EditorGUILayout.LabelField($"{i + 1}.", GUILayout.MaxWidth(20));
                    EditorGUILayout.LabelField("[Define name require]", alertStyle, GUILayout.MaxWidth(200));
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(serializedObject.targetObject, "Modify data");
        }
    }
#endif
}


