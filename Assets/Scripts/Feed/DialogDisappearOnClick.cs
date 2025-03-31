using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDisappearOnClick : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    bool canDoPondActions;
    bool isActive;
    public int exp;
    // Update is called once per frame
    void Start(){
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        sceneMngrState.setCanDoPondActions(false);
        isActive = true;

    }
    void Update()
    {           
        if (canDoPondActions == true && isActive == false)
        {
            sceneMngrState.setCanDoPondActions(false);
        }
    
        if (Input.GetMouseButtonDown(0))
        {

            // PlayerStats.Instance.GainExperience(exp);
            sceneMngrState.setCanDoPondActions(true);
            canDoPondActions = true;
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}
