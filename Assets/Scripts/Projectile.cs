using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    public float damage = 50;

    private void Update()
    {
        transform.position -= transform.right* speed * Time.deltaTime;
    }
}
