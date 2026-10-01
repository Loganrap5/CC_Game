using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public Transform target;
    public Transform player;

    public float rotationSpeed = 3f;

    private float yaw;
    private float pitch;

    public bool active = true;

    private Vector2 lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!active)
            return;

        float mouseX = lookInput.x * rotationSpeed;
        float mouseY = lookInput.y * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Player rotates horizontally
        player.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Camera looks vertically
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
