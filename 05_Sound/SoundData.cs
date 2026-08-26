using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Sound
{
    public enum SoundType
    {
        // Main
        MAIN,
        INTRO,

        // Combat
        ATTACK,
        IMPACT,
        RELOAD,
    }

    [CreateAssetMenu(menuName = "Core/SoundData", fileName = "Sound_")]
    public sealed class SoundData : ScriptableObject
    {
        [field: SerializeField] public int AudioSize { get; private set; } = 1;
        [field: SerializeField] public AudioMixerGroup[] Outputs { get; private set; }
        [field: SerializeField] public SoundInfo[] SoundInfos { get; private set; }

        [HideInInspector] public AudioSource[] Audios;
    }

    [Serializable]
    public sealed class SoundInfo
    {
        [field: SerializeField] public SoundType Type { get; private set; }
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
        [field: SerializeField] public int AudioIndex { get; private set; }
        [field: Range(0f, 1f), SerializeField] public float Volume { get; private set; }
    }
}