using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderIndicatorMngr : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject arrow;
    private Vector3 startingPos;
    void Start()
    {
        arrow = transform.Find("ArrowIndicator").gameObject;
        startingPos = arrow.transform.localPosition;
    }

    // Update is called once per frame

    public void setValue(float ratio){
        if(float.IsNaN(ratio) || float.IsInfinity(ratio) || ratio < 0){
            ratio = 0;
        }
        if(ratio > 1){
            ratio = 1;
        }
        ratio *= 15.2f;
        if(arrow == null){
            arrow = transform.Find("ArrowIndicator").gameObject;
        }
        arrow.transform.localPosition = new Vector3(startingPos.x + ratio, startingPos.y, startingPos.z);
    }
}
