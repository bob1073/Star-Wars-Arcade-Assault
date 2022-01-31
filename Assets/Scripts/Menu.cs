using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject FactionSelector;
    public GameObject music;
    public GameObject smallEnemy;
    public GameObject bigEnemy;
    public GameObject lightSaber;
    public GameObject R2D2;

    public Sprite smallEnemyImperio;
    public Sprite bigEnemyImperio;
    public Sprite smallEnemyRebelde;
    public Sprite bigEnemyRebelde;

    public AudioClip startSong;
    public AudioClip menuSong;
    public AudioClip lightsaberOn;
    public AudioClip lightsaberOff;
    public AudioClip R2D2Up;
    public AudioClip R2D2Down;
    public AudioClip vaderBreath;

    private AudioSource menuMusic;

    // Start is called before the first frame update
    void Start()
    {
        menuMusic = music.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        Application.Quit();

    }
    public void Selector()
    {
        FactionSelector.SetActive(true);
        gameObject.SetActive(false);
        menuMusic.Stop();
        menuMusic.clip = menuSong;
        menuMusic.Play();
        menuMusic.volume = 0.1f;
        
    }
   
    public void JoinRebellion()
    {
        PlayerPrefs.SetInt("faction", 1);
        SceneManager.LoadScene("Game");
        smallEnemy.GetComponent<SpriteRenderer>().sprite = smallEnemyImperio;
        bigEnemy.GetComponent<SpriteRenderer>().sprite = bigEnemyImperio;
    }

    public void JoinEmpire()
    {
        PlayerPrefs.SetInt("faction", 0);
        SceneManager.LoadScene("Game");
        smallEnemy.GetComponent<SpriteRenderer>().sprite = smallEnemyRebelde;
        bigEnemy.GetComponent<SpriteRenderer>().sprite = bigEnemyRebelde;
    }

    public void ShowLightSaber()
    {
        menuMusic.PlayOneShot(vaderBreath, 0.75f);
        lightSaber.GetComponent<Animator>().SetBool("on", true);
        menuMusic.PlayOneShot(lightsaberOn, 1.0f);
        
    }

    public void HideLightSaber()
    {
        
            
        lightSaber.GetComponent<Animator>().SetBool("on", false);
        menuMusic.PlayOneShot(lightsaberOff, 1.0f);

    }


    public void ShowR2D2()
    {
        R2D2.GetComponent<Animator>().SetBool("show", true);
        menuMusic.PlayOneShot(R2D2Up, 1.0f);
    }

    public void HideR2D2()
    {
        R2D2.GetComponent<Animator>().SetBool("show", false);
        menuMusic.PlayOneShot(R2D2Down, 1.0f);
    }

   
}