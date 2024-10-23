using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] lines;
    public float textSpeed;
    public Canvas questCanvas;
    
    private int index;
    private Coroutine typingCoroutine;
    private int numberOfLines;

    // Start is called before the first frame update
    void Start()
    {
        questCanvas.gameObject.SetActive(false);
        numberOfLines = lines.Length;
        textDisplay.text = "";
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textDisplay.text == lines[index])
            {
                NextLine();
            }
            else
            {
                // Stop the typing coroutine and display the full line immediately
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                textDisplay.text = lines[index];
            }
        }
        if (index == numberOfLines - 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                questCanvas.gameObject.SetActive(true);
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textDisplay.text = "";
        foreach (char letter in lines[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            textDisplay.text = "";
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
