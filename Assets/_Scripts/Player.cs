using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Player : MonoBehaviour
{
    //Store player data here, avatar chosen, name etc

    public string name;
    public int playerIndex;

    public GameObject carriedItem = null; 

    //This is a start point for where items should be dropped and where the player is looking at. 
    public Transform eyes;
    public float dropRange = 1f;

    public void DropItem()    // TODO -- Right now it just drops the first item in line in the inventory, or wont if its empty. will change later. 
    {
        if (carriedItem == null) return;

        Ray r = new Ray(eyes.transform.position, eyes.transform.forward);
        if(Physics.Raycast(r, out RaycastHit hitinfo, dropRange))
        {
            Vector3 hitLoc = hitinfo.point;

            hitLoc.y = 0.5f;
            Instantiate(carriedItem.gameObject, hitLoc, Quaternion.identity);

            carriedItem = null; 
        }
        else
        {
            Vector3 playerForward = eyes.transform.position + eyes.transform.forward * dropRange;

            Instantiate(carriedItem.gameObject, playerForward, Quaternion.identity);

            carriedItem = null; 
        }
    }
}
