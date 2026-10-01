using UnityEngine;

public class PressInteraction : Interaction
{
    //This class is for interaction whenever you simply just want to press one time (like a button or something)
    //Put this Class on along with Interactable.cs on the object. 

    public Player player;


    public override void StartInteraction(Player _player)
    {
        player = _player;




        Complete();
    }

    public override void UpdateInteraction()
    {
        //Nothing needed.
    }

    public override void CancelInteraction()
    {
        //Nothing needed.
    }

    public override void CompleteInteraction()
    {
        //Nothing needed.
    }
}
