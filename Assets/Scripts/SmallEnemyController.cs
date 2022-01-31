using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyController : MonoBehaviour
{
    // Objetos
    public GameObject shootRebelde;
    public GameObject shootImperio;
    public GameObject explosion;
    public LevelProgression levelProgression;

    public AudioClip tieSound;
    public AudioClip wingSound;

    // Componentes
    private AudioSource soundEffects;
    private PlayerController playerScript;
    private BoxCollider2D enemyCollider;
    private Animator enemyAnim;
    

    // Variables
    [SerializeField]
    Vector3 offsetRebelde, offsetImperio;

    public int numDisparosRebelde, numDisparosImperio;
    float fireCooldown;

    float angle;
    
    Vector3 centre;

    bool allowFire = true;
    bool alternate = true;
    

    
    void Start()
    {
        
        soundEffects = GetComponent<AudioSource>();
        playerScript = FindObjectOfType<PlayerController>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyAnim = GetComponent<Animator>();


        angle = 0;
        
        
        //Si el enemigo es el imperio cambiamos colider de tamaño y posición para ajustarlo a la nave
        if(playerScript.faction)
        {
            enemyCollider.offset = new Vector2(0.000678837f, 0.009561479f);
            enemyCollider.size = new Vector2(0.3004146f, 0.1777444f);
            enemyAnim.SetBool("faction", false);
        }

        centre = transform.position;
    }

    void Update()
    {
        // Dispara todo el rato
        if(allowFire)
        {
            fireCooldown = Random.Range(levelProgression.smallFireCooldown - levelProgression.smallFireCooldown / 2.0f, levelProgression.smallFireCooldown + levelProgression.smallFireCooldown / 2.0f);
            
            StartCoroutine("FireRate");
        }

        if(levelProgression.phase == 1)
        {
            MovementPhase1();
        }
        else if(levelProgression.phase == 2)
        {
            MovementPhase2();
        }
        
        // Eliminamos la nave al salir del escenario
        if (transform.position.y > 6.0f || transform.position.y < -4.0f) Destroy(gameObject);
    }

    // Colisiones
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerShoot"))
        {
            Game.score += 10;
            SpawnManager.levelScore += 10;
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
            

        }

        if (other.gameObject.CompareTag("Player"))
        {
            SpawnManager.levelScore += 10;
            Game.score += 10;
        }
        /*if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }*/
    }

    void MovementPhase1()
    {
        // Movemos la nave con MRU
        transform.position += new Vector3(0.0f, -levelProgression.smallSpeed * Time.deltaTime, 0.0f);
    }

    void MovementPhase2()
    {

        // Movemos la nave con MRU
        // PENDIENTE DE OPTIMIZAR
        angle += levelProgression.oscilationSpeed * Time.deltaTime;
        Debug.Log(angle);
        centre += new Vector3(0.0f, -levelProgression.smallSpeed * Time.deltaTime, 0.0f);
        transform.position = centre + new Vector3(0.5f*Mathf.Sin(angle), 0.0f, 0.0f);
    }

    //Coroutinas
    IEnumerator FireRate()
    {
        allowFire = false;

        yield return new WaitForSeconds(fireCooldown);
        // Si el enemigo es la alianza dispara dos proyectiles alternados
        if (!playerScript.faction)
        {
            

            for (int i=0; i<numDisparosRebelde; i++)
            {
                if (alternate) Instantiate(shootRebelde, transform.position + offsetRebelde, Quaternion.identity);
                else Instantiate(shootRebelde, transform.position + new Vector3(-offsetRebelde.x, offsetRebelde.y, 0.0f), Quaternion.identity);
                alternate = !alternate;
                soundEffects.PlayOneShot(wingSound, 0.5f);
                yield return new WaitForSeconds(levelProgression.smallFireRate);
            }
            

            
        }
        // Si el enemigo es el imperio dispara dos proyectiles a la vez
        else
        {
            

            for (int i=0; i<numDisparosImperio; i++)
            {
                if (alternate) Instantiate(shootImperio, transform.position + offsetImperio, Quaternion.identity);
                else Instantiate(shootImperio, transform.position + new Vector3(-offsetImperio.x, offsetImperio.y, 0.0f), Quaternion.identity);
                alternate = !alternate;
                soundEffects.PlayOneShot(tieSound, 0.5f);
                yield return new WaitForSeconds(levelProgression.smallFireRate);
            }

            
        }
        allowFire = true;
    }
}
