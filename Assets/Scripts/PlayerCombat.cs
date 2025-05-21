using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public Transform weaponHolder;

    private Weapon currentWeapon;
    private GameObject currentVisual;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Lanzar arma
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
                bool isSharp = currentWeapon != null && currentWeapon.isSharp;

                if (eh.IsVulnerable() || (isSharp && eh.IsBackStab(transform)))
                    eh.Execute();
                else
                    eh.TakeDamage(damage);
            }
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        if (currentVisual) Destroy(currentVisual);

        if (weapon.visualPrefab && weaponHolder)
        {
            currentVisual = Instantiate(weapon.visualPrefab, weaponHolder);
            currentVisual.transform.localPosition = Vector3.zero;
            currentVisual.transform.localRotation = Quaternion.identity;
        }
    }

    void ThrowWeapon()
    {
        if (currentWeapon == null) return;

       GameObject thrown = Instantiate(currentWeapon.projectilePrefab, attackPoint.position, transform.rotation);
Rigidbody2D rb = thrown.GetComponent<Rigidbody2D>();
if (rb != null)
{
    rb.linearVelocity = transform.up * currentWeapon.throwForce;
}

ThrownWeapon tw = thrown.GetComponent<ThrownWeapon>();
if (tw != null)
{
    tw.targetTag = "Enemy"; // solo puede golpear enemigos
}

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
