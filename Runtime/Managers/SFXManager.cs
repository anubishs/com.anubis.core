using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [System.Serializable]
    public class Sound
    {
        public string id;
        public AudioClip clip;
    }

    [Header("Audio Source")]
    public AudioSource sfxSource;
    public AudioSource uiSource;
    public AudioSource ambientMusic;

    [Header("Clips")]
    public List<Sound> sounds = new List<Sound>();

    Dictionary<string, AudioClip> soundDictionary;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (Sound s in sounds)
        {
            if (!string.IsNullOrEmpty(s.id) && !soundDictionary.ContainsKey(s.id) && s.clip != null)
                soundDictionary.Add(s.id, s.clip);
        }
    }

    public void Play(string id)
    {
        if (!soundDictionary.ContainsKey(id) || sfxSource == null) return;

        sfxSource.clip = soundDictionary[id];
        sfxSource.Play();
    }

    public void Stop()
    {
        if (sfxSource == null) return;
        sfxSource.Stop();
    }

    public void PlayUI(string id)
    {
        if (!soundDictionary.ContainsKey(id) || uiSource == null) return;

        uiSource.PlayOneShot(soundDictionary[id]);
    }

    public void PlayPitch(string id, float minPitch, float maxPitch)
    {
        if (!soundDictionary.ContainsKey(id) || sfxSource == null) return;

        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(soundDictionary[id]);
        sfxSource.pitch = 1f;
    }

    public void SetSFXVolume(float v)
    {
        if (sfxSource == null) return;
        sfxSource.volume = v;
    }

    public void SetUIVolume(float v)
    {
        if (uiSource == null) return;
        uiSource.volume = v;
    }
}
