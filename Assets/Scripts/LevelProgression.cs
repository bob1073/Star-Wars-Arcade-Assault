using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class LevelProgression : ScriptableObject
{
    // Start is called before the first frame update
    public int level;
    public int numEnemys;
    public int numEnemysPerWave;
    public float smallEnemyProb;
    public float spawnRate;
    public float levelTransitionTime;

    public float smallFireCooldown;
    public float bigFireCooldown;
    public float smallFireRate;
    public float bigFireRate;
    public float smallSpeed;
    public float bigSpeed;
    public int phase;
    public float oscilationSpeed;
    public int oscilationNum;
    public float playerSpeed;
}
