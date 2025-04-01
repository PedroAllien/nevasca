using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioSource audioSrc; 

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlaySound(AudioClip clip)
    {
        audioSrc.clip = clip;
        audioSrc.Play();
    }
}
