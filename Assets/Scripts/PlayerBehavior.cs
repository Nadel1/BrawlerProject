﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody player_Rigidbody;

    private float speed = 15;

    void Start()
    {
        player_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        movePlayer();
       
    }
    void movePlayer()
    {
        float moveLeftRight = Input.GetAxis("Horizontal");
        float moveUpDown = Input.GetAxis("Vertical");

        player_Rigidbody.velocity = new Vector3(moveLeftRight * speed, player_Rigidbody.velocity.y, moveUpDown * speed);
    }
}

