using UnityEngine;
using System.Collections;

public class AutoAttack : MonoBehaviour
{
    public int attackDamage = 2;
    public float attackCooldown = 1.0f;
    private Health_ForTest targetHealth;
    private bool isAttacking = false;

    void Update()
    {
        if (isAttacking && targetHealth != null)
        {
            StartCoroutine(AttackLoop());
        }
    }

    public void StartCombat(Health_ForTest target)
    {
        targetHealth = target;
        isAttacking = true;
    }

    private IEnumerator AttackLoop()
    {
        while (targetHealth != null && !targetHealth.IsDead())
        {
            targetHealth.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackCooldown);
        }

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Player")) && targetHealth == null)
        {
            targetHealth = collision.GetComponent<Health_ForTest>();
            StartCombat(targetHealth);
        }
    }
}
