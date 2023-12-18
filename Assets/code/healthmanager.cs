using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthmanager : MonoBehaviour
{
    public Image healthBar;

    public float healthAmount = 100f;

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        
    }

    public void takeDamage(float dmg)
    {
        healthAmount -= dmg;
        healthBar.transform.localScale = new Vector3(healthAmount / 100f, 1, 1);
    }

    public void heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        
        healthBar.fillAmount = healthAmount / 100f;
    }
}
