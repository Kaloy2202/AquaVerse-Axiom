using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas inGameUI;
    public Canvas miniGameUI;
    public Image fishDemandMarketUI;
    public Button miniGameButton;
    public GameObject BuyerManager;
    private bool isMarketOpen = false;

    private void Start()
    {
        // Ensure the button has a click listener
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(() => SwitchCanvas(miniGameUI));
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
            SwitchCanvas(miniGameUI);
        }
        if (Input.GetKeyDown(KeyCode.F2) && PlayerStats.Instance.level >= 3)
        {
            isMarketOpen = !isMarketOpen;
            fishDemandMarketUI.gameObject.SetActive(isMarketOpen);
            BuyerManager.SetActive(isMarketOpen);
        }
    }

    public void SwitchCanvas(Canvas canvas)
    {
        if (inGameUI != null && canvas != null)
        {
            // Play close sound if the current active canvas is about to close
            if (inGameUI.gameObject.activeSelf)
            {
                PlayCanvasCloseSound();
                inGameUI.gameObject.SetActive(false);
            }

            // Play open sound for the new canvas being opened
            if (!canvas.gameObject.activeSelf)
            {
                PlayCanvasOpenSound();
                canvas.gameObject.SetActive(true);
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
