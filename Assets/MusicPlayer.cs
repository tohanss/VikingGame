using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void startGame (float fadeTime)
    {
        StartCoroutine(fadeMusic(0.5f));
    } 
    
    IEnumerator fadeMusic (float fadeTime) 
    {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }

        audioSource.clip = backgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.pitch = 0.6f;
        audioSource.loop = true;
        audioSource.Play();
    }
}
