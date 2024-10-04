using System.Collections;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    void Update(){
        Vector3 mousePos = inputManager.getSelectedMapPosition();
        Debug.Log("mouse" + mousePos);
        Vector3Int gridposition = grid.WorldToCell(mousePos);
        Vector3 pos = grid.CellToWorld(gridposition);
        Vector3 offset = new Vector3(0.5f, 0, 0.5f); 
        Debug.Log("grid indicator" + pos);
        cellIndicator.transform.position = pos + offset;
        mouseIndicator.transform.position = mousePos;
    }
}
