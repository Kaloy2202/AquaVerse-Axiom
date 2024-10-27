using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public string title = "Beginner";
    public float money = 1000;
    public int totalStocks = 1000;
    public int availableStocks = 1000;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI titleText;
    public GameObject SensorManager;
    public TextMeshProUGUI stocks;
    [SerializeField] private TMP_Text stockText;

    public RewardPopupManager rewardPopupManager;

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
        UpdateUI();
        SensorManager.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExperience(100);
        }
        
        // Reset to main menu when backspace is pressed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReturnToMainMenu();
        }
        
        if (level >= 2){
            SensorManager.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        // Reset all player stats to initial values
        level = 1;
        experience = 0;
        experienceToNextLevel = 100;
        title = "Beginner";
        money = 1000;
        totalStocks = 1000;
        availableStocks = 1000;

        // Update UI with reset values
        UpdateUI();
        
        // Deactivate sensor manager
        if (SensorManager != null)
        {
            SensorManager.SetActive(false);
        }

        try
        {
            // Load the MainMenu scene
            SceneManager.LoadScene("MainMenu");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load MainMenu scene. Make sure the scene is added to Build Settings! Error: " + e.Message);
        }

        // Since we're returning to main menu, we might want to destroy this instance
        // Uncomment the next line if you want to completely reset the PlayerStats instance
        // Destroy(gameObject);
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
        if (levelText != null) levelText.text = "Level: " + level;
        if (experienceText != null) experienceText.text = "XP: " + experience + "/" + experienceToNextLevel;
        if (titleText != null) titleText.text = title;
        if (moneyText != null) moneyText.text = "" + money;
        if (stocks != null) stocks.text = availableStocks + " grams";
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
            availableStocks = 0;
        }
    }

    private void UpdateStockUI()
    {
        if (stockText != null)
        {
            stockText.text = availableStocks.ToString() + " kg";
        }
    }
}