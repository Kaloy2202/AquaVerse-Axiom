using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public Canvas canvas;

    void Start()
    {
        // Add debug log to confirm canvas starts disabled
        canvas.gameObject.SetActive(false);
        Debug.Log("Canvas disabled on start");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed"); // Confirm F key detection
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Debug ray visualization
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            Debug.Log($"Hit: {hit.collider.name} at distance: {hit.distance}");
            Debug.Log($"Object layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            Debug.Log($"Object tag: {hit.collider.tag}");
            
            if(hit.collider.CompareTag("NPC")) // Use CompareTag instead of tag ==
            {
                Debug.Log("NPC tag detected, enabling canvas");
                canvas.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Hit object does not have NPC tag");
            }
        }
        else
        {
            Debug.Log($"No hit detected. Ray length: {interactionRange}, Layer mask: {interactableLayer.value}");
        }
    }
}