using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy2Behaviour : MonoBehaviour
{
    public float health = 100;
    public float speed = 1;
    public float damage = 30;
    public float coolDown = 1;

    //when distance between enemy and player is under this range, the enemy can attack, could also be done with a bigger collision box of the player
    public float range = 1;

    private Transform Player;
    //determine wether or not coroutine is running
    private bool isRunning;
    //determine for how many frames the enemy moves before correcting the rotation
    public int averageWaitFrames = 230;
    //random value that determines for how many frames the enemy will be moved
    private int movingFrameAmount;

    private bool isCooling;
    private int seed;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        isRunning = false;
        isCooling = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (!isRunning&&Player!=null)
            StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        //block the start of a new coroutine using the boolean variable 
        isRunning = true;

        //seed calculation, necessary for actual randomness
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);

        //determine a random frame amount for which the enemy will be moved
        movingFrameAmount = Random.Range(averageWaitFrames-30, averageWaitFrames+30);

        //move enemy for a random amount of frames
        for(int i = 0; i < movingFrameAmount; i++)
        {
            //only move while not in range
            if(Player!=null&&Vector3.Distance(transform.position, Player.position) >= range)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                if (!isCooling&&Player!=null)
                {
                    //block the start of a new attack
                    isCooling = true;
                    //enemy is in the range of the player can therefore attack
                    Player.gameObject.GetComponent<PlayerBehavior>().TakeDamage(damage);
                    //start the cooldown coroutine
                    yield return new WaitForSeconds(coolDown);
                    //unblock next attack
                    isCooling = false;
                }
            }
            
        }

        //seed calculation, necessary for actual randomness
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        yield return new WaitForSeconds(Random.Range(1, 2));//stop enemy

        //correct the rotation so that the enemy is looking at the player
        transform.LookAt(Player);

        //seed calculation, necessary for actual randomness
        seed = (int)(Time.unscaledTime * 100000 + 1000);
        UnityEngine.Random.InitState(seed);
        yield return new WaitForSeconds(Random.Range(1, 2));//stop enemy
        
        //unblock the start of a new coroutine
        isRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            health-=other.gameObject.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
            if (health < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
