using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public Canvas NPCDialog;
    public Canvas HouseDialog;
    public Canvas FishFryDialog;
    public TextMeshPro questIndicator;
    void Start()
    {
        // Add debug log to confirm canvas starts disabled
        NPCDialog.gameObject.SetActive(false);
        HouseDialog.gameObject.SetActive(false);
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
                NPCDialog.gameObject.SetActive(true);
                questIndicator.gameObject.SetActive(false);
                QuestManager.Instance.StartDialogueForQuest();

            }
            else if (hit.collider.CompareTag("Diary") && QuestManager.Instance.IsQuestComplete("QUEST_1") == false)
            {
                Debug.Log("Diary tag detected, enabling canvas");
                HouseDialog.gameObject.SetActive(true);
            }
            else if (hit.collider.CompareTag("Inventory") && QuestManager.Instance.IsQuestComplete("QUEST_2") == false)
            {
                Debug.Log("Inventory tag detected, enabling canvas");
                FishFryDialog.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("No valid tag detected");
            }

        }
        else
        {
            Debug.Log($"No hit detected. Ray length: {interactionRange}, Layer mask: {interactableLayer.value}");
        }
    }
}