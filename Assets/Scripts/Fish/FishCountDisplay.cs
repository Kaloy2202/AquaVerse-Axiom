using UnityEngine;
using TMPro;

public class FishCountDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float displayDistance = 20f; // Maximum distance to show the text
    public TextMeshProUGUI timerText;
    private const int MAX_FISH = 200;
    private PoolManager? lastPool = null;
    private float lastFishCount = -1;

    void Start()
    {
        if (fishCountText == null)
        {
            Debug.LogError("Fish count TextMeshPro component not assigned!");
        }
        
        if (sceneCamera == null)
        {
            sceneCamera = Camera.main;
            if (sceneCamera == null)
            {
                Debug.LogError("No camera assigned and couldn't find main camera!");
            }
        }

        // Hide text initially
        fishCountText.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        PoolManager? currentPool = null;
        float timeToHarvest = 0f;

        if (Physics.Raycast(ray, out RaycastHit hit, displayDistance, placementLayer))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                currentPool = hit.collider.gameObject.GetComponent<PoolManager>();
                // Enable text if we hit a pool within range
                if (currentPool != null)
                {
                    fishCountText.gameObject.SetActive(true);
                    timerText.gameObject.SetActive(true);
                    timeToHarvest = currentPool.timer;
                }
            }
        }

        timerText.text = $"Time to harvest: {(int)timeToHarvest} seconds";

        // If we didn't hit a pool or hit is too far, hide the text
        if (currentPool == null)
        {
            fishCountText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            lastPool = null;
            lastFishCount = -1;
            return;
        }

        // Update display if pool or fish count changed
        float currentFishCount = currentPool.getNumberOfFish();
        if (currentPool != lastPool || currentFishCount != lastFishCount)
        {
            UpdateDisplay(currentPool);
            lastPool = currentPool;
            lastFishCount = currentFishCount;
        }
    }

    private void UpdateDisplay(PoolManager pool)
    {
        int currentFish = (int)pool.getNumberOfFish();
        string poolName = pool.gameObject.name;

        Debug.Log($"Updating display for {poolName}: {currentFish}/{MAX_FISH} fish");
        fishCountText.text = $"{poolName}: {currentFish}/{MAX_FISH} fish";
        
        if (currentFish >= MAX_FISH)
        {
            fishCountText.color = Color.red;
        }
        else if (currentFish >= MAX_FISH * 0.5f)
        {
            fishCountText.color = new Color(1.0f, 0.5f, 0);  // Orange
        }
        else
        {
            fishCountText.color = Color.white;
        }
    }
}