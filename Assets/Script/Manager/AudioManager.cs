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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSourcePool = new List<AudioSource>();
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                audioSourcePool.Add(source);
            }

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.clip = background;
            musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
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
        // 모든 소스가 사용 중이면 null 반환
        return null;
    }

    public void SoundOnOff(bool onOff)
    {
        musicSource.volume = onOff ? 0.7f : 0;
    }

    public void EffectSoundOnOff(bool onOff)
    {
        foreach (var source in audioSourcePool)
        {
            source.volume = onOff ? 0.7f : 0;
        }
    }
}