using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public string title = "Beginner";
    public float money = 150000;
    public int totalStocks = 1000;
    public int availableStocks = 1000;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI titleText;
    public GameObject InputManager;
    public TextMeshProUGUI stocks;
    [SerializeField] private TMP_Text stockText;

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
        InputManager.SetActive(false);
    }

    private void Update()
    {
        // For testing purposes
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExperience(100);
        }
        if (level >= 2){
            InputManager.SetActive(true);
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
            rewardPopupManager.ShowRewardPopup("Congratulations! You leveled up to Level " + level + "!", level);
        }

        UpdateUI();
    }

    void UpdateTitle()
    {
        if (level >= 1 && level < 5)
            title = "Beginner";
        else if (level >= 5 && level < 10)
            title = "Intermediate";
        else if (level >= 15)
            title = "Aquaculture Tycoon";
        else if (level >= 10)
            title = "Expert";
    }

    public void AddStocks(int amount)
    {
        availableStocks += amount;
        UpdateStockUI();
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        experienceText.text = "XP: " + experience + "/" + experienceToNextLevel;
        titleText.text = title;
        moneyText.text = "" + money;
        stocks.text = availableStocks + " grams";
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void AddStock(int harvest)
    {
        totalStocks += harvest;
    }

    public void DeductStocks(int amount)
    {
        availableStocks -= amount;
        UpdateStockUI();
        if (availableStocks < 0)
        {
            availableStocks = 0;  // Ensure stocks don't go below 0
        }
    }

    private void UpdateStockUI()
    {
        // Assuming you have a TMP_Text stockText field to update the UI
        stockText.text = availableStocks.ToString() + " kg";
    }
}
