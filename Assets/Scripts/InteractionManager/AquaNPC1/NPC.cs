using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // Logic for interacting with the NPC
        Debug.Log("You are interacting with the NPC!");
    }
}
