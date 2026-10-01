using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    private CharacterController characterController;

    private Vector2 moveInput;

    private float verticalVelocity;

    Interactor interactor;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        interactor = GetComponent<Interactor>();
    }

    private void Update()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        //if (characterController.isGrounded && verticalVelocity < 0)
        //{

        //}
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;

        characterController.Move(movement * moveSpeed * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Setting the Interact function to whatever button is set for it. 
        interactor.InteractEvent();

        if (context.started)
        {
            interactor.InteractEvent();
        }
        if (context.canceled)
        {
            interactor.CancelInteraction();
        }
    }
}
