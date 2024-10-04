using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayer;
    
    public Vector3 getSelectedMapPosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            lastPosition = hit.point;   
        }
        return lastPosition;
    }
}

