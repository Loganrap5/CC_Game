using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class Grill : MonoBehaviour
{
    public Transform ingredientLoc;

    private PressInteraction interaction;

    public bool populated = false;

    [Header("Meat Types and Stages")]
    //Nebula Meat
    public GrillData patty;
    public GrillData kebab;
    public GrillData strips;
    public GrillData filling;
    //Dog
    public GrillData spaceDog;
    //Tentacles
    public GrillData tentacles;

    private void Start()
    {
        interaction = GetComponent<PressInteraction>();

        interaction.OnComplete += PlaceItemOnGrill;
    }

    public void PlaceItemOnGrill()
    {
        if (interaction.player.carriedItem != null)
        {
            Item item = interaction.player.carriedItem.GetComponent<Item>();

            if (item == null)
                return;

            Instantiate(item, ingredientLoc.position, Quaternion.identity);

            interaction.player.carriedItem = null;

            return;
        }
    }

    private void OnDestroy()                        
    {
        if (interaction != null)
        {
            interaction.OnComplete -= PlaceItemOnGrill;
        }
    }
}
