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
    // Start is called before the first frame update
    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        inputManager = GameObject.Find("InputManager").GetComponentInParent<InputManager>();
        feedController = GameObject.Find("FeedController").GetComponent<FeedController>();
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
    }
void Update()
{
    int status = sceneMngrState.getStatus();

    if(sceneMngrState.getCanDoPondActions()){
        if( status == 1 && Input.GetMouseButtonDown(0)){
            //spawn fish when status is 1
            PoolManager? pool = inputManager.getSelectedPool();
            if(pool != null){
                fishController.spawnFish(pool);
            }else{
                // Debug.Log("there is no pool manager script found");
            }
        }
        if(status == 0 && Input.GetMouseButtonDown(0)){
            Vector3[]? mousePos = inputManager.getMousePosition();
            if(mousePos != null){
                //0 is the position of the mouse
                //1 is the center of the pond
                //2 is the dimension of the pond
                feedController.generateFeeds(100, mousePos[0], mousePos[1], mousePos[2]);
            }else{
                Debug.Log("no pond hit detected");
            }
        }
    }
}

}
