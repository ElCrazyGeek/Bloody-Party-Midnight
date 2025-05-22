using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Pacifico,
        Alerta,
        Amenaza,
        Busqueda
    }

    public State currentState = State.Pacifico;

    public Transform player;
    public float visionRange = 5f;
    public float attackRange = 1.2f;
    public float visionAngle = 60f;
    public LayerMask playerLayer;

    [Header("Alerta")]
    public float detectionTime = 0.5f;
    private float alertTimer = 0f;
    public float alertCooldown = 1f;

    private float lostPlayerTimer = 0f;
    public float timeBeforeSearch = 1.5f;

    private Vector3 lastKnownPosition;
    private float searchTime = 2f;
    private float searchTimer = 0f;

    [Header("Comportamiento Personalizado")]
    public bool esAgresivo = false;
    public bool esCobarde = false;
    public float distanciaHuida = 6f;

    private EnemyHealth health;
    private PlayerCombat playerCombat;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        if (player != null) playerCombat = player.GetComponent<PlayerCombat>();
    }

    void Update()
    {
       if (health == null) return;

        switch (currentState)
        {
            case State.Pacifico:
                CheckForPlayer();
                break;
            case State.Alerta:
                HandleAlert();
                break;
            case State.Amenaza:
                AttackPlayer();
                break;
            case State.Busqueda:
                SearchArea();
                break;
        }
    }

    void CheckForPlayer()
    {
        if (CanSeePlayer())
        {
            currentState = State.Alerta;
            alertTimer = 0f;
            Debug.Log(name + " está en alerta.");
        }
    }

    void HandleAlert()
    {
        if (CanSeePlayer())
        {
            alertTimer += Time.deltaTime;

            if (alertTimer >= detectionTime)
            {
                currentState = State.Amenaza;
                Debug.Log(name + " confirmó amenaza.");
            }
        }
        else
        {
            alertTimer -= Time.deltaTime * alertCooldown;

            if (alertTimer <= 0f)
            {
                currentState = State.Busqueda;
                lastKnownPosition = player.position;
                searchTimer = 0f;
                Debug.Log(name + " no encontró nada, va a buscar.");
            }
        }
    }

    void AttackPlayer()
    {
        if (esCobarde && playerCombat != null && playerCombat.TieneArma())
        {
            float distancia = Vector2.Distance(transform.position, player.position);

            if (distancia <= distanciaHuida)
            {
                Vector2 huida = (transform.position - player.position).normalized;
                transform.position += (Vector3)(huida * Time.deltaTime * 2f);
                transform.up = -huida;
                Debug.Log(name + " huye del jugador armado.");
                return;
            }
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
{
    Debug.Log(name + " ataca al jugador.");

    PlayerHealth ph = player.GetComponent<PlayerHealth>();
    if (ph != null)
    {
        ph.TakeDamage(1); // puedes ajustar el daño por enemigo
    }
}


        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * Time.deltaTime * 2f);
        transform.up = dir;

        if (CanSeePlayer())
        {
            lostPlayerTimer = 0f;
        }
        else
        {
            lostPlayerTimer += Time.deltaTime;
            if (lostPlayerTimer >= timeBeforeSearch)
            {
                currentState = State.Busqueda;
                lastKnownPosition = player.position;
                searchTimer = 0f;
                Debug.Log(name + " perdió al jugador, va a investigar.");
            }
        }

        if (esAgresivo && !TieneArmaEquipada())
        {
            Vector2 agresion = (player.position - transform.position).normalized;
            transform.position += (Vector3)(agresion * Time.deltaTime * 2.2f);
            transform.up = agresion;
        }
    }

    void SearchArea()
    {
        searchTimer += Time.deltaTime;

        Vector2 dir = (lastKnownPosition - transform.position).normalized;
        transform.position += (Vector3)(dir * Time.deltaTime * 1.5f);
        transform.up = dir;

        if (Vector2.Distance(transform.position, lastKnownPosition) < 0.2f || searchTimer >= searchTime)
        {
            currentState = State.Pacifico;
            Debug.Log(name + " terminó de buscar.");
        }
    }

    bool TieneArmaEquipada()
    {
        return GetComponentInChildren<SpriteRenderer>() != null;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);
        float angle = Vector2.Angle(transform.up, dirToPlayer);

        if (distance <= visionRange && angle <= visionAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, visionRange, playerLayer);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 rightLimit = Quaternion.Euler(0, 0, visionAngle) * transform.up;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -visionAngle) * transform.up;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightLimit * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + leftLimit * visionRange);
    }
}
