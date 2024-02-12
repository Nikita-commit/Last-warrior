using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    AudioSource source;
    public AudioClip thorn;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void sound()
    {
        source.clip = thorn;
        source.Play();
    }
}
