using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 3.0f;

    private float yaw = 0f;
    private float pitch = 0f;

    public bool active = true;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //private void LateUpdate()
    //{
    //    if (active == true)
    //    {
    //        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
    //        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

    //        yaw += mouseX;

    //        pitch -= mouseY;

    //        transform.position = target.position;
    //        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    //    }
    //}

    private void Update()
    {
        if (active == true)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            yaw += mouseX;

            pitch -= mouseY;

            transform.position = target.position;
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}
