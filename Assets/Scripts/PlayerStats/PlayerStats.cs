using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public string title = "Beginner";
    public int money = 150000;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI titleText;

    public RewardPopupManager rewardPopupManager; // Reference to the RewardPopupManager

    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure this persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance exists
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // For testing purposes
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExperience(10);
        }
    }

    public void GainExperience(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        experience -= experienceToNextLevel;
        level++;
        experienceToNextLevel += 100;
        UpdateTitle();

        // Show the level-up popup using RewardPopupManager
        if (rewardPopupManager != null)
        {
            rewardPopupManager.ShowRewardPopup("Congratulations! You leveled up to Level " + level + "!");
        }

        UpdateUI();
    }

    void UpdateTitle()
    {
        if (level >= 1 && level < 5)
            title = "Beginner";
        else if (level >= 5 && level < 10)
            title = "Intermediate";
        else if (level >= 10)
            title = "Expert";
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        experienceText.text = "XP: " + experience + "/" + experienceToNextLevel;
        titleText.text = title;
        moneyText.text = "" + money;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }
}
