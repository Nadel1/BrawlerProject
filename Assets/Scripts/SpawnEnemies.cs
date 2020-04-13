using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    //array storing all the enemy prefabs
    public GameObject[] enemies;
    public float differenceInWaitSpawn=2;

    //holds the amount of enemies that still have to be spawned
    private int needToSpawn;
    //random value that controlls the time between spawning
    private float waitTime;
    //holds amount of enemies that need to be spawned in total
    private int enemyToSpawn;
    //blocks the new spawning while coroutine is running (and waiting)
    private bool isWaiting;
    //random seed used for actual randomness of spawned enemy type
    private int seed;

    // Start is called before the first frame update
    void Start()
    {
        needToSpawn = 0;
        waitTime = 5;
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //spawne enemies while there is still an amount left to be spawned and the coroutine is running 
        if (needToSpawn > 0&&!isWaiting)
        {
            isWaiting = true;
            //initialise the seed
            UnityEngine.Random.InitState(seed);
            //the seed is the time which will always be different for each call, the multiplication and addition ensure that that amount will not be
            //rounded towards zero as it is converted to int
            seed = (int)(Time.unscaledTime * 100000 + 1000);
            //decide a random enemy to spawn
            enemyToSpawn = UnityEngine.Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyToSpawn],transform.position,transform.rotation);
            //decrease the spawn counter
            needToSpawn--;
            StartCoroutine(WaitBetweenSpawn());
        }
    }

    //this method is called by the spawn controller and has to be public
    public void Spawn(int seed,int amount,float averageWaitTime)
    {
        needToSpawn += amount;
        waitTime = averageWaitTime;
        this.seed = seed;
        
    }
    IEnumerator WaitBetweenSpawn()
    {
        isWaiting = true;
        //random seed generation
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        //wait a random amount of time, a different seed is used than in the spawn
        yield return new WaitForSeconds(UnityEngine.Random.Range(waitTime- differenceInWaitSpawn, waitTime+ differenceInWaitSpawn));
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        //unblock the spawning of the next enemy
        isWaiting = false;
    }
}
