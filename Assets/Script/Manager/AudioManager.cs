using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
        if(background != null) 
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();

        }
    }

    public void EffectPlay(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }
}
