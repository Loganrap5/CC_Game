using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement : MonoBehaviour
{
    public float speed;
    private Vector2 move;

    
    Interactor interactor;
    Player player;

    private void Start()
    {
        interactor = GetComponent<Interactor>();
        player = GetComponent<Player>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Setting the Interact function to whatever button is set for it. 
        interactor.InteractEvent(); 

        if(context.started)
        {
            interactor.InteractEvent();
        }
        if(context.canceled)
        {
            interactor.CancelInteraction();
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movement),
                0.15f
            );
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World); 
    }


    
}
