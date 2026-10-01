using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private Interaction interaction;

    private void Start()
    {
        interaction = GetComponent<Interaction>();
    }

    //called by interactor.cs
    public void Interact(Player player)
    {
        if (interaction == null)
        {
            Debug.LogWarning($"{gameObject.name} has Interactable.cs but no Interaction component.");
            return;
        }

        interaction.StartInteraction(player);
    }

    //used for interactions that need to update while the button is held
    public void UpdateInteraction()
    {
        if (interaction != null)
            interaction.UpdateInteraction();
    }

    //called when the player releases the interaction button
    public void CancelInteraction()
    {
        if (interaction != null)
            interaction.CancelInteraction();
    }
}
