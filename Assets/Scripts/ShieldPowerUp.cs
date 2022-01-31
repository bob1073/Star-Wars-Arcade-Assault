using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShieldPowerUp : MonoBehaviour
{


    public AudioSource ShieldSoundEffect;
    private PlayerController player;
    private Game game;
    [SerializeField]
    private float rotacionSpeed = 10;
    [SerializeField]
    private float radious = 0.1f;
    float angle = 0;
    Vector3 centre;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        game = FindObjectOfType<Game>();
        centre = transform.position;
        ShieldSoundEffect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        angle += rotacionSpeed*Time.deltaTime;
        gameObject.transform.position = centre + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)*radious;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.playerAudio.PlayOneShot(player.powerUpSound, 0.75f); 
            Destroy(gameObject);
            if (player.lifes < 11)
            {
                if (player.lifes == 10) player.lifes++;
                else player.lifes += 2;
            }
            game.CheckLifes(player.lifes);
            
        }
    }
}
