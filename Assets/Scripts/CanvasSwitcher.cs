using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas inGameUI;
    public Canvas miniGameUI;
    public Button switchButton;

    private void Start()
    {
        // Ensure the button has a click listener
        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchCanvas);
        }
        else
        {
            Debug.LogWarning("Switch Button not assigned in the CanvasSwitcher script. Button functionality will be disabled.");
        }

        // Set initial state
        if (inGameUI != null && miniGameUI != null)
        {
            inGameUI.gameObject.SetActive(true);
            miniGameUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("One or both canvases not assigned in the CanvasSwitcher script!");
        }
    }

    private void Update()
    {
        // Check for F1 key press
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SwitchCanvas();
        }
    }

    public void SwitchCanvas()
    {
        if (inGameUI != null && miniGameUI != null)
        {
            // Toggle the active state of each canvas
            inGameUI.gameObject.SetActive(!inGameUI.gameObject.activeSelf);
            miniGameUI.gameObject.SetActive(!miniGameUI.gameObject.activeSelf);
        }
    }
}