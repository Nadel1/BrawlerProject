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

    private Transform Player;
    //boolean to block the start of a new attack while cooling down
    private bool isCooling;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        isCooling = false;
    }

    // Update is called once per frame
    void Update()
    {
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
}
