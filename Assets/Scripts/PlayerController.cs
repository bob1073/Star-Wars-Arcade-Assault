using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Objetos
    public GameObject shootRebelde;
    public GameObject shootImperio;
    public GameObject explosion;
    public GameObject shield;
    public LevelProgression levelProgression;
    public AudioClip rebeldeSound;
    public AudioClip imperioSound;
    public AudioClip powerUpSound;
    private SpawnManager spawnManager;
    private Game game;

    // Componentes
    Animator playerAnim;
    Animator shieldAnim;
    public AudioSource playerAudio;
    Rigidbody2D playerRb;


    // Variables
    public bool faction;//true Rebelde   false Imperio
    public bool isAlive;
    public short lifes; //Vidas del jugador
    public bool allowMovement;
    public float speed;

    [SerializeField]
    Vector3 offsetRebelde, offsetImperio;
    [SerializeField]
    float fireRateWing;
    [SerializeField]
    float fireRateTie;

    public float maxY;

    Vector2 direction;
    bool allowFire = true;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody2D>();
        spawnManager = FindObjectOfType<SpawnManager>();
        shieldAnim = shield.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("faction") == 1) faction = true;
        else faction = false;
        game = FindObjectOfType<Game>();
        allowMovement = false;

        // Cambiamos animación según el bando
        if (faction)
        {
            playerAnim.SetBool("faction", true);

        }
        else
        {
            playerAnim.SetBool("faction", false);
            transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
        }

        isAlive = true;
        lifes = 7;
        
        


    }

    void Update()
    {

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Disparo
        if (Input.GetKey(KeyCode.Space) && allowFire)
        {
            StartCoroutine("FireRate");
        }

        //Al entrar la nave en escena dejamos libertad en el movimeinto
        if (transform.position.y >= -1.5 && !allowMovement) StartCoroutine("StartSpawn");
        
    }

    private void FixedUpdate()
    { 
        //Entrada de la nave
        if (!allowMovement)
        {
            FirstMove();
            

        } else

        {
            // Movimiento con físicas
            playerRb.velocity = direction * levelProgression.playerSpeed * Time.deltaTime * 10.0f;

            // Restringimos los limites del escenario
            playerRb.position = new Vector2(Mathf.Clamp(playerRb.position.x, -2.5f, 2.5f), Mathf.Clamp(playerRb.position.y, -1.75f, maxY));

        }
    }

    private void FirstMove()
    {
        playerRb.velocity = new Vector3(0, speed * Time.deltaTime * 5,0);
    
    }



    // Colisiones
    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.CompareTag("SmallEnemyShoot") || other.gameObject.CompareTag("BigEnemyShoot"))
        {
            short dmg = 1;
            if (other.gameObject.CompareTag("BigEnemyShoot")) dmg = 2;

            Damage(dmg, other);
            game.CheckLifes(lifes);
        }
        //Impacto con otra nave
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(other.gameObject);
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Damage(4, other);
            game.CheckLifes(lifes);

        }


    }

   

    private void Damage( short dmg, Collider2D other)
    {
        lifes -= dmg;
        Destroy(other.gameObject);

        if(lifes <= 0)
        {
            isAlive = false;
           
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(lifes == 1)
        {
            StartCoroutine(ShowShield());
            shieldAnim.SetBool("destroyed", true);
            
        }
        else
        {
            StartCoroutine(ShowShield());
            shieldAnim.SetBool("destroyed", false);
        }
    }

    


    // Corutinas
    IEnumerator FireRate()
    {
        allowFire = false;

        // Si es alianza rebelde dispara dos proyectiles a la vez
        if(faction)
        {
            Instantiate(shootRebelde, transform.position + offsetRebelde, Quaternion.identity);
            Instantiate(shootRebelde, transform.position + new Vector3(-offsetRebelde.x, offsetRebelde.y, 0.0f), Quaternion.identity);
            playerAudio.PlayOneShot(rebeldeSound, 0.5f);

            yield return new WaitForSeconds(fireRateWing);
        }
        // Si es imperio dispara en ráfagas de 3 disparos
        else
        {
            Instantiate(shootImperio, transform.position + offsetImperio, Quaternion.identity);
            playerAudio.PlayOneShot(imperioSound, 0.5f);
            yield return new WaitForSeconds(0.1f);
            Instantiate(shootImperio, transform.position + offsetImperio, Quaternion.identity);
            playerAudio.PlayOneShot(imperioSound, 0.5f);
            yield return new WaitForSeconds(0.1f);
            Instantiate(shootImperio, transform.position + offsetImperio, Quaternion.identity);
            playerAudio.PlayOneShot(imperioSound, 0.5f);

            yield return new WaitForSeconds(fireRateTie);
        }
        allowFire = true;
    }
    IEnumerator ShowShield()
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shield.SetActive(false);
    }
    IEnumerator StartSpawn()
    {
        allowMovement = true;
        game.levelText.gameObject.SetActive(true);
        game.scoreText.gameObject.SetActive(true);
        game.CheckLifes(lifes);
        yield return new WaitForSeconds(2);
        spawnManager.canSpawn = true;
        

    }


}
