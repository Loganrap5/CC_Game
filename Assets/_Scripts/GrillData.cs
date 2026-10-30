using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

[CreateAssetMenu(fileName = "Grill Data", menuName = "Grill Data")]
public class GrillData : ScriptableObject
{
    public List<ItemSO> meatStages = new List<ItemSO>(); 
}
