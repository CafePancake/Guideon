using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}
    public AudioSource _audio;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        _audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
}
