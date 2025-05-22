using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    private Weapon currentWeapon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowWeapon();
        }
    }

    void PerformAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                int damage = currentWeapon != null ? currentWeapon.damage : 1;
                eh.TakeDamage(damage);
            }
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        Debug.Log("Arma equipada: " + weapon.weaponName);
    }

    void ThrowWeapon()
    {
        if (currentWeapon == null || currentWeapon.projectilePrefab == null) return;

        // Calcular dirección hacia el mouse
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - attackPoint.position).normalized;

        // Instanciar el arma lanzada
        GameObject thrown = Instantiate(currentWeapon.projectilePrefab, attackPoint.position, Quaternion.identity);
        Rigidbody2D rb = thrown.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * currentWeapon.throwForce;
        }

        // Pasar propiedades al objeto lanzado
        ThrownWeapon tw = thrown.GetComponent<ThrownWeapon>();
        if (tw != null)
        {
            tw.targetTag = "Enemy";
            tw.damage = currentWeapon.damage;
            tw.isSharp = currentWeapon.isSharp;
            tw.pickupPrefab = currentWeapon.pickupPrefab;
        }

        currentWeapon = null;
    }

    public bool TieneArma()
    {
        return currentWeapon != null;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
