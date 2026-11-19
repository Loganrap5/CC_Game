using UnityEngine;
using FMODUnity;
public class DropSoundOnCollision : MonoBehaviour
{
    private bool soundPlayed = false;

    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (soundPlayed) return;

        if (item == null || item.itemData == null) return;

        // Play the item's specific drop sound
        RuntimeManager.PlayOneShot(
            item.itemData.itemDrop,
            collision.GetContact(0).point
        );

        soundPlayed = true;
    }
}