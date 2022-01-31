
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    int level;
    int aux;
   

    int perfectScore;
    public static int levelScore;

    public GameObject smallEnemy;
    public GameObject bigEnemy;
    public GameObject shieldPU;
    public LevelProgression levelProgression;

    public List<Transform> spawnPositions;

    private PlayerController player;
    

    public bool canSpawn;

    

    int numEnemys;
    int numEnemysPerWave;
    float smallEnemyProb;
    float spawnRate;
    float smallSpeed;
    float bigSpeed;
    float oscilationSpeed;
    float smallFireCooldown;
    float bigFireCooldown;
    int phase;
    float playerSpeed;



    // Start is called before the first frame update
    void Start()
    {
        aux = 1;
       
        player = FindObjectOfType<PlayerController>();
        
        canSpawn = false;
        levelScore = -1;
        perfectScore = 0;

        levelProgression.level = level;
        levelProgression.numEnemys = 6;
        levelProgression.numEnemysPerWave = 1;
        levelProgression.smallEnemyProb = 0.7f;
        levelProgression.spawnRate = 3.0f;
        levelProgression.levelTransitionTime = 4.0f;
        levelProgression.smallFireCooldown = 2.5f;
        levelProgression.bigFireCooldown = 1.5f;
        levelProgression.bigFireRate = 0.5f;
        levelProgression.smallFireRate = 0.5f;
        levelProgression.smallSpeed = 0.9f;
        levelProgression.bigSpeed = 0.6f;
        levelProgression.phase = 1;
        levelProgression.oscilationNum = 3;
        levelProgression.oscilationSpeed = 3.0f;
        levelProgression.playerSpeed = player.speed;



        numEnemys = levelProgression.numEnemys;
        numEnemysPerWave = levelProgression.numEnemysPerWave;
        smallEnemyProb = levelProgression.smallEnemyProb;
        spawnRate = levelProgression.spawnRate;
        smallSpeed = levelProgression.smallSpeed;
        bigSpeed = levelProgression.bigSpeed;
        oscilationSpeed = levelProgression.oscilationSpeed;
        smallFireCooldown = levelProgression.smallFireCooldown;
        bigFireCooldown = levelProgression.bigFireCooldown;
        playerSpeed = player.speed;

        phase = levelProgression.phase;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isAlive && canSpawn && player.allowMovement)
        {
            // Cosas antes de la oleada
            UpdateLevelStats();

            //Puntuación pefecta, ganamos power up
            if (perfectScore == levelScore && player.lifes < 11)
            {
                Instantiate(shieldPU, new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-1.5f, 0.5f), 0.0f), Quaternion.identity);
            }

            StartCoroutine(SpawnWave(levelProgression.numEnemys, levelProgression.numEnemysPerWave, levelProgression.smallEnemyProb, levelProgression.spawnRate, levelProgression.levelTransitionTime));

            
            // Cosas despues de la oleada

        }

         
         if (500 * aux <= Game.score && Game.score != 0)
         {
             Instantiate(shieldPU, new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-1.5f, 0.5f), 0.0f), Quaternion.identity);
             aux++;
          }
       
            
           
        
    }

    void UpdateLevelStats()
    {
        levelProgression.level++;
        levelProgression.smallSpeed = smallSpeed + 0.05f * levelProgression.level;
        levelProgression.bigSpeed = bigSpeed + 0.05f * levelProgression.level;
        levelProgression.oscilationSpeed = oscilationSpeed + 0.1f * levelProgression.level;

        if (levelProgression.spawnRate >= 1.0f)
        {
            levelProgression.spawnRate = spawnRate - 0.1f * levelProgression.level;
        }

        levelProgression.numEnemys = numEnemys + levelProgression.level / 5;

        if (levelProgression.numEnemysPerWave <= 3)
        {
            levelProgression.numEnemysPerWave = numEnemysPerWave + levelProgression.level / 15;
        }

        levelProgression.smallFireCooldown = smallFireCooldown - 0.01f * levelProgression.level;
        levelProgression.bigFireCooldown = bigFireCooldown - 0.01f * levelProgression.level;


        if (levelProgression.level % 5 == 0 && levelProgression.level >= 5)
        {
            if (levelProgression.smallEnemyProb >= 0.2f) levelProgression.smallEnemyProb -= 0.1f;  
        }

        levelProgression.playerSpeed = playerSpeed + levelProgression.level / 5;

        if (levelProgression.level >= 10) levelProgression.phase = 2;
    }

    void SpawnEnemys(int num, float smallEnemyProb) 
    {
        bool differentPositions = true;
        int[] spawnIndex = new int[3];

        // Genera posiciones aleatorias
        for (int j = 0; j < num; j++)
        {
            spawnIndex[j] = Random.Range(0, spawnPositions.Count);

        }
        if(num == 2)
        {
            if (spawnIndex[0] == spawnIndex[1]) differentPositions = false;
        }
        if(num == 3)
        {
            if (spawnIndex[0] == spawnIndex[1] || spawnIndex[1] == spawnIndex[2] || spawnIndex[0] == spawnIndex[2]) differentPositions = false;
        }
         
        if(differentPositions)
        {
            // Spawnea los enemigos
            for (int j = 0; j < num; j++)
            {
                float prob = Random.Range(0.0f, 1.0f);

                Transform spawnPos = spawnPositions[spawnIndex[j]];
                if (player.isAlive)
                {
                    if (prob < smallEnemyProb)
                    {

                        Instantiate(smallEnemy, spawnPos.position, Quaternion.identity);
                        perfectScore += 10;
                    }
                    else
                    {
                        Instantiate(bigEnemy, spawnPos.position, Quaternion.identity);
                        perfectScore += 25;
                    }
                }
                else break;
            }
        }
    }
    IEnumerator SpawnWave(int numEnemy, int numEnemysPerWave, float smallEnemyProb, float spawnRate, float transitionTime) // smallEnemyProb - Probabiliad de spawn de nave pequeña 
    {
        
        perfectScore = 0;
        levelScore = 0;
        canSpawn = false;
       
        for (int i=0; i<numEnemy; i++)
        {
            SpawnEnemys(numEnemysPerWave, smallEnemyProb);
            yield return new WaitForSeconds(spawnRate);
        }

        yield return new WaitForSeconds(transitionTime);
        canSpawn = true;
        
    }

}
