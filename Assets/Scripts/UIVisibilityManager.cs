using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityManager : MonoBehaviour
{
    public Image uiImageElement;
    public string triggerTag = "InspectText";
    private bool isInTrigger = false;

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
        // Check if player is in trigger zone and pressing F
        if (isInTrigger && Input.GetKey(KeyCode.F))
        {
            ShowUIElement();
        }
        else
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

    private void ShowUIElement()
    {
        if (uiImageElement != null)
        {
            uiImageElement.gameObject.SetActive(true);
        }
    }

    private void HideUIElement()
    {
        if (uiImageElement != null)
        {
            uiImageElement.gameObject.SetActive(false);
        }
    }
}