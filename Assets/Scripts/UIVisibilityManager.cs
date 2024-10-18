using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityManager : MonoBehaviour
{
    public Image uiImageElement;
    public string triggerTag = "InspectText";
    private bool isInTrigger = false;
    private bool isUIVisible = false;

    private void Start()
    {
        // Ensure the UI Image is initially hidden
        if (uiImageElement != null)
        {
            uiImageElement.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if player is in trigger zone and presses F
        if (isInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            ToggleUIElement();
        }
        
        // Hide UI element if player leaves trigger zone
        if (!isInTrigger && isUIVisible)
        {
            HideUIElement();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isInTrigger = false;
        }
    }

    private void ToggleUIElement()
    {
        isUIVisible = !isUIVisible;
        if (uiImageElement != null)
        {
            uiImageElement.gameObject.SetActive(isUIVisible);
        }
    }

    private void HideUIElement()
    {
        isUIVisible = false;
        if (uiImageElement != null)
        {
            uiImageElement.gameObject.SetActive(false);
        }
    }
}