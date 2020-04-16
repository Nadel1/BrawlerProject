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

    //used in order to make closer explosions more powerful than further away ones
    public float explosionRange = 45;
    //campls the stress so that no single explosion can be the maximum
    public float maximumStress = 0.6f;


    //used for the flashing effect
    private Color originalColor;
    public Color flashColor;
    public float flashTime;
    public float flashSpeed;

    //random value that determines for how many frames the enemy will be moved
    private int movingFrameAmount;


    //calculated stress value that is sent to the camera to make it rumble
    private float stress;
    //used to calculate the distance of the enemy to the player which influences the intensity of the rumble
    private float distanceToPlayer;
    private float distance;
    //camera gameobject (object that will be rumbled)
    private GameObject target;
    private bool isCooling;
    private int seed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        isRunning = false;
        isCooling = false;
        target = GameObject.FindGameObjectWithTag("MainCamera");
        originalColor = GetComponent<MeshRenderer>().material.color;
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
                //distance calcultation to the player
                distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
                distance = Mathf.Clamp01(distanceToPlayer / explosionRange);

                //calculate the actual stress factor that should be sent to the camera
                stress = (1 - Mathf.Pow(distance, 2) * maximumStress);

                //rumble the camera
                target.GetComponent<RumbleEffect>().induceStress(stress);

                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(Flash());
                //camera should also rumble when enemy is hit but not killed, the rumble should be smaller then
                //distance calcultation to the player
                distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
                distance = Mathf.Clamp01(distanceToPlayer / explosionRange);

                //calculate the actual stress factor that should be sent to the camera
                stress = (1 - Mathf.Pow(distance, 2) * maximumStress);

                //rumble the camera
                target.GetComponent<RumbleEffect>().induceStress(stress * 0.3f);
            }
        }
    }
    IEnumerator Flash()
    {
        float flashingFor = 0;
        var newColor = flashColor;

        while (flashingFor < flashTime)
        {
            this.GetComponent<MeshRenderer>().material.color = newColor;
            flashingFor += Time.deltaTime;
            yield return new WaitForSeconds(flashSpeed);
            flashingFor += flashSpeed;
            if (newColor == flashColor)
            {
                newColor = originalColor;
            }
            else
            {
                newColor = flashColor;
            }
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
    }
}
