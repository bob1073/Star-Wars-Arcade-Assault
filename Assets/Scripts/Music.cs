using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private PlayerController player;
    private AudioSource music;

    public AudioClip rebelionSong;
    public AudioClip imperioSong;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        music = GetComponent<AudioSource>();
        if(player.faction)
        {
            music.clip = rebelionSong;
        }
        else
        {
            music.clip = imperioSong;
        }

        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
