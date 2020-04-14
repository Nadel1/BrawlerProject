using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{

    public GameObject basicProjectile;
    private Quaternion spawnRotation;
    private Transform spawnPoint;
  
    void Start()
    {
        spawnRotation = new Quaternion(0,0,0,1);
        spawnPoint = GameObject.Find("ProjectileSpawn").transform;
    }
    void Update()
    {
            if (Input.GetMouseButtonDown(0)) // not nice, always use GetButton
            {
                Vector3 position = new Vector3(transform.position.x , transform.position.y, transform.position.z);
                

                
                Instantiate(basicProjectile, spawnPoint.position, spawnPoint.rotation );
            }
        
    }
}
