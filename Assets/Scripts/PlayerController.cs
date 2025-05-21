using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float slowMultiplier = 0.5f; // para torpeza
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;




    [HideInInspector] public bool isSlowed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        float finalSpeed = isSlowed ? moveSpeed * slowMultiplier : moveSpeed;
        rb.linearVelocity = movement.normalized * finalSpeed;

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    
    public void SetSpeed(float newSpeed)
{
    moveSpeed = newSpeed;
}

}
