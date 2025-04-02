using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;
    public static SFX_Manager Instance;

    // Load sounds into the library
    public void LoadSounds()
    {
        soundLibrary.Clear();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (AudioClip clip in clips)
        {
            soundLibrary[clip.name] = clip;
        }
    }

    // Play a single sound
    public void PlaySound(string soundName, float volume = 1.0f, bool loop = false)
    {
        if (soundLibrary.ContainsKey(soundName))
        {
            audioSource.clip = soundLibrary[soundName];
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found in the library!");
        }
    }

    // Play a random sound from a list of sound names
    public void PlayRandomSound(string[] soundNames, float volume = 1.0f, bool loop = false)
    {
        if (soundNames.Length == 0)
        {
            Debug.LogWarning("No sounds provided to play.");
            return;
        }

        // Choose a random index from the soundNames array
        string randomSoundName = soundNames[Random.Range(0, soundNames.Length)];
        PlaySound(randomSoundName, volume, loop);
    }

    // Stop the current sound
    public void StopSound()
    {
        audioSource.Stop();
    }

    // Adjust the global volume
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Initialize the audio source
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            LoadSounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}