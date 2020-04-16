using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public float health = 100;
    //damage that the enemy will do to the player
    public float damage = 30;
    //cooldown time between two attacks
    public float coolDown = 2;
    //movement speed of the enemy
    public float speed = 1;

    //when distance between enemy and player is under this range, the enemy can attack, could also be done with a bigger collision box of the player
    public float range = 3;

    //used in order to make closer explosions more powerful than further away ones
    public float explosionRange = 45;
    //campls the stress so that no single explosion can be the maximum
    public float maximumStress = 0.6f;

    //used for the flashing effect
    private Color originalColor;
    public Color flashColor;
    public float flashTime;
    public float flashSpeed;

    private Transform Player;
    //boolean to block the start of a new attack while cooling down
    private bool isCooling;

    //calculated stress value that is sent to the camera to make it rumble
    private float stress;
    //used to calculate the distance of the enemy to the player which influences the intensity of the rumble
    private float distanceToPlayer;
    private float distance;
    //camera gameobject (object that will be rumbled)
    private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        isCooling = false;
        target = GameObject.FindGameObjectWithTag("MainCamera");
        originalColor = GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        new Quaternion(0, 0, 0, 1);
        if (Player != null)
        {
            transform.LookAt(Player);

            //distance is bigger than range, enemy has to be moved
            if (Vector3.Distance(transform.position, Player.position) >= range)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else
            {
                if (!isCooling)
                {
                    //block the start of a new attack
                    isCooling = true;
                    //enemy is in the range of the player can therefore attack
                    Player.gameObject.GetComponent<PlayerBehavior>().TakeDamage(damage);
                    //start the cooldown coroutine
                    StartCoroutine(WaitBetweenAttacks());
                }

            }
        }
        
    }

    IEnumerator WaitBetweenAttacks()
    { 
        yield return new WaitForSeconds(coolDown);
        //unblock next attack
        isCooling = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            health -= other.gameObject.GetComponent<Projectile>().damage;
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
                target.GetComponent<RumbleEffect>().induceStress(stress*0.3f);
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
