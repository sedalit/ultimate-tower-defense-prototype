﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class Sounds : ScriptableObject
{
    [SerializeField] private AudioClip[] m_Sounds;
    public AudioClip this[Sound s] => m_Sounds[(int)s];

    #if UNITY_EDITOR
    [CustomEditor(typeof(Sounds))]
    public class SoundsInspector : Editor
    {
        private static int SoundCount => System.Enum.GetValues(typeof(Sound)).Length;
        private new Sounds target => base.target as Sounds;
        public override void OnInspectorGUI()
        {
            if (target.m_Sounds.Length < SoundCount) System.Array.Resize(ref target.m_Sounds, SoundCount);
            for (int i = 0; i < target.m_Sounds.Length; i++)
            {
                target.m_Sounds[i] = EditorGUILayout.ObjectField(
                    $"{(Sound)i}:", target.m_Sounds[i], typeof(AudioClip), false) as AudioClip;
            }
        }
    }
    #endif
}
