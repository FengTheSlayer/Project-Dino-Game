using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform hitbox;
    public float attackRange = .8f;
    public LayerMask enemyLayers;
    public int attackDamage = 30;
    
    public AudioSource attackSFX;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            attack();
			animator.SetBool("attack hold",true);
            attackSFX.Play();
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            animator.SetBool("attack hold",false);
        }
    }

    void attack()
    {
        animator.SetTrigger("attack");
        
        //enemy detection
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitbox.position, attackRange, enemyLayers);
        
        //damaging enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().takeDamage(attackDamage);
            Debug.Log("target hit" + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitbox == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(hitbox.position, attackRange);
    }
}
