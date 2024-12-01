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
    public Image Ebtn;
    public Image Gbtn;
    public Image Qbtn;
    public TextMeshProUGUI EbtnText;
    public TextMeshProUGUI GbtnText;
    public TextMeshProUGUI QbtnText;
    
    // Maximum number of fish allowed in the pool
    const int MAX_FISH = 200;
    
    // Button animation parameters
    private float buttonRaiseAmount = 50f;  // Amount to raise the button in Y axis
    private Vector3 QbtnInitialPos;
    private Vector3 EbtnInitialPos;
    private Vector3 GbtnInitialPos;

    private bool buttonsVisible = false;

    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        inputManager = GameObject.Find("InputManager").GetComponentInParent<InputManager>();
        feedController = GameObject.Find("FeedController").GetComponent<FeedController>();
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        
        // Store initial button positions
        QbtnInitialPos = Qbtn.transform.position;
        EbtnInitialPos = Ebtn.transform.position;
        GbtnInitialPos = Gbtn.transform.position;

        // Hide all text initially
        HideAllButtonTexts();
        
        // Initially hide all buttons and show E button raised
        SetButtonsVisibility(false);
        RaiseButton(Qbtn);
    }

    void Update()
    {
        // Check if there's a selected pool
        PoolManager? selectedPool = inputManager.getSelectedPool();
        bool shouldShowButtons = selectedPool != null && sceneMngrState.getCanDoPondActions();
        
        // Update buttons visibility if needed
        if (shouldShowButtons != buttonsVisible)
        {
            SetButtonsVisibility(shouldShowButtons);


        }
        
        // Only process input if buttons are visible
        if (buttonsVisible == true)
        {
            int status = sceneMngrState.getStatus();

            if (Input.GetKeyDown(KeyCode.Q))
            { 
                ResetButtonPositionsAndTexts();
                RaiseButton(Qbtn);
                ShowButtonText(QbtnText);
                sceneMngrState.setStatus(1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            { 
                ResetButtonPositionsAndTexts();
                RaiseButton(Ebtn);
                ShowButtonText(EbtnText);
                sceneMngrState.setStatus(0);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            { 
                ResetButtonPositionsAndTexts();
                RaiseButton(Gbtn);
                ShowButtonText(GbtnText);
                sceneMngrState.setStatus(2);
            }

            if (Input.GetMouseButtonDown(0))
            {
                switch (status)
                {
                    case 1:
                        handleSpawnStatus();
                        PlayerStats.Instance.AddMoney(-200);
                        break;
                    case 0:
                        handleFeedStatus();
                        PlayerStats.Instance.AddMoney(-20);
                        break;
                    case 2:
                        handleChangeWater();
                        PlayerStats.Instance.AddMoney(-500);
                        break;
                }
            }
        }
    }

    private void SetButtonsVisibility(bool visible)
    {
        buttonsVisible = visible;
        
        // If making visible, ensure buttons are in their initial positions

        
        // Set the visibility of each button
        Qbtn.gameObject.SetActive(visible);
        Ebtn.gameObject.SetActive(visible);
        Gbtn.gameObject.SetActive(visible);
        GbtnText.gameObject.SetActive(visible);
        QbtnText.gameObject.SetActive(visible);
        EbtnText.gameObject.SetActive(visible);
    }

    private void ShowButtonText(TextMeshProUGUI text)
    {
        HideAllButtonTexts();
        text.gameObject.SetActive(true);
    }

    private void HideAllButtonTexts()
    {
        QbtnText.gameObject.SetActive(false);
        GbtnText.gameObject.SetActive(false);
        EbtnText.gameObject.SetActive(false);
    }

    private void RaiseButton(Image button)
    {
        Vector3 newPosition = button.transform.position;
        newPosition.y += buttonRaiseAmount;
        button.transform.position = newPosition;
    }

    private void ResetButtonPositionsAndTexts()
    {
        // Reset all buttons to their initial positions
        Qbtn.transform.position = QbtnInitialPos;
        Ebtn.transform.position = EbtnInitialPos;
        Gbtn.transform.position = GbtnInitialPos;
        
        // Hide all texts
        HideAllButtonTexts();
    }

    private void handleSpawnStatus()
    {
        PoolManager? pool = inputManager.getSelectedPool();
        if (pool != null)
        {
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