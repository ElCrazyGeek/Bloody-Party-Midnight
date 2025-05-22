using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Melee Settings")]
    public float meleeRange = 1.5f;
    public int meleeDamage = 1;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    [Header("Ranged Settings")]
    public bool hasWeapon = false;
    public Weapon equippedWeapon;
    public Transform firePoint;

    private float nextAttackTime = 0f;
    private GameObject currentVisual;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, meleeRange, playerLayer);

            if (player != null)
            {
                if (hasWeapon && equippedWeapon != null && equippedWeapon.projectilePrefab != null && firePoint != null)
                {
                    Shoot(player.transform);
                }
                else
                {
                    player.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage);
                    Debug.Log("Enemigo golpeó al jugador");
                }

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Shoot(Transform target)
    {
        GameObject bullet = Instantiate(equippedWeapon.projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = (target.position - firePoint.position).normalized;
            rb.linearVelocity = dir * equippedWeapon.throwForce;
        }

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.targetTag = "Player";
            b.damage = equippedWeapon.damage;
        }

        Debug.Log("Enemigo disparó");
    }

   public void EquippedWeaponFromPickup(Weapon weapon)
{
    equippedWeapon = weapon;
    hasWeapon = true;
    Debug.Log("Enemigo equipado con: " + weapon.weaponName);
}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
