using UnityEngine;
using TMPro;

public class FishCountDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayer;
    private const int MAX_FISH = 200;
    private PoolManager? lastPool = null;
    private float lastFishCount = -1;  // Force initial update

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

        UpdateDisplay(null);
    }

    void Update()
    {
        // Get mouse position and create ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        // Raycast to find pool
        PoolManager? currentPool = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayer))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                currentPool = hit.collider.gameObject.GetComponent<PoolManager>();
            }
        }

        // Only update if pool changed or fish count changed
        if (currentPool != null)
        {
            float currentFishCount = currentPool.getNumberOfFish();
            if (currentPool != lastPool || currentFishCount != lastFishCount)
            {
                UpdateDisplay(currentPool);
                lastPool = currentPool;
                lastFishCount = currentFishCount;
            }
        }
        else if (lastPool != null)  // If we were looking at a pool but now we're not
        {
            UpdateDisplay(null);
            lastPool = null;
            lastFishCount = -1;
        }
    }

    private void UpdateDisplay(PoolManager? pool)
    {
        if (pool != null)
        {
            int currentFish = (int)pool.getNumberOfFish();
            string poolName = pool.gameObject.name;
            
            // For debugging
            Debug.Log($"Updating display for {poolName}: {currentFish}/{MAX_FISH} fish");
            
            fishCountText.text = $"{poolName}: {currentFish}/{MAX_FISH} fish";
            
            // Color coding based on capacity
            if (currentFish >= MAX_FISH)
            {
                fishCountText.color = Color.red;
            }
            else if (currentFish >= MAX_FISH * 0.5f)  // Over 80% capacity
            {
                fishCountText.color = new Color(1.0f, 0.5f, 0);  // Orange
            }
            else
            {
                fishCountText.color = Color.white;
            }
        }
        else
        {
            fishCountText.text = "No Pool Selected";
            fishCountText.color = Color.gray;
        }
    }
}