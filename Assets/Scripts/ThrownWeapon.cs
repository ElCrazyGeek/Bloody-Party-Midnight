using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public int damage = 2;
    public bool isSharp = false;
    public float destroyDelay = 0.05f;
    public LayerMask targetLayers;
    public string targetTag = "Enemy"; // por defecto
    public GameObject pickupPrefab; // Lo que dejará en el suelo al impactar
    private bool canHit = false;    // Permite impacto tras un breve delay
    private float activationDelay = 0.1f; // Tiempo antes de activar colisiones
    private float timer = 0f;



    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;

        if (other.CompareTag(targetTag))
        {
            // dañar al enemigo
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                if (isSharp && eh.IsBackStab(transform))
                    eh.Execute();
                else
                    eh.TakeDamage(damage);
            }

            // luego dejar pickup y destruir este objeto
           Instantiate(pickupPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
    void Update()
{
    timer += Time.deltaTime;
    if (timer >= activationDelay)
        canHit = true;
}

}
