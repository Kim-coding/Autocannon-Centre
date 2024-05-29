using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private int poolSize = 10;
    private List<AudioSource> audioSourcePool;
    private int currentIndex = 0;
    
    public AudioSource musicSource;
    public AudioClip background;
    public AudioClip selectedSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Add(source);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.clip = background;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void EffectPlay(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void EffectStop()
    {
        foreach (var source in audioSourcePool)
        {
            source.Stop();
        }
    }

    public void SelectedSoundPlay()
    {
        EffectPlay(selectedSound);
    }

    private AudioSource GetAvailableAudioSource()
    {
        for (int i = 0; i < audioSourcePool.Count; i++)
        {
            int index = (currentIndex + i) % poolSize;
            if (!audioSourcePool[index].isPlaying)
            {
                currentIndex = index;
                return audioSourcePool[index];
            }
        }
        return null;
    }

    public void SoundOnOff(bool onOff)
    {
        musicSource.volume = onOff ? 0.6f : 0;
    }

    public void EffectSoundOnOff(bool onOff)
    {
        foreach (var source in audioSourcePool)
        {
            source.volume = onOff ? 0.7f : 0;
        }
    }
}