using UnityEngine;

public class HoldInteraction : Interaction
{
    //This class is important because lots of interactions will be done by holding down. 
    //Put this Class on along with Interactable.cs on the object. 

    public float requiredTime;

    public float timer;
    public Player player;
    public bool interacting;

    public override void StartInteraction(Player _player)
    {
        this.player = _player;

        timer = 0f;
        interacting = true;

        player.isHoldingDown = true;
        player.movement.animator.SetBool("Holding", player.isHoldingDown);
    }

    public override void UpdateInteraction()
    {
        if (!interacting)
            return;

        timer += Time.deltaTime;

        Debug.Log("Timer: " + timer + " / Required: " + requiredTime);

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

        player.isHoldingDown = false;
        player.movement.animator.SetBool("Holding", player.isHoldingDown);

        //reset progress bar here


    }

    public override void CompleteInteraction()
    {
        if (!interacting)
            return;

        interacting = false;
        player.isHoldingDown = false;
        player.movement.animator.SetBool("Holding", player.isHoldingDown);



        Complete();
    }

    public void SetInteractionTime(float time)
    {
        requiredTime = time;
    }
}
