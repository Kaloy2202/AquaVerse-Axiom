#nullable enable
using System;
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
    private void Start()
    {
        // Hide cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
{
    // Example: Toggle cursor visibility with Escape key
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
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

    public Vector3[]? getMousePosition(){
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            GameObject pondObject = hit.collider.gameObject;
            if(pondObject.GetComponent<PoolManager>() != null){
                PoolManager poolManager = pondObject.GetComponent<PoolManager>();
                //0 is the position of the mouse
                //1 is the center of the pond
                //2 is the dimension of the pond
                Vector3[] returnable = {hit.point, poolManager.getCenter(), poolManager.getDimensions()};
                return returnable;
            }else{
                Debug.Log("no pool manager script found");
            }
        }
        return null;
    }
    public (Vector3, PoolManager)? getHitPositionAndPoolObject(){
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            GameObject pondObject = hit.collider.gameObject;
            if(pondObject.GetComponent<PoolManager>() != null){
                PoolManager poolManager = pondObject.GetComponent<PoolManager>();
                //0 is the position of the mouse
                //1 is the center of the pond
                //2 is the dimension of the pond
                return (hit.point, poolManager);
            }else{
                Debug.Log("no pool manager script found");
            }
        }
        return null;
    }
    public PoolManager? getSelectedPool(){
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer)){
            if(hit.collider != null && hit.collider.gameObject != null){
                Debug.Log(hit.collider.gameObject.name);
                return hit.collider.gameObject.GetComponent<PoolManager>();
            }
        }
        return null;
    }
    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

