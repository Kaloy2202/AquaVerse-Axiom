using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public string sceneToLoad;
    
    private void Start()
    {
        // Get the Button component
        Button button = GetComponent<Button>();
        
        // Add a listener for the click event
        button.onClick.AddListener(TransitionToScene);
    }

    private void TransitionToScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}