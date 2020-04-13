using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //holds all the spawn objects
    public GameObject[] spawnPoints;
    public int wave = 1;
    //holds the amount of enemies to be spawn in the first wave
    public int spawnAmount = 4;
    //holds the amount of time that will be averagely waited between spawning of each enemy
    public int waitAmount = 4;
    //time between each wave
    public float waitForNextWave = 60;

    private bool isSpawning;
    // Start is called before the first frame update
    void Start()
    {
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        //start the coroutine only if no coroutine is running, ensured by the boolean variable
        if (!isSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        //block next coroutine
        isSpawning = true;
        //iterate over the spawn points 
        for (int i = 0; i < spawnPoints.Length; i++)
        {  
            //call the spawn method on the spawn points with random seed 
            spawnPoints[i].GetComponent<SpawnEnemies>().Spawn((int)(Time.unscaledTime*100000+1000), spawnAmount ,waitAmount);
            //wait between the spawns so that the given seed is not always the same one
            yield return new WaitForSeconds(1);
        }
        //wait for the next wave
        yield return new WaitForSeconds(waitForNextWave);
        //unblock the start of the next coroutine
        isSpawning = false;
        wave++;
    }
}
