using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    private float nextAttackTime = 0f;

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        
        if (player != null && Time.time >= nextAttackTime)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
            Debug.Log("Enemigo golpeó al jugador");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Dibujo en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
