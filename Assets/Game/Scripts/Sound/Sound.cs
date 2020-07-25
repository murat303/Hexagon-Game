using UnityEngine;
using UnityEngine.Audio;

namespace Utilities.Audio
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Scriptables/Sound", order = 2)]
    public class Sound : ScriptableObject
    {
        public string assetName;
        public bool randomPlay;
        public AudioClip[] clips;
        public float volume = 1;
        public AudioMixerGroup mixer;
    }
}
