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

    public AudioManager audioManager; // Reference to AudioManager for quest sounds

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
            "Na dumduman ko pa sang una ang hambal sang kamal-aman parte sa industriya sang pangisda sa Iloilo."
            , "Maayo kay daw ga amat na balik ang estado sang katubigan. Excited na ako makatilaw sang gina hambal nila nga seafoods!",
            "Dali lang...", "nadumduman ko na!","Hambal nila ang imo lola ang ginhihngalanan nga Aqua Queen sang una!", "Hambal nila, may ara siya diary nga puno sang mga impormasyon parte sa aquaculture.",  "Go to your house and find that diary!" },
            requiredProgress = 1,
            questDescription = "Find the Aqua Queen's diary at your grandmother's house"
        });
        quests.Add(new QuestData
        {
            questId = "QUEST_2",
            dialogueLines = new string[] { 
            "Salamat kay nakita mo gid man! Abi lantawon ta kun ano ang unod...",
            "Siling diri, may ara kuno fish fry nga na preserve ang imo Lola paagi sa  cryo method.",
            "Kag, nakatago daw ini sa isa ka daan nga warehouse lapit sa inyo balay.",
            "Go to the storage house and find the fish fry!" },
            requiredProgress = 1,
            questDescription = "Find the cryogenically preserved fish fry"
        });
        quests.Add(new QuestData
        {
            questId = "QUEST_3",
            dialogueLines = new string[] { 
            "Hala! may ara gid man? Ano ni nga isda man?",
            "Kag may fish feeds pa!",
            "Pwede mo na ini ma gamit sa pagpa dako sang mga fish fry!",
            "Go to your pond and start growing your fish!" },
            requiredProgress = 1,
            questDescription = "Start growing your fish!"
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
        if( currentQuest != null && currentQuest.dialogueLines != null)
        {
            foreach (char letter in currentQuest.dialogueLines[currentDialogueIndex].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
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

        // Play quest completion sound
        if (audioManager != null)
        {
            audioManager.Play("QuestCompleteSound");
        }
        else
        {
            Debug.LogWarning("AudioManager is not assigned.");
        }
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
    public string GetCurrentQuestId()
    {
        return currentQuest?.questId ?? "No active quest";
    }
    public bool isCurrentQuestCompleted(){
        return currentQuest?.isCompleted ?? false;
    }
    public void StartDialogueForQuest(){
        StartDialogue();
    }
}