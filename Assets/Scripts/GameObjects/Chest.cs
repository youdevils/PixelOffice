using GameDataDictionary;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite chestSpriteClosed;
    public Sprite chestSpriteOpen;
    public ParticleSystem chestParticles;
    public int chestItemID;
    private bool isOpen = false;
    public void Open() {
        if (isOpen) {
            string message = "You keep looking expecting to find more..";
            UIManager.instance.PostNotification("Icons/Card/unknown", message);
            return;
        }
        ItemData data = DataMart.GetItem(chestItemID);
        bool isAdded = Player.instance.Inventory.AddInventoryItem(data);
        if (isAdded) {
            isOpen = true;
            transform.GetComponent<SpriteRenderer>().sprite = chestSpriteOpen;
            chestParticles.Stop();
            string message = "You have found a <color=#C2C34D>" + data.Name + "</color>!";
            UIManager.instance.PostNotification(data.SpriteURI, message);
        } else {
            string message = "Your inventory is full.";
            UIManager.instance.PostNotification("Icons/Card/unknown", message);
        }
    }
}
