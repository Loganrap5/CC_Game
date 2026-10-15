using UnityEngine;

public class PickupInteraction : Interaction
{
    //This class is for whenever you want an object to be picked up and stored. 
    //Put this Class on along with Interactable.cs on the object. 


    private CookingStation cookingStation;
    private int stationSlot = -1;

    public void SetCookingStation(CookingStation station, int slot)
    {
        cookingStation = station;
        stationSlot = slot;
    }


    public override void StartInteraction(Player player)
    {
        //dont pick anything up if you already have something
        if (player.carriedItem != null)
            return;

        Item item = GetComponent<Item>();

        if (item == null)
        {
            return;
        }

        //tell the cooking station to remove this ingredient.
        if (cookingStation != null && stationSlot >= 0)
        {
            cookingStation.RemoveIngredientAt(stationSlot);
        }

        //get the prefab from the scriptable object
        player.carriedItem = item.itemData.obj;

        //then get rid of the object
        Destroy(gameObject);
    }

    public override void UpdateInteraction()
    {
        //Not needed
    }

    public override void CancelInteraction()
    {
        //Not needed
    }

    public override void CompleteInteraction()
    {
        //Not needed
    }
}
