using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Color32 colorHit;
    
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        _spriteRenderer.color = colorHit;

        if (currentHealth <= 0)
        {
            //currentHealth = maxHealth;
            dead();
        }
    }

    void dead()
    {
        Debug.Log("dead");
        
        //enemy is dead
        Destroy(gameObject);
        
        //disable enemy
        this.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
