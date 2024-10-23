using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivitySensor : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    private InputManager inputManager;

    private FeedController feedController;
    private FishController fishController;
    // Start is called before the first frame update
    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        inputManager = GameObject.Find("InputManager").GetComponentInParent<InputManager>();
        feedController = GameObject.Find("FeedController").GetComponent<FeedController>();
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
    }

    // Update is called once per frame
    void Update()
    {
        int status = sceneMngrState.getStatus();
        if(Input.GetMouseButtonDown(0)){
            Vector3? pos = inputManager.getMousePosition();
            if(pos != null){
                switch(status){
                    case 0:
                        feedController.generateFeeds(100, (Vector3)pos);
                        break;
                    case 1:
                        fishController.spawnFish((Vector3)pos);
                        break;
                }
            }else{
                Debug.Log("invalid area");
            }
        }
    }
}
