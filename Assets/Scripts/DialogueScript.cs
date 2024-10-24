using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Canvas questCanvas;
    [SerializeField] private float textSpeed = 0.05f;
    
    private List<QuestData> quests = new List<QuestData>();
    private QuestData currentQuest;
    private int currentDialogueIndex;
    private Coroutine typingCoroutine;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeQuests();
        StartFirstQuest();
    }

    private void InitializeQuests()
    {
        // Initialize your quests here
        quests.Add(new QuestData
        {
            questId = "QUEST_1",
            dialogueLines = new string[] { 
            "After a long time, our waters are finally recovering, hopefull we'll be able to eat some edible fish from now on"
            , "Speaking of fish, I remember now! My parents once mentioned that your grandmother was a great aquaculture farmer, she was even called the Aqua Queen!",
            "They told me she kept all her secrets in a diary", "Go to your house and find that diary!" },
            requiredProgress = 1,
            questDescription = "Find the Aqua Queen's diary at your grandmother's house"
        });
        quests.Add(new QuestData
        {
            questId = "QUEST_2",
            dialogueLines = new string[] { 
            "You found the diary! Let's see what secrets it holds...",
            "It seems she has hidden some fish fry that was preserved using a cryogenic method",
            "It seems it is hidden in a storage house near your house",
            "Go to the storage house and find the fish fry!" },
            requiredProgress = 1,
            questDescription = "Find the cryogenically preserved fish fry"
        });
        quests.Add(new QuestData
        {
            questId = "QUEST_3",
            dialogueLines = new string[] { 
            "You found the cryogenically preserved fish fry! Now you can use the ponds inside your property to grow these fish fry!",
            "Go to the ponds and start the aquaculture process!" },
            requiredProgress = 1,
            questDescription = "Start the aquaculture process"
        });
        // Add more quests as needed
    }

    private void StartFirstQuest()
    {
        if (quests.Count > 0)
        {
            currentQuest = quests[0];
            currentDialogueIndex = 0;
            StartDialogue();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == currentQuest.dialogueLines[currentDialogueIndex])
            {
                NextDialogueLine();
            }
            else
            {
                CompleteCurrentLine();
            }
        }
    }

    private void StartDialogue()
    {
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeDialogueLine());
    }

    private IEnumerator TypeDialogueLine()
    {
        foreach (char letter in currentQuest.dialogueLines[currentDialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextDialogueLine()
    {
        if (currentDialogueIndex < currentQuest.dialogueLines.Length - 1)
        {
            currentDialogueIndex++;
            StartDialogue();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueText.text = currentQuest.dialogueLines[currentDialogueIndex];
    }

    public void UpdateQuestProgress(string questId, int progress)
    {
        var quest = quests.Find(q => q.questId == questId);
        if (quest != null && !quest.isCompleted)
        {
            if (progress >= quest.requiredProgress)
            {
                CompleteQuest(quest);
            }
        }
    }

    private void CompleteQuest(QuestData quest)
    {
        quest.isCompleted = true;
        int nextQuestIndex = quests.IndexOf(quest) + 1;
        
        if (nextQuestIndex < quests.Count)
        {
            currentQuest = quests[nextQuestIndex];
            currentDialogueIndex = 0;
            StartDialogue();
        }
        
        // Trigger any quest completion events/rewards here
    }

    // Public methods to interact with the quest system
    public bool IsQuestComplete(string questId)
    {
        return quests.Find(q => q.questId == questId)?.isCompleted ?? false;
    }

    public string GetCurrentQuestDescription()
    {
        return currentQuest?.questDescription ?? "No active quest";
    }
}