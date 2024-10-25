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
            if(Input.GetKeyDown(KeyCode.Q)){
                //spawn
                sceneMngrState.setStatus(1);
            }
            if(Input.GetKeyDown(KeyCode.E)){
                //feed
                sceneMngrState.setStatus(0);
            }
            if(Input.GetKeyDown(KeyCode.G)){
                //change water
                sceneMngrState.setStatus(2);
            }
            if(Input.GetMouseButtonDown(0)){
                switch(status){
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

    private void handleSpawnStatus(){
        PoolManager? pool = inputManager.getSelectedPool();
        if(pool != null){
            fishController.spawnFish(pool);
        }else{
            // Debug.Log("there is no pool manager script found");
        }
    }

    private void handleFeedStatus(){
        (Vector3, PoolManager)? mousePos = inputManager.getHitPositionAndPoolObject();
        if(mousePos != null){
            //0 is the position of the mouse
            //1 is the center of the pond
            //2 is the dimension of the pond
            Vector3 pos = mousePos.Value.Item1;
            PoolManager poolManager = mousePos.Value.Item2;
            feedController.generateFeeds(100, pos, poolManager);
        }else{
            Debug.Log("no pond hit detected");
        }
                        
    }
    private void handleChangeWater(){
        PoolManager? pool = inputManager.getSelectedPool();
        if(pool != null){
            pool.changeWater();
        }else{
            Debug.Log("no selected pool");
        }
    }
}