using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public string title = "Beginner";
    public int money = 150000;

    //UI elements to display the stats
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI titleText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purpooses
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExperience(10);
        }
    }

    //Metood to handle gaining experience
    public void GainExperience(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();

    }

    //Method to hhandle leveling up
    void LevelUp()
    {
        experience -= experienceToNextLevel;
        level++;
        experienceToNextLevel += 100;
        UpdateTitle();

        //add bonuses here
    }

    void UpdateTitle()
    {
        if (level >= 1 && level < 5)
            title = "Beginner";
        else if (level >= 5 && level < 10)
            title = "Intermediate";
        else if (level >= 10)
            title = "Expert";

        UpdateUI();
    }

    //method to handle UI update
    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        experienceText.text = "XP: " + experience + "/" + experienceToNextLevel;
        titleText.text = title;
        moneyText.text = "" + money;
    }

    //Method to add money
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }
}
