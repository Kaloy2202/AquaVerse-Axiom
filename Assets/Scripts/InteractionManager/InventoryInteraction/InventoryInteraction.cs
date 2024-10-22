using UnityEngine;

public class InventoryInteraction : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // Logic for interacting with the NPC
        Debug.Log("You opened thhe inventory!");
    }
}
