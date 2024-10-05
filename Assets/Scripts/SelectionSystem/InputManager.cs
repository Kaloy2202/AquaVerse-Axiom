using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //reference to the camera that captures the position of the mouse
    [SerializeField] private Camera sceneCamera;
    //keeps track of the last position
    [SerializeField] private Vector3 lastPosition;
    //layer where we wanted to to apply the raycast
    [SerializeField] private LayerMask placementLayer;
    
    public Vector3 getSelectedMapPosition(){
        //get the position of the mouse based on the input package
        Vector3 mousePos = Input.mousePosition;
        //prevents selection of position that is not included in the field of view of the camera
        mousePos.z = sceneCamera.nearClipPlane;
        //creates a ray from the camera in the position determined by the mouse position
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        //if there is a hit in the specified layer
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            //update the last position
            lastPosition = hit.point;   
        }
        //always return a vector3 even if there is no hit
        //returning the last position means returning the position that is valid
        return lastPosition;
    }

    public Vector3? getMousePosition(){
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            return hit.point;
        }
        return null;
    }
}

