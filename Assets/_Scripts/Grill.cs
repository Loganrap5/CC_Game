using NUnit.Framework;
using UnityEngine;
//using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Grill : MonoBehaviour
{
    public Transform ingredientLoc;

    private PressInteraction interaction;

    private bool populated = false;
    private bool cooked = false;

    public GameObject spawnedObject;
    public ItemSO itemRef;

    public float timeToCount;
    private float startingTime;

    public int index = 0;

    public Image progressBar;
    public Canvas canvas;

    [Header("Meat Types and Stages")]
    public List<GrillData> data = new List<GrillData>();
    public GrillData currentGrillData;

    


    //TODO - Determine when player picks up meat from grill it resets everything. 
    //TODO - After that make sure to reset grill.
    //TODO - Allow player to take meat off thats half cooked and continue to cook it. 


    //make it check index regardless because the itemso if starting at nothing will be 0 anyway. 

    private void Start()
    {
        interaction = GetComponent<PressInteraction>();

        interaction.OnComplete += PlaceItemOnGrill;
    }

    private void Update()
    {
        if(populated && !cooked)
        {
            timeToCount -= Time.deltaTime;

            progressBar.fillAmount = timeToCount / startingTime;

            if (timeToCount <= 0)
            {
                if (index < 3)
                {
                    SpawnNextMeatType();
                }
                else if(index == 3)
                {
                    //This is where it would be burnt stop here.
                    cooked = true;

                    //Reset timer
                    timeToCount = 0;
                }
            }
        }

        
        
    }

    public void PlaceItemOnGrill()
    {
        if (populated) return;
        if (interaction.player.carriedItem == null) return;

        Debug.Log("Item placed");

        Item item = interaction.player.carriedItem.GetComponent<Item>();

        if (item == null)
            return;

        //Spawn item on grill
        spawnedObject = Instantiate(item.itemData.obj, ingredientLoc.position, Quaternion.identity);

        //Assign reference
        itemRef = item.itemData;

        //Inside the ItemSO, set this Grill as its reference, so when it's picked up it can stop this grill from doing it's thing. 
        //itemRef.GetComponent<PickupInteraction>().SetGrill(this);

        PickupInteraction pickup = spawnedObject.GetComponent<PickupInteraction>();

        if (pickup != null)
            pickup.SetGrill(this);

        //Remove item from player inventory
        interaction.player.carriedItem = null;

        StartGrill();

        //if (interaction.player.carriedItem == null || populated)
        //    return;

        //if (interaction.player.carriedItem != null)
        //{
        //    Debug.Log("Item placed");

        //    Item item = interaction.player.carriedItem.GetComponent<Item>();

        //    if (item == null)
        //        return;

        //    //Spawn item on grill
        //    spawnedObject = Instantiate(item.itemData.obj, ingredientLoc.position, Quaternion.identity);

        //    //Assign reference
        //    itemRef = item.itemData;

        //    //Inside the ItemSO, set this Grill as its reference, so when it's picked up it can stop this grill from doing it's thing. 
        //    //itemRef.GetComponent<PickupInteraction>().SetGrill(this);

        //    PickupInteraction pickup = spawnedObject.GetComponent<PickupInteraction>();

        //    if (pickup != null)
        //        pickup.SetGrill(this);

        //    //Remove item from player inventory
        //    interaction.player.carriedItem = null;

        //    return;
        //}

        //StartGrill();
    }

    public void StartGrill()
    {
        Debug.Log("Grill started");

        ////First compare the beginning type (raw meat) to the grilldata to tell the game what type of meat to cook up to.
        //for(int i = 0; i < data.Count; i++)
        //{
        //    if (data[i].meatStages[0] == itemRef)
        //    {
        //        currentGrillData = data[i];
        //    }
        //    else continue;
        //}

        for (int i = 0; i < data.Count; i++)   //Check all meat data
        {
            foreach(ItemSO meatType in data[i].meatStages)
            {
                if(meatType == itemRef)
                {
                    //Set grill data so it knows what to switch next to
                    currentGrillData = data[i];

                    //Set correct index
                    index = meatType.index;


                }
            }
        }

        //Enable timer
        canvas.gameObject.SetActive(true);

        //Set timer based on meat data.
        timeToCount = currentGrillData.timeToCook;
        startingTime = timeToCount;

        populated = true;

    }

    public void SpawnNextMeatType()
    {
        index++;

        //Get rid of old temp meat
        Destroy(spawnedObject);

        //Spawn new temp of meat
        spawnedObject = Instantiate(currentGrillData.meatStages[index].obj, ingredientLoc.position, Quaternion.identity);

        PickupInteraction pickup = spawnedObject.GetComponent<PickupInteraction>();

        if (pickup != null)
            pickup.SetGrill(this);

        //Reset timer
        timeToCount = currentGrillData.timeToCook;
        startingTime = timeToCount;
    }

    public void RemoveIngredient()
    {
        Debug.Log("INGREDIENT REMOVED");

        //Reset timer
        timeToCount = 0;

        //Delete item
        Destroy(spawnedObject);

        //Unassign ItemSO reference
        itemRef = null;

        //Reset index
        index = 0;

        populated = false;
        cooked = false;
        currentGrillData = null;

        //Remove timer
        canvas.gameObject.SetActive(false);
    }

    private void OnDestroy()                        
    {
        if (interaction != null)
        {
            interaction.OnComplete -= PlaceItemOnGrill;
        }
    }
}
