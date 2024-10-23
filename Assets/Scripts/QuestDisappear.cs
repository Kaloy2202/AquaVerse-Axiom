using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisappear : MonoBehaviour
{
    // Reference to the Canvas GameObject
    public GameObject canvas;

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse click (0 is the left mouse button)
        if (Input.GetMouseButtonDown(0))
        {
            // Disable the canvas by setting it to inactive
            canvas.SetActive(false);
        }
    }
}
