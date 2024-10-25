using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#nullable enable

public class ActivitySensor : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    private InputManager inputManager;
    private FeedController feedController;
    private FishController fishController;
    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        inputManager = GameObject.Find("InputManager").GetComponentInParent<InputManager>();
        feedController = GameObject.Find("FeedController").GetComponent<FeedController>();
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();  // Initialize AudioManager
    }

    void Update()
    {
        int status = sceneMngrState.getStatus();

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
            fishController.spawnFish(pool);
            audioManager.Play("SpawnSound");  // Play sound for spawning fish
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
            audioManager.Play("FeedSound");  // Play sound for feeding fish
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
            audioManager.Play("WaterChangeSound");  // Play sound for changing water
        }
        else
        {
            Debug.Log("No selected pool");
        }
    }
}
