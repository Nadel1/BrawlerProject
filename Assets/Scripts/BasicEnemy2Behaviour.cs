using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy2Behaviour : MonoBehaviour
{
    public float health = 100;
    public float speed = 1;

    //when distance between enemy and player is under this range, the enemy can attack, could also be done with a bigger collision box of the player
    public float range = 3;

    private Transform Player;
    //determine wether or not coroutine is running
    private bool isRunning;
    //determine for how many frames the enemy moves before correcting the rotation
    public int averageWaitFrames = 230;
    //random value that determines for how many frames the enemy will be moved
    private int movingFrameAmount;

    private int seed;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (!isRunning)
            StartCoroutine(Run());
    }

   
    IEnumerator Run()
    {
        //block the start of a new coroutine using the boolean variable 
        isRunning = true;
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        //determine a random frame amount for which the enemy will be moved
        movingFrameAmount = Random.Range(averageWaitFrames-30, averageWaitFrames+30);
        for(int i = 0; i < movingFrameAmount; i++)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        yield return new WaitForSeconds(Random.Range(1, 2));//stop enemy
        //correct the rotation so that the enemy is looking at the player
        transform.LookAt(Player);
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        yield return new WaitForSeconds(Random.Range(1, 2));//stop enemy
        
        //unblock the start of a new coroutine
        isRunning = false;
    }
}
