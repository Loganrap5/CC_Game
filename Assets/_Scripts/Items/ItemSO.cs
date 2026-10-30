using UnityEngine;

[CreateAssetMenu(fileName = "Create Item", menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string itemName; 
    public GameObject obj;

    public int index;
}
