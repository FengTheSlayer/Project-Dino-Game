using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float speed = 5;
    public int startPoint;
    public Transform[] points;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length) //if platform was on last point after increase index
            {
                i = 0; //reset back to 0
            }
        }
        //moves platform based on index of i
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
