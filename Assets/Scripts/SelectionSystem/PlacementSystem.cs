using System.Collections;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //gameobject used for visualization
    [SerializeField] private GameObject cellIndicator;
    //reference to the input manager script
    [SerializeField] private InputManager inputManager;
    //grid object
    [SerializeField] private Grid grid;
    //offset of the tile used to identify the currently selected grid
    private Vector3 offset = new Vector3(0.5f, 0, 0.5f);
    void Update(){
        //get the position of the mouse within the screen
        Vector3 mousePos = inputManager.getSelectedMapPosition();
        //convert the position of the mouse to local position of grid
        Vector3Int gridposition = grid.WorldToCell(mousePos);
        //convert the cell converted position back to the world position
        Vector3 pos = grid.CellToWorld(gridposition);
        //update the position of the cell inidicator
        cellIndicator.transform.position = pos + offset;
    }
}
