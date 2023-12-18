using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchEnable : MonoBehaviour
{
    public Transform[] platform;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            foreach (Transform t in platform)
            {
                t.gameObject.SetActive(true);
            }
        }
    }
}
