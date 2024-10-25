using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
#nullable enable

public class ActivitySensor : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    private InputManager inputManager;
    private FeedController feedController;
    private FishController fishController;
    private AudioManager audioManager;
    public TextMeshProUGUI currentFishCount;
    
    // Maximum number of fish allowed in the pool
    const int MAX_FISH = 200;

    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        inputManager = GameObject.Find("InputManager").GetComponentInParent<InputManager>();
        feedController = GameObject.Find("FeedController").GetComponent<FeedController>();
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        int status = sceneMngrState.getStatus();
        PoolManager poolManager = GameObject.FindObjectOfType<PoolManager>();
        currentFishCount.text = $"{poolManager.getNumberOfFish()}/{MAX_FISH}";
        if (sceneMngrState.getCanDoPondActions())
        {
            if (Input.GetKeyDown(KeyCode.Q)) { sceneMngrState.setStatus(1); }
            if (Input.GetKeyDown(KeyCode.E)) { sceneMngrState.setStatus(0); }
            if (Input.GetKeyDown(KeyCode.G)) { sceneMngrState.setStatus(2); }

            if (Input.GetMouseButtonDown(0))
            {
                switch (status)
                {
                    case 0:
                        handleSpawnStatus();
                        break;
                    case 1:
                        handleFeedStatus();
                        break;
                    case 2:
                        handleChangeWater();
                        break;
                }
            }
        }
    }

    private void handleSpawnStatus()
    {
        PoolManager? pool = inputManager.getSelectedPool();
        if (pool != null)
        {
            // Check if adding 100 more fish would exceed the maximum
            if (pool.getNumberOfFish() + 100 <= MAX_FISH)
            {
                fishController.spawnFish(pool);
                audioManager.Play("SpawnSound");
            }
            else
            {
                Debug.Log($"Cannot spawn more fish. Maximum limit of {MAX_FISH} would be exceeded.");
            }
        }
    }

    private void handleFeedStatus()
    {
        (Vector3, PoolManager)? mousePos = inputManager.getHitPositionAndPoolObject();
        if (mousePos != null)
        {
            Vector3 pos = mousePos.Value.Item1;
            PoolManager poolManager = mousePos.Value.Item2;
            feedController.generateFeeds(100, pos, poolManager);
            audioManager.Play("FeedSound");
        }
        else
        {
            Debug.Log("No pond hit detected");
        }
    }

    private void handleChangeWater()
    {
        PoolManager? pool = inputManager.getSelectedPool();
        if (pool != null)
        {
            pool.changeWater();
            audioManager.Play("WaterChangeSound");
        }
        else
        {
            Debug.Log("No selected pool");
        }
    }
}