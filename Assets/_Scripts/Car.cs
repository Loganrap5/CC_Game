using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    //Possibly contain order for car with condition to meet. 

    public Item order;

    private PressInteraction interaction;

    private float carSpeed = 3f;

    public bool foodDelivered = false; 

    private void Start()
    {
        interaction = GetComponentInChildren<PressInteraction>();

        interaction.OnComplete += DeliverFood;
    }

    public void DeliverFood()
    {
        //check if proper item is whats being given.

        Debug.Log(interaction.player.name);

        if (interaction.player.carriedItem == null)
        {
            Debug.Log("No item held!");
            return;
        }

        Item carriedItem = interaction.player.carriedItem.GetComponent<Item>();

        if (carriedItem != order)
        {
            Debug.Log("Carrying incorrect item!");
            return;
        }

        //If you are holding something and have the correct item.

        //Remove item from inventory
        interaction.player.carriedItem = null;

        //Get car to leave - in this case setting foodDeliviered to true as GameManager handles the logic
        foodDelivered = true;

        //Add any conditions (money, bonus etc)

    }


    public void MoveCar(Transform loc)
    {
        StartCoroutine(MoveCarForward(loc));
    }


    public IEnumerator MoveCarForward(Transform loc)
    {
        while(Vector3.Distance(this.transform.position, loc.position) > 0.01f)
        {
            //Move car slowly towards the windows position.
            transform.position = Vector3.MoveTowards(transform.position, loc.position, carSpeed * Time.deltaTime);

            yield return null; 

        }

        //Set at the end to ensure correct placement.
        this.transform.position = loc.position;

    }

    private void OnDestroy()
    { 
        if (interaction != null)
        {
            interaction.OnComplete -= DeliverFood;
        }
    }

}
