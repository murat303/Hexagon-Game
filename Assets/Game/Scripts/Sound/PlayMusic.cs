using UnityEngine;

namespace Utilities.Audio
{
    public class PlayMusic : MonoBehaviour
    {
        public bool playOnStart = true;
        public Sound music;

        void Start()
        {
            if(playOnStart) Play();
        }

        public void Play()
        {
            if (music != null) SoundManager.Instance.PlayMusic(music);
        }

        public void Stop()
        {
            if (music != null) SoundManager.Instance.StopMusic();
        }
    }
}
