using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolRestrictoin : MonoBehaviour
{
    private SceneMngrState sceneMngrState;

    [SerializeField] private GameObject[] poolList;
    // Start is called before the first frame update
    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        
    }


    void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("Player")){
            enableIndicators();
            sceneMngrState.setCanDoPondActions(true);
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.CompareTag("Player")){
            disableIndicators();
            sceneMngrState.setCanDoPondActions(false);
        }
    }

    private void disableIndicators(){
        foreach(GameObject go in poolList){
            GameObject poolIndicator = go.transform.Find("PondIndicator").gameObject;
            GameObject fishIndicator = go.transform.Find("FishIndicator").gameObject;
            poolIndicator.SetActive(false);
            fishIndicator.SetActive(false);
        }
    }

    private void enableIndicators(){
            foreach(GameObject go in poolList){
            GameObject poolIndicator = go.transform.Find("PondIndicator").gameObject;
            GameObject fishIndicator = go.transform.Find("FishIndicator").gameObject;
            poolIndicator.SetActive(true);
            fishIndicator.SetActive(true);
        }
    }
}
