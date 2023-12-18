using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlayer : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject Object_spawnPoint;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Application.LoadLevel(Application.loadedLevel);
            print("collided");
        }
        else if (col.gameObject.CompareTag("object"))
        {
            col.transform.position = Object_spawnPoint.transform.position;
            print("object collided");
        }
    }
    
}
