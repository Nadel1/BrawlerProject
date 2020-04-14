using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody player_Rigidbody;

    public float speed = 15;
    public float health = 100;

    void Start()
    {
        player_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        movePlayer();
       
    }
    
    private void movePlayer()

    {
        float moveLeftRight = Input.GetAxis("Horizontal");
        float moveUpDown = Input.GetAxis("Vertical");

        player_Rigidbody.velocity = new Vector3(moveLeftRight * speed, player_Rigidbody.velocity.y, moveUpDown * speed);
    }
    //method called by the enemy, damage is enemy specific
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
    }
}

