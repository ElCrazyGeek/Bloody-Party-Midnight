using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Follow")]
    public float followSmoothness = 0.125f;

    [Header("Zoom")]
    public float normalZoom = 5f;
    public float zoomOutSize = 8f;
    public float zoomSpeed = 5f;
    public KeyCode zoomKey = KeyCode.LeftShift;

    [Header("Look Ahead")]
    public float lookAheadDistance = 3f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = normalZoom;
    }

    void LateUpdate()
    {
        if (!target) return;

        // 1. Zoom
        float targetZoom = Input.GetKey(zoomKey) ? zoomOutSize : normalZoom;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // 2. Dirección del mouse (Look Ahead)
        Vector3 desiredPosition = target.position;

        if (Input.GetKey(zoomKey))
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorld - target.position).normalized;

            desiredPosition += (Vector3)(direction * lookAheadDistance);
        }

        // 3. Desplazamiento final
        desiredPosition += offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness);
    }
}
