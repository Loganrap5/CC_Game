using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set; }

    public Transform camTransform;

    public Rigidbody rb;

    Camera cam;

    Vector3 screenCenter;

    //Input Actions
    public Movement movement;

    public InputAction forward;
    public InputAction backward;
    public InputAction left;
    public InputAction right;
    public InputAction jump;

    //Target Point Ray
    Ray targetRay;
    RaycastHit targetRayHit;
    public Vector3 targetPoint;

    //Vars
    Vector3 moveDirection;
    public float speed = 5f;
    public float jumpForce = 5f;
    public bool grounded = true;
    public LayerMask groundLayers;

    private bool movingForward;
    private bool movingBackward;
    private bool movingLeft;
    private bool movingRight;

    //DEBUG
    public InputAction cursor;
    public bool cursorEnabled = false;


    private void Awake()
    {
        //DEBUG
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //-----

        rb = GetComponent<Rigidbody>();

        instance = this;

        movement = new Movement();

        forward = movement.Inputs.Forward;
        backward = movement.Inputs.Backward;
        left = movement.Inputs.Left;
        right = movement.Inputs.Right;
        jump = movement.Inputs.Jump;

        //cursor = movement.Inputs.Cursor;
    }

    private void Update()
    {
        //Raycast target point
        targetRay = cam.ScreenPointToRay(screenCenter);
        //Debug.DrawRay(targetRay.origin, targetRay.direction * 100f, Color.red);

        if (Physics.Raycast(targetRay, out targetRayHit))
        {
            targetPoint = targetRayHit.point;
        }
        else
        {
            targetPoint = targetRay.GetPoint(100f);
        }

        //Grounded Check
        grounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hitInfo, 1.0f, groundLayers);

       
    }

    private void FixedUpdate()
    {
        moveDirection = Vector3.zero;

        if (movingForward) { moveDirection += GetCameraForward(); }
        if (movingBackward) { moveDirection += GetCameraBackward(); }
        if (movingLeft) { moveDirection += GetCameraLeft(); }
        if (movingRight) { moveDirection += GetCameraRight(); }

        moveDirection.Normalize();

        rb.position += moveDirection * speed * Time.deltaTime;
    }

    private void Start()
    {
        cam = Camera.main;

        screenCenter = (new Vector3(Screen.width / 2, Screen.height / 2));
    }

    public void EnableMovement()
    {
        forward.Enable();
        backward.Enable();
        left.Enable();
        right.Enable();
        cursor.Enable();
        jump.Enable();
    }

    public void DisableMovement()
    {
        forward.Disable();
        backward.Disable();
        left.Disable();
        right.Disable();
        cursor.Disable();
        jump.Disable();
    }

    private void OnEnable()
    {
        forward.Enable();
        backward.Enable();
        left.Enable();
        right.Enable();
        cursor.Enable();
        jump.Enable();

        cursor.performed += CursorDebug;

        forward.performed += Forward;
        forward.canceled += HaltForward;

        backward.performed += Backward;
        backward.canceled += HaltBackward;

        left.performed += Left;
        left.canceled += HaltLeft;

        right.performed += Right;
        right.canceled += HaltRight;

        jump.performed += Jump;
    }

    private void OnDisable()
    {
        forward.Disable();
        backward.Disable();
        left.Disable();
        right.Disable();
        cursor.Disable();
        jump.Disable();

        cursor.performed -= CursorDebug;

        forward.performed -= Forward;
        forward.canceled -= HaltForward;

        backward.performed -= Backward;
        backward.canceled -= HaltBackward;

        left.performed -= Left;
        left.canceled -= HaltLeft;

        right.performed -= Right;
        right.canceled -= HaltRight;

        jump.performed -= Jump;
    }

    //DEBUG-------------------------------------------------
    public void CursorDebug(InputAction.CallbackContext context)
    {
        if (cursorEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorEnabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorEnabled = true;
        }

    }
    //DEBUG--------------------------------------

    private void Jump(InputAction.CallbackContext context)
    {
        if (grounded)
        {
            Debug.Log("JUMPED!");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }
    }

    public Vector3 GetCameraForward()
    {
        Vector3 forward = camTransform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetCameraBackward()
    {
        Vector3 backward = -camTransform.forward;
        backward.y = 0;
        return backward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = camTransform.right;
        right.y = 0;
        return right.normalized;
    }

    public Vector3 GetCameraLeft()
    {
        Vector3 left = -camTransform.right;
        left.y = 0;
        return left.normalized;
    }

    public void Forward(InputAction.CallbackContext context)
    {
        //moveDirection += GetCameraForward();
        movingForward = true;
    }

    public void HaltForward(InputAction.CallbackContext context)
    {
        //moveDirection -= GetCameraForward();
        movingForward = false;
    }

    public void Backward(InputAction.CallbackContext context)
    {
        //moveDirection += GetCameraBackward();
        movingBackward = true;
    }

    public void HaltBackward(InputAction.CallbackContext context)
    {
        //moveDirection -= GetCameraBackward();
        movingBackward = false;
    }

    public void Left(InputAction.CallbackContext context)
    {
        //moveDirection += GetCameraLeft();
        movingLeft = true;
    }

    public void HaltLeft(InputAction.CallbackContext context)
    {
        //moveDirection -= GetCameraLeft();
        movingLeft = false;
    }

    public void Right(InputAction.CallbackContext context)
    {
        //moveDirection += GetCameraRight();
        movingRight = true;
    }

    public void HaltRight(InputAction.CallbackContext context)
    {
        //moveDirection -= GetCameraRight();
        movingRight = false;
    }
}
