using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolRestrictoin : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    // Start is called before the first frame update
    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("Player")){
            sceneMngrState.setCanDoPondActions(true);
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.CompareTag("Player")){
            sceneMngrState.setCanDoPondActions(false);
        }
    }
}
