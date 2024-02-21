using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] AudioClip bgm;
    [SerializeField] float musicVolume = 0.02f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartGame() {
        audioSourceMusic.clip = bgm;
        audioSourceMusic.volume = musicVolume;
        audioSourceMusic.Play();
    }
}
