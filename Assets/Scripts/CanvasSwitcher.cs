using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas inGameUI;
    public Canvas miniGameUI;
    public Image fishDemandMarketUI;
    public Button miniGameButton;
    public GameObject BuyerManager;
    public Image marketLockedBtn;
    public Image marketUnlockedBtn;
    public AudioManager audioManager;
    private FirstPersonController firstPersonController; // Reference to the PlayerStat script

    private bool isMarketOpen = false;
    private bool isMiniGameCanvasOpen = false;

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
            ToggleMiniGameCanvas();
        }
        if (Input.GetKeyDown(KeyCode.F2) && PlayerStats.Instance.level >= 3)
        {
            isMarketOpen = !isMarketOpen;
            fishDemandMarketUI.gameObject.SetActive(isMarketOpen);
            BuyerManager.SetActive(isMarketOpen);
            firstPersonController.enabled = !isMarketOpen;
        }
        if (PlayerStats.Instance.level >= 3)
        {
            marketLockedBtn.gameObject.SetActive(false);
            marketUnlockedBtn.gameObject.SetActive(true);
        }
    }

    private void ToggleMiniGameCanvas()
    {
        if (inGameUI != null && miniGameUI != null)
        {
            if (isMiniGameCanvasOpen)
            {
                // Close the miniGameUI canvas
                PlayCanvasCloseSound();
                miniGameUI.gameObject.SetActive(false);
            }
            else
            {
                // Open the miniGameUI canvas
                PlayCanvasOpenSound();
                inGameUI.gameObject.SetActive(false);
                miniGameUI.gameObject.SetActive(true);
            }

            // Toggle the state
            isMiniGameCanvasOpen = !isMiniGameCanvasOpen;
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
