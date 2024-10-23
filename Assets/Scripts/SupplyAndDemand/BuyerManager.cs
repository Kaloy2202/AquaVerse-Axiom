using System.Collections.Generic;
using UnityEngine;

public class BuyerManager : MonoBehaviour
{
    public GameObject buyerCardPrefab;  // Prefab for creating buyer cards
    public Transform buyerUIPanel;      // UI panel where buyer cards are displayed

    private List<Buyer> buyers = new List<Buyer>();

    void Start()
    {
        // Clear any initial buyers/cards already in the BuyerCardUI panel
        DisableExistingBuyerCards();

        // Automatically generate new buyers after clearing the old ones
        GenerateInitialBuyers(9); // Example: Create 30 new buyers
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
            CreateBuyer("Buyer " + (i + 1), "Fish for Restaurant", Random.Range(10, 200), Random.Range(130, 200), Random.Range(10f, 30f));
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
        // Logic for supplying the buyer
        Debug.Log("Supplied buyer: " + buyer.Name);
        RemoveBuyer(buyer);
    }

    void DenyBuyer(Buyer buyer)
    {
        // Logic for denying the buyer
        Debug.Log("Denied buyer: " + buyer.Name);
        RemoveBuyer(buyer);
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
}
