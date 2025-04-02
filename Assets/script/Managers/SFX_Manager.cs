using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public static SFX_Manager Instance;
    private AudioSource audioSource;
    
    [System.Serializable]
    public class SoundEntry
    {
        public string soundName;
        public AudioClip clip;
    }
    
    public List<SoundEntry> soundEntries = new List<SoundEntry>();
    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

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

    // Load sounds from Inspector references
    public void LoadSounds()
    {
        soundLibrary.Clear();
        
        foreach (SoundEntry entry in soundEntries)
        {
            if (entry.clip != null && !string.IsNullOrEmpty(entry.soundName))
            {
                soundLibrary[entry.soundName] = entry.clip;
                Debug.Log($"Loaded sound: {entry.soundName}");
            }
        }
    }

    public void PlaySound(string soundName, float volume = 1.0f, bool loop = false)
    {
        if (soundLibrary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found in the library!");
        }
    }

    public void PlayRandomSound(string[] soundNames, float volume = 1.0f, bool loop = false)
    {
        if (soundNames.Length == 0)
        {
            Debug.LogWarning("No sounds provided to play.");
            return;
        }

        string randomSoundName = soundNames[Random.Range(0, soundNames.Length)];
        PlaySound(randomSoundName, volume, loop);
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
