using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas smartCalculatorCanvas;  // The canvas for the smart calculator
    public Canvas miniGameUI;
    public Button miniGameButton;
    public AudioManager audioManager;
    private FirstPersonController firstPersonController;

    private bool isSmartCalculatorOpen = false; // Track if the Smart Calculator is open

    private void Start()
    {
        firstPersonController = FindObjectOfType<FirstPersonController>();

        // Ensure the button has a click listener
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(() => ToggleMiniGameCanvas());
        }
        else
        {
            Debug.LogWarning("Switch Button not assigned in the CanvasSwitcher script. Button functionality will be disabled.");
        }

        // Set initial state for canvases
        if (smartCalculatorCanvas != null && miniGameUI != null)
        {
            smartCalculatorCanvas.gameObject.SetActive(false);  // Hide Smart Calculator by default
            miniGameUI.gameObject.SetActive(false);  // Hide Mini Game canvas by default
        }
        else
        {
            Debug.LogError("One or more canvases not assigned in the CanvasSwitcher script!");
        }
    }

    private void Update()
    {
        // Listen for 'C' key press to toggle the Smart Calculator canvas
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSmartCalculatorCanvas();
        }

        // Optional: If you still want to toggle MiniGameUI with F1 key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleMiniGameCanvas();
        }
    }

    private void ToggleSmartCalculatorCanvas()
    {
        if (smartCalculatorCanvas != null)
        {
            if (isSmartCalculatorOpen)
            {
                // Close the Smart Calculator canvas
                PlayCanvasCloseSound();
                smartCalculatorCanvas.gameObject.SetActive(false);
            }
            else
            {
                // Open the Smart Calculator canvas
                PlayCanvasOpenSound();
                smartCalculatorCanvas.gameObject.SetActive(true);
            }

            // Toggle the state
            isSmartCalculatorOpen = !isSmartCalculatorOpen;
        }
    }

    private void ToggleMiniGameCanvas()
    {
        if (miniGameUI != null)
        {
            // Toggle the Mini Game canvas (assuming you still want this feature)
            if (miniGameUI.gameObject.activeSelf)
            {
                miniGameUI.gameObject.SetActive(false);
            }
            else
            {
                miniGameUI.gameObject.SetActive(true);
            }
        }
    }

    private void PlayCanvasOpenSound()
    {
        if (audioManager != null)
        {
            audioManager.Play("CanvasOpenSound");
        }
        else
        {
            Debug.LogWarning("AudioManager not assigned in CanvasSwitcher.");
        }
    }

    private void PlayCanvasCloseSound()
    {
        if (audioManager != null)
        {
            audioManager.Play("CanvasCloseSound");
        }
        else
        {
            Debug.LogWarning("AudioManager not assigned in CanvasSwitcher.");
        }
    }
}
