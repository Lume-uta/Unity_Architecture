using UnityEngine;

namespace Core.Sound
{
    public sealed class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        private static GameObject _obj;

        private void Awake()
        {
            if (_instance == null)
            {
                _obj = gameObject;

                _instance = this;
                DontDestroyOnLoad(gameObject);

                Debug.Log($"Initialize: {GetType().Name}");
            }
            else Destroy(gameObject);
        }

        public static void InitSoundData(SoundData[] datas)
        {
            foreach (SoundData data in datas)
            {
                data.Audios = new AudioSource[data.AudioSize];

                for (int i = 0; i < data.Audios.Length; i++)
                {
                    data.Audios[i] = _obj.AddComponent<AudioSource>();

                    data.Audios[i].playOnAwake = false;
                    data.Audios[i].outputAudioMixerGroup = data.Outputs[i];
                }

                Debug.Log($"InitSoundData: {data.name}");
            }
        }

        public static void RemoveSoundData(SoundData[] datas)
        {
            foreach (SoundData data in datas)
            {
                foreach (AudioSource audio in data.Audios) Destroy(audio);

                data.Audios = null;

                Debug.Log($"RemoveSoundData: {data.name}");
            }
        }

        public static bool Execute(SoundData data, SoundType type, bool isStop = false)
        {
            foreach (SoundInfo info in data.SoundInfos)
            {
                if (info.Type != type) continue;

                if (isStop) data.Audios[info.AudioIndex].Stop();
                else
                {
                    data.Audios[info.AudioIndex].clip = info.Clip;
                    data.Audios[info.AudioIndex].volume = info.Volume;
                    data.Audios[info.AudioIndex].loop = info.Loop;

                    data.Audios[info.AudioIndex].Play();
                }

                return true;
            }

            return false;
        }
    }
}