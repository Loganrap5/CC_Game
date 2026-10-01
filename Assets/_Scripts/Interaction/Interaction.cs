using System;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    public event Action OnComplete;

    public abstract void StartInteraction(Player player);

    public abstract void UpdateInteraction();

    public abstract void CancelInteraction();

    public abstract void CompleteInteraction();

    protected void Complete()
    {
        OnComplete?.Invoke();
    }
}
