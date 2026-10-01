using UnityEngine;

public class HoldInteraction : Interaction
{
    //This class is important because lots of interactions will be done by holding down. 
    //Put this Class on along with Interactable.cs on the object. 

    public float requiredTime;

    private float timer;
    private Player player;
    private bool interacting;

    public override void StartInteraction(Player player)
    {
        this.player = player;

        timer = 0f;
        interacting = true;
    }

    public override void UpdateInteraction()
    {
        if (!interacting)
            return;

        timer += Time.deltaTime;

        float progress = timer / requiredTime;

        //progress bar will go here
        //progressBar.value = progress;



        if (progress >= 1f)
        {
            CompleteInteraction();
        }
    }

    public override void CancelInteraction()
    {
        if (!interacting)
            return;

        interacting = false;
        timer = 0f;

        //reset progress bar here


    }

    public override void CompleteInteraction()
    {
        if (!interacting)
            return;

        interacting = false;



        Complete();
    }
}
