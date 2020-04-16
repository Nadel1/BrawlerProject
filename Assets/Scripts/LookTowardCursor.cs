using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardCursor : MonoBehaviour
{// Update is called once per frame
    private Camera cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {

        ////Get the Screen positions of the object
        Vector2 positionOnScreen = cam.WorldToViewportPoint(transform.position);

        ////Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition);

        ////Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        ////Ta Daaa
        transform.rotation = Quaternion.Euler(new Vector3(0f, - angle, 0f ));

     
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
