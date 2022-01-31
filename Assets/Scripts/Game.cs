using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public LevelProgression levelProgression;
    public Text levelText;
    public Text scoreText;
    public Text deathText;
    public Button exitButton;
    public List<GameObject> shields;

    public Sprite fullShield;
    public Sprite halfShield;

    
    public static int score;

    SpawnManager spawnManager;
    PlayerController player;

    SpriteRenderer currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        player = FindObjectOfType<PlayerController>();
        score = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(levelProgression.level == 0)
        {
            levelText.text = "Preparate...";
        }
        else
        {
            levelText.text = "Nivel: " + levelProgression.level;
        }
        
        scoreText.text = "Puntos: " + score;
        
        if(!player.isAlive)
        {
            deathText.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);

            deathText.text = "Has muerto...\nPuntuacion final: " + score;
        }
    }

    public void CheckLifes(int lifes)
    {

        if (!player.isAlive)
        {
            for(int i = 0; i < 5; i++)
            {
                shields[i].SetActive(false);
            }
        }
        if(lifes == 1)
        {
            shields[0].SetActive(false);
            shields[1].SetActive(false);
        }
        else
        {
            currentSprite = shields[lifes / 2 - 1].gameObject.GetComponent<SpriteRenderer>();
            for(int i=5; i>lifes/2 -1; i--)
            {
                shields[i].SetActive(false);

                if (i > 3)
                {
                    shields[5 - i].SetActive(true);
                    shields[5 - i + 1].gameObject.GetComponent<SpriteRenderer>().sprite = fullShield;
                }

            }
            for(int i=0; i<lifes/2-1; i++) shields[i].gameObject.GetComponent<SpriteRenderer>().sprite = fullShield;
            shields[lifes / 2].SetActive(false);
            currentSprite.gameObject.SetActive(true);
            if (lifes % 2 == 0)
            {
                currentSprite.sprite = halfShield;
            } else
            {
                currentSprite.sprite = fullShield;
            }

        }   
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
