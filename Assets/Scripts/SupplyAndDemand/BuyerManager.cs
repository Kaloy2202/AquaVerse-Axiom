using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class BuyerManager : MonoBehaviour
{
    public GameObject buyerCardPrefab;  // Prefab for creating buyer cards
    public Transform buyerUIPanel;      // UI panel where buyer cards are displayed
    public TMP_Text playerMoneyText;  // Reference to the UI text element for player's money
    public TMP_Text playerStockText;  // Reference to the UI text element for player's stocks
    private PlayerStats playerStat;    // Reference to the PlayerStat script
    private FirstPersonController firstPersonController; // Reference to the PlayerStat script

    private List<Buyer> buyers = new List<Buyer>();  // List of active buyers
    private const int maxBuyersDisplayed = 12;        // Maximum number of buyers to display

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStats>(); // Find the PlayerStat component in the scene (assuming it's attached to the player)
        DisableExistingBuyerCards();                 // Clear any initial buyers/cards already in the BuyerCardUI panel
        firstPersonController = FindObjectOfType<FirstPersonController>();

        UpdatePlayerMoneyUI();
        UpdatePlayerStockUI();

        GenerateInitialBuyers();  // Generate the initial buyers, capped at 9

        EnableCursor();  // Enable the cursor when the UI is active
        DisablePlayerMovement();  // Disable player movement
    }

    void Update()
    {
        for (int i = buyers.Count - 1; i >= 0; i--)
        {
            Buyer buyer = buyers[i];
            buyer.UpdateTimer(Time.deltaTime);

            // Update the corresponding buyer card's timer display
            if (i < buyerUIPanel.childCount)
            {
                BuyerCard card = buyerUIPanel.GetChild(i).GetComponent<BuyerCard>();
                if (card != null)
                {
                    card.UpdateTimerDisplay();
                }
                else
                {
                    Debug.LogError("BuyerCard component is missing on child " + i);
                }
            }
            else
            {
                Debug.LogError("Child index out of bounds in buyerUIPanel");
            }

            // Check if a buyer's timer has expired
            if (buyer.IsExpired())
            {
                RemoveBuyer(buyer, buyerUIPanel.GetChild(i).gameObject);
            }
        }

        // Ensure only 9 buyers are displayed and generate new ones as needed
        if (buyers.Count < maxBuyersDisplayed && playerStat.availableStocks > 0)
        {
            GenerateNewBuyer();  // Generate new buyers if there are fewer than 9
        }
    }

    void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Unlocks the cursor
        Cursor.visible = true;  // Makes the cursor visible
    }

    void DisablePlayerMovement()
    {
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;  // Disable player movement
        }
    }

    // Clear all existing buyer cards in the UI panel
    void DisableExistingBuyerCards()
    {
        foreach (Transform child in buyerUIPanel)
        {
            child.gameObject.SetActive(false);  // Disable the card instead of destroying it
        }
    }

    // Generate initial buyers, ensuring no more than 9 are displayed
    void GenerateInitialBuyers()
    {
        int buyersToGenerate = Mathf.Min(maxBuyersDisplayed, playerStat.availableStocks > 0 ? 9 : 0);
        for (int i = 0; i < buyersToGenerate; i++)
        {
            GenerateNewBuyer();
        }
    }

    // Generate a new buyer and add them to the UI, ensuring a maximum of 9 buyers are displayed
    void GenerateNewBuyer()
    {
        if (buyers.Count >= maxBuyersDisplayed || playerStat.availableStocks <= 0)
        {
            return;  // Don't generate more than 9 buyers, or if stocks are depleted
        }

        // Create a new buyer with random demand, price, and timer
        CreateBuyer("Buyer " + (buyers.Count + 1), "Restaurant", Random.Range(10, 200), Random.Range(130, 200), Random.Range(10f, 30f));
    }

    // Create a new buyer and instantiate its card in the UI
    void CreateBuyer(string name, string reason, int demand, int price, float timer)
    {
        GameObject buyerCardObj = GetDisabledBuyerCard();  // Try to reuse a disabled card
        if (buyerCardObj == null)
        {
            buyerCardObj = Instantiate(buyerCardPrefab, buyerUIPanel);  // Instantiate a new card if none are available
        }

        buyerCardObj.SetActive(true);  // Activate the card to make it visible

        BuyerCard buyerCard = buyerCardObj.GetComponent<BuyerCard>();
        Buyer newBuyer = new Buyer(name, reason, demand, price, timer);
        buyers.Add(newBuyer);

        buyerCard.SetupCard(newBuyer, SupplyBuyer, DenyBuyer);
    }

    GameObject GetDisabledBuyerCard()
    {
        foreach (Transform child in buyerUIPanel)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                return child.gameObject;  // Return the first disabled card found
            }
        }
        return null;  // No disabled card found
    }

    void SupplyBuyer(Buyer buyer)
    {
        Debug.Log("Supply button clicked for buyer: " + buyer.Name);
        if (playerStat.availableStocks >= buyer.Demand)
        {
            playerStat.DeductStocks(buyer.Demand);  // Deduct the demanded amount from the available stock
            playerStat.money += buyer.Price * buyer.Demand;  // Update player's money

            UpdatePlayerMoneyUI();
            UpdatePlayerStockUI();

            RemoveBuyer(buyer);
        }
        else
        {
            Debug.Log("Not enough stocks to supply buyer: " + buyer.Name);
        }
    }

    void DenyBuyer(Buyer buyer)
    {
        Debug.Log("Deny button clicked for buyer: " + buyer.Name);
        RemoveBuyer(buyer);  // Deny buyer and remove from UI
    }

    // Update the player's money display in the UI
    void UpdatePlayerMoneyUI()
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = playerStat.money.ToString();
        }
    }

    // Update the player's stock display in the UI
    void UpdatePlayerStockUI()
    {
        if (playerStockText != null)
        {
            playerStockText.text = playerStat.availableStocks.ToString() + " kg";
        }
    }

    // Remove a buyer and its corresponding card
    void RemoveBuyer(Buyer buyer, GameObject buyerCard = null)
    {
        buyers.Remove(buyer);
        if (buyerCard != null)
        {
            Destroy(buyerCard);
        }
        else
        {
            // Find and destroy the buyer's card in the UI
            for (int i = 0; i < buyerUIPanel.childCount; i++)
            {
                BuyerCard card = buyerUIPanel.GetChild(i).GetComponent<BuyerCard>();
                if (card.name == buyer.Name)
                {
                    Destroy(buyerUIPanel.GetChild(i).gameObject);
                    break;
                }
            }
        }
    }
}
