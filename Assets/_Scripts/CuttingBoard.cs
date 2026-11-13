using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour
{
    public Transform ingredientLoc;

    public HoldInteraction interaction;
    private bool populated = false;

    private GameObject spawnedObject;

    public List<GrillData> data = new List<GrillData>();
    public GrillData currentCuttingData;
    public int index = 0;

    private ItemSO itemRef;

    [Header("Progress Bar")]
    public Image progressBar;
    public Canvas canvas;

    private float timeToCount;
    private float startingTime;

    private void Start()
    {
        interaction = GetComponent<HoldInteraction>();

        //interaction.requiredTime = 0.1f;

        interaction.OnComplete += PlaceItemOnBoard;

        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (populated && interaction.interacting)
        {
            canvas.gameObject.SetActive(true);

            startingTime = interaction.requiredTime;
            timeToCount = interaction.timer;

            progressBar.fillAmount = timeToCount / startingTime;
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void PlaceItemOnBoard()
    {
        //bool itemMatches = CheckPlacedItem();

        if (!populated)// && interaction.player.carriedItem == itemRef)
        {
            if (interaction.player.carriedItem == null) return;
            

            Item item = interaction.player.carriedItem.GetComponent<Item>();

            if (item == null)
                return;

            SetCuttingData();

            //Spawn item on grill
            spawnedObject = Instantiate(item.itemData.obj, ingredientLoc.position, Quaternion.identity);

            //Prevent object from moving
            spawnedObject.GetComponent<Rigidbody>().isKinematic = true;

            //Set cuttingboard reference inside interaction so it knows when its picked up.
            PickupInteraction pickup = spawnedObject.GetComponent<PickupInteraction>();
            if (pickup != null)
            {
                pickup.SetCuttingBoard(this);
            }

            populated = true;

            interaction.requiredTime = 2.5f;

            //Remove from inventory
            interaction.player.carriedItem = null;

            //Create reference
            itemRef = item.itemData;

        }
        else if(populated)
        {
            if (index == currentCuttingData.meatStages.Count - 1) return;
            //Spawn next type until complete.
            index++;

            //Delete original object
            Destroy(spawnedObject);

            //Spawn new temp of meat
            spawnedObject = Instantiate(currentCuttingData.meatStages[index].obj, ingredientLoc.position, Quaternion.identity);

            //Set cuttingboard reference inside interaction so it knows when its picked up.
            PickupInteraction pickup = spawnedObject.GetComponent<PickupInteraction>();
            if (pickup != null)
            {
                pickup.SetCuttingBoard(this);
            }
        }
        
    }

    public void RemoveIngredient()
    {
        populated = false;
        index = 0;
        currentCuttingData = null;
        interaction.requiredTime = 0.1f;
    }

    public void SetCuttingData()
    {
        Item item = interaction.player.carriedItem.GetComponent<Item>();

        //for (int i = 0; i < data.Count; ++i)
        //{
        //    foreach(ItemSO type in data[i].meatStages)
        //    {
        //        if (type == itemRef)
        //        {
        //            currentCuttingData = data[i];
        //            index = type.index;
        //        }
        //    }
        //}
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].meatStages.Count; j++)
            {
                if (data[i].meatStages[j] == item.itemData)
                {
                    currentCuttingData = data[i];
                    index = j;
                    return;
                }
            }
        }
    }
}
