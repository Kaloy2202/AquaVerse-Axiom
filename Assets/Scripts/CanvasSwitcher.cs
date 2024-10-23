using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas inGameUI;
    public Canvas miniGameUI;
    public Canvas storeUI;
    public Button miniGameButton;

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
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwitchCanvas(storeUI);
        }
    }

    public void SwitchCanvas(Canvas canvas)
    {
        if (inGameUI != null && canvas != null)
        {
            // Toggle the active state of each canvas
            inGameUI.gameObject.SetActive(!inGameUI.gameObject.activeSelf);
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
        }
    }
}