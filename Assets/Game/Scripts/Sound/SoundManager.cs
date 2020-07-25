using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Audio
{
    public class SoundManager : Singleton<SoundManager>
    {
        string resourcePath = "Sounds";
        Sound[] sounds;

        GameObject oneShotGameObject;
        AudioSource oneShotAudioSource;

        GameObject musicGameObject;
        AudioSource musicAudioSource;
        int musicIndex;
        Dictionary<string, int> playedSounds;
        List<Tuple<string, int>> randomSounds;
        bool inited;

        /// <summary>
        /// Only Load sounds in path and clear memory
        /// </summary>
        /// <param name="resourcePath"></param>
        public void Load(string resourcePath)
        {
            sounds = null;
            sounds = Resources.LoadAll<Sound>(resourcePath);
            inited = true;
            Resources.UnloadUnusedAssets();
        }
        public override void Awake()
        {
            base.Awake();
            playedSounds = new Dictionary<string, int>();
            randomSounds = new List<Tuple<string, int>>();

            if (!inited)
                sounds = Resources.LoadAll<Sound>(resourcePath);
        }
        public void PlaySound(Sound sound)
        {
            PlaySound(sound.name);
        }
        public void PlayMusic(Sound sound)
        {
            PlayMusic(sound.name);
        }

        public void PlaySound(string name)
        {
            if (!GetSoundStatus()) return;

            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            var sound = GetAudioClip(name);
            if (sound != null)
            {
                oneShotAudioSource.volume = sound.volume;
                if (sound.mixer) oneShotAudioSource.outputAudioMixerGroup = sound.mixer;

                AudioClip clip;
                if (sound.randomPlay)
                    clip = GetClipByRandom(sound);
                else
                {
                    clip = GetClipByIndex(sound);
                }

                oneShotAudioSource.PlayOneShot(clip);
            }
        }
        public void PlaySound(string name, Vector3 position)
        {
            if (!GetSoundStatus()) return;

            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            var sound = GetAudioClip(name);
            if (sound != null)
            {
                audioSource.volume = sound.volume;
                if (sound.mixer) audioSource.outputAudioMixerGroup = sound.mixer;

                if(sound.randomPlay)
                    audioSource.clip = GetClipByRandom(sound);
                else
                {
                    audioSource.clip = GetClipByIndex(sound);
                }

                audioSource.Play();

                Destroy(soundGameObject, audioSource.clip.length);
            }
        }

        public void PlayMusic(string name)
        {
            if (!GetMusicStatus()) return;

            if (musicGameObject == null)
            {
                musicGameObject = new GameObject("Music");
                musicAudioSource = musicGameObject.AddComponent<AudioSource>();
            }

            var sound = GetAudioClip(name);
            if (sound != null)
            {
                musicAudioSource.volume = sound.volume;
                if (sound.mixer) musicAudioSource.outputAudioMixerGroup = sound.mixer;

                if(sound.randomPlay)
                    musicAudioSource.clip = GetClipByRandom(sound);
                else
                {
                    musicAudioSource.clip = GetClipByIndex(sound);
                }

                musicAudioSource.Play();

                StartCoroutine(CheckMusic(sound));
            }
        }

        public void StopMusic()
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.Stop();
                StopAllCoroutines();
            }
        }

        IEnumerator CheckMusic(Sound sound)
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (!musicAudioSource.isPlaying)
                {
                    if (musicIndex < sound.clips.Length - 1) musicIndex++;
                    else musicIndex = 0;

                    musicAudioSource.clip = sound.clips[musicIndex];
                    musicAudioSource.Play();
                }
            }
        }

        public bool GetSoundStatus()
        {
            return PlayerPrefs.GetInt("SoundStatus", 1).IntToBool();
        }
        public bool GetMusicStatus()
        {
            return PlayerPrefs.GetInt("MusicStatus", 1).IntToBool();
        }
        public void SoundStatus(bool status)
        {
            PlayerPrefs.SetInt("SoundStatus", status.BoolToInt());
        }
        public void MusicStatus(bool status)
        {
            if (!status) StopMusic();
            PlayerPrefs.SetInt("MusicStatus", status.BoolToInt());
        }

        public Sound GetAudioClip(string name)
        {
            var sound = sounds.Where(x => x.name == name);
            if (sound.Any())
            {
                var s = sound.First();
                return s;
            }
            Debug.LogError("Sound" + name + " Not Found!");
            return null;
        }

        AudioClip GetClipByRandom(Sound sound)
        {
            int index = 0;
            if (!randomSounds.Where(x=>x.Item1 == sound.assetName).Any())
            {
                index = UnityEngine.Random.Range(0, sound.clips.Length);
                randomSounds.Add(Tuple.Create(sound.assetName, index));
            }
            else
            {
                var items = randomSounds.Where(x => x.Item1 == sound.assetName);
                if (items.Count() == sound.clips.Length)
                {
                    var sounds = items.AsEnumerable().ToList();
                    foreach (var item in sounds)
                    {
                        randomSounds.Remove(item);
                    }
                    Debug.Log(sound.assetName + " random sounds Cleaned.");
                }

                index = UnityEngine.Random.Range(0, sound.clips.Length);
                while (randomSounds.Where(x => x.Item1 == sound.assetName && x.Item2 == index).Any())
                {
                    index = UnityEngine.Random.Range(0, sound.clips.Length);
                }
                randomSounds.Add(Tuple.Create(sound.assetName, index));
            }

            var clip = sound.clips[index];
            return clip;
        }

        AudioClip GetClipByIndex(Sound sound)
        {
            if (!playedSounds.ContainsKey(sound.assetName))
                playedSounds.Add(sound.assetName, 0);

            int index = playedSounds[sound.assetName];
            var clip = sound.clips[index];

            if (index < sound.clips.Length - 1) index++;
            else index = 0;
            playedSounds[sound.assetName] = index;
            return clip;
        }
    }
}
