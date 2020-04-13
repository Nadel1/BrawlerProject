using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public float health = 100;
    public float speed = 1;

    //when distance between enemy and player is under this range, the enemy can attack, could also be done with a bigger collision box of the player
    public float range = 3;

    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);

        //distance is bigger than range, enemy has to be moved
        if (Vector3.Distance(transform.position, Player.position) >= range)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
