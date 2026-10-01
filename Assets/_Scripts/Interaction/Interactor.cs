using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

//Every player contains this Class. It allows you to interact with items or objects that have "Interactable.cs" attached to them

interface IInteractable
{
    public void Interact(Player player);
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange;

    Player player;

    private Interactable currentInteractable;
    private bool interacting;

    private void Start()
    {
        player = GetComponent<Player>();
    }


    public void InteractEvent()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.TryGetComponent(out Interactable interactable))
            {
                currentInteractable = interactable;
                interacting = true;

                currentInteractable.Interact(player);
            }
        }

    }

    private void Update()
    {
        if (interacting && currentInteractable != null)
        {
            currentInteractable.UpdateInteraction();
        }
    }

    public void CancelInteraction()
    {
        if (!interacting || currentInteractable == null)
            return;

        currentInteractable.CancelInteraction();

        currentInteractable = null;
        interacting = false;
    }

}
