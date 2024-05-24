using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public AudioSource musicSource;
    public AudioSource effectSource;

    public AudioClip background;

    public static AudioManager Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<AudioManager>();
            }

            return m_Instance;
        }
    }
    public static AudioManager m_Instance;

    void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
        effectSource.volume = 0.7f;
    }

    public void EffectPlay(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void SoundOnOff(bool onOff)
    {
        musicSource.volume = onOff ? 0.7f : 0;
    }

    public void EffectSoundOnOff(bool onOff) 
    {
        effectSource.volume = onOff ? 0.7f : 0;
    }
}
