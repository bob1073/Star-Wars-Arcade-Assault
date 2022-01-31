using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyController : MonoBehaviour
{
    // Objetos
    public GameObject shootRebelde;
    public GameObject shootImperio;
    public GameObject explosion;
    public GameObject shield;
    public LevelProgression levelProgression;

    public AudioClip tieSound;
    public AudioClip wingSound;

    // Componentes
    private AudioSource soundEffects;
    private PlayerController playerScript;
    private BoxCollider2D enemyCollider;
    private Animator enemyAnim;
    Animator shieldAnim;
   

    // Variables
    [SerializeField]
    Vector3 offsetRebelde, offsetImperio;

    public int numDisparosRebelde, numDisparosImperio;

    int oscilations;
    float fireCooldown;

    bool allowFire = true;
    int currentDirection = -1;
    int lifes = 2;
    float angle = 0;

    Vector3 centre;
    

    void Start()
    {
        soundEffects = GetComponent<AudioSource>();
        playerScript = FindObjectOfType<PlayerController>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyAnim = GetComponent<Animator>();
        shieldAnim = shield.GetComponent<Animator>();
        
     

        centre = transform.position;

        fireCooldown = levelProgression.bigFireCooldown;
        oscilations = levelProgression.oscilationNum;
        

        //Si el enemigo es el imperio cambiamos colider de tamaño y posición para ajustarlo a la nave
        if (playerScript.faction)
        {
            enemyCollider.offset = new Vector2(0.001780719f, 0.03102493f);
            enemyCollider.size = new Vector2(0.4781188f, 0.3971634f);
            enemyAnim.SetBool("faction", false);
        }

        shieldAnim.SetBool("destroyed", true);
    }

    void Update()
    {
        // Dispara todo el rato
        if (allowFire)
        {
            fireCooldown = Random.Range(levelProgression.bigFireCooldown - levelProgression.bigFireCooldown / 2.0f, levelProgression.bigFireCooldown + levelProgression.bigFireCooldown / 2.0f);
            
            
            StartCoroutine("FireRate");
        }
       
        // Eliminamos la nave al salir del escenario
        if (transform.position.y > 6.0f ) Destroy(gameObject);

        if (levelProgression.phase == 1)
        {
            MovementPhase1();
        }
        else if (levelProgression.phase == 2)
        {
            MovementPhase2();
        }

    }

    // Colisiones
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShoot"))
        {
            lifes--;

            
            StartCoroutine(ShowShield());
            shieldAnim.SetBool("destroyed", true);
            if(lifes == 0)
            {
                Game.score += 25;
                SpawnManager.levelScore += 25;
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(gameObject);
                
            }
        }

        if(other.gameObject.CompareTag("Top"))
        {
            oscilations--;

            if(oscilations == 0) Destroy(gameObject);
            currentDirection = -1;
        }

        if(other.gameObject.CompareTag("Middle"))
        {
            currentDirection = 1;
        }

        if (other.gameObject.CompareTag("Player"))
        {   
            SpawnManager.levelScore += 25;
            Game.score += 25;
        }
        /*if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(other.gameObject);
            Instantiate(explosion, other.transform.position, Quaternion.identity);
        }*/

    }

    void MovementPhase1()
    {
        transform.position += new Vector3(0.0f, currentDirection * levelProgression.bigSpeed * Time.deltaTime, 0.0f);
    }

    void MovementPhase2()
    {

        // Movemos la nave con MRU
        // PENDIENTE DE OPTIMIZAR
        angle += levelProgression.oscilationSpeed * Time.deltaTime;
        Debug.Log(angle);
        centre += new Vector3(0.0f, currentDirection*levelProgression.bigSpeed * Time.deltaTime, 0.0f);
        transform.position = centre + new Vector3(0.5f * Mathf.Sin(angle), 0.0f, 0.0f);
    }

    //Coroutinas
    IEnumerator FireRate()
    {
        allowFire = false;

        yield return new WaitForSeconds(fireCooldown);

        if (!playerScript.faction)
        {
            
            
            for (int i=0; i<numDisparosRebelde; i++)
            {
                Instantiate(shootRebelde, transform.position + offsetRebelde, Quaternion.identity);
                soundEffects.PlayOneShot(wingSound, 0.5f);
                yield return new WaitForSeconds(levelProgression.bigFireRate);
            }
            

        }
        
        else
        {
            
            
            for (int i = 0; i < numDisparosImperio; i++)
            {
                Instantiate(shootImperio, transform.position + offsetImperio, Quaternion.identity);
                soundEffects.PlayOneShot(tieSound, 0.5f);
                yield return new WaitForSeconds(levelProgression.bigFireRate);
            }
            
        }
        allowFire = true;
    }
    IEnumerator ShowShield()
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shield.SetActive(false);
    }
}
