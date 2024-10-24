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
    private FirstPersonController firstPersonController; // Reference to teh PlayerStat script

    private List<Buyer> buyers = new List<Buyer>();

    void Start()
    {

        playerStat = FindObjectOfType<PlayerStats>(); // Find the PlayerStat component in the scene (assuming it's attached to the player)
        DisableExistingBuyerCards(); // Clear any initial buyers/cards already in the BuyerCardUI panel
        firstPersonController = FindObjectOfType<FirstPersonController>();


        // Update the UI with the initial player money
        UpdatePlayerMoneyUI();
        UpdatePlayerStockUI();

        // Automatically generate new buyers after clearing the old ones
        GenerateInitialBuyers(9); // Example: Create 30 new buyers

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

            if (buyer.IsExpired())
            {
                RemoveBuyer(buyer, buyerUIPanel.GetChild(i).gameObject);
            }
        }
    }

    void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Unlocks the cursor
        Cursor.visible = true;  // Makes the cursor visible
    }

    void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Locks the cursor to the center of the screen
        Cursor.visible = false;  // Hides the cursor
    }


    void DisablePlayerMovement()
    {
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;  // Disable player movement
        }
    }

    void EnablePlayerMovement()
    {
        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;  // Enable player movement
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

    // Generate new buyers and display them in the UI
    void GenerateInitialBuyers(int numBuyers)
    {
        for (int i = 0; i < numBuyers; i++)
        {
            CreateBuyer("Buyer " + (i + 1), "Restaurant", Random.Range(10, 200), Random.Range(130, 200), Random.Range(10f, 30f));
        }
    }

    // Create a new buyer and instantiate its card in the UI
    void CreateBuyer(string name, string reason, int demand, int price, float timer)
    {
        GameObject buyerCardObj = GetDisabledBuyerCard();  // Try to reuse a disabled card
        if (buyerCardObj == null)
        {
            // No disabled card available, so instantiate a new one
            buyerCardObj = Instantiate(buyerCardPrefab, buyerUIPanel);
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
        if (playerStat.availableStocks >= buyer.Demand)
        {
            // Deduct the buyer's demand from available stocks
            playerStat.DeductStocks(buyer.Demand);  // Deduct the demanded amount from the available stock
            playerStat.money += buyer.Price * buyer.Demand;  // Update player's money

            // Update the UI to reflect the new player money and stock
            UpdatePlayerMoneyUI();
            UpdatePlayerStockUI();

            Debug.Log("Supplied buyer: " + buyer.Name);
            RemoveBuyer(buyer);
        }
        else
        {
            Debug.Log("Not enough stocks to supply buyer: " + buyer.Name);
        }
    }

    void DenyBuyer(Buyer buyer)
    {
        // Logic for denying the buyer
        Debug.Log("Denied buyer: " + buyer.Name);
        RemoveBuyer(buyer);
    }

    // Update the player's money display in the UI
    void UpdatePlayerMoneyUI()
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = playerStat.money.ToString();
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
                if (card.name == buyer.Name) // Check by name or another unique identifier
                {
                    Destroy(buyerUIPanel.GetChild(i).gameObject);
                    break;
                }
            }
        }

        // Automatically create a new buyer to replace the old one
        CreateBuyer("New Buyer", "New Reason", Random.Range(10, 200), Random.Range(130, 200), Random.Range(10f, 30f));
    }
    void UpdatePlayerStockUI()
    {
        if (playerStockText != null)
        {
            playerStockText.text = playerStat.availableStocks.ToString() + " kg";
        }
    }

}
