using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas inGameUI;
    public Canvas miniGameUI;
    public Image fishDemandMarketUI;
    public Button miniGameButton;
    public GameObject BuyerManager;
    public Image marketLockedBtn;
    public Image marketUnlockedBtn;

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
        if (PlayerStats.Instance.level >= 3)
        {
            marketLockedBtn.gameObject.SetActive(false);
            marketUnlockedBtn.gameObject.SetActive(true);
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