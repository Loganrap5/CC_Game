using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "Create Item", menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string itemName; 
    public GameObject obj;

    public int index;

    //FMOD
    public EventReference placingSFX;
    public EventReference itemDrop;
}
