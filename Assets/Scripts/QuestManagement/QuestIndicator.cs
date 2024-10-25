using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestIndicator : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.Instance.isCurrentQuestCompleted()){
            text.gameObject.SetActive(true);
            Debug.Log("Quest Complete");
        }
        else
        {
            text.gameObject.SetActive(false);
            Debug.Log("Quest Incomplete");
        }
    }
}
