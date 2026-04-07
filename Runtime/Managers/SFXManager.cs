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
            if (!soundDictionary.ContainsKey(s.id) && s.clip != null)
                soundDictionary.Add(s.id, s.clip);
        }
    }

    // ================= BASIC PLAY =================

    public void Play(string id)
    {
        if (!soundDictionary.ContainsKey(id)) return;

        sfxSource.clip= soundDictionary[id];
        sfxSource.Play();
    }
    public void Stop()
    {
        sfxSource.Stop();
    }

    public void PlayUI(string id)
    {
        if (!soundDictionary.ContainsKey(id)) return;

        uiSource.PlayOneShot(soundDictionary[id]);
    }

    // ================= ADVANCED =================

    public void PlayPitch(string id, float minPitch, float maxPitch)
    {
        if (!soundDictionary.ContainsKey(id)) return;

        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(soundDictionary[id]);
        sfxSource.pitch = 1f;
    }

    public void SetSFXVolume(float v)
    {
        sfxSource.volume = v;
    }

    public void SetUIVolume(float v)
    {
        uiSource.volume = v;
    }
}