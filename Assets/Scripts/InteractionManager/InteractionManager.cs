using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float interactionRange = 3f; // Range within which interactions can happen
    public LayerMask interactableLayer; // Set this to the layer that contains all interactable objects

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Detect 'F' key press
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            Debug.Log($"Hit: {hit.collider.name} at distance: {hit.distance}");
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            // Additional debug log
            Debug.Log(interactable != null ? "Interactable found." : "No IInteractable found on the hit object.");
        }

    }

}
