using System.Collections.Generic;
using UnityEngine;

public class BuyerManager : MonoBehaviour
{
    public GameObject buyerCardPrefab;
    public Transform buyerUIPanel;

    private List<Buyer> buyers = new List<Buyer>();

    void Start()
    {
        GenerateInitialBuyers(3); // Example: Create 3 initial buyers
    }

    void Update()
    {
        for (int i = buyers.Count - 1; i >= 0; i--)
        {
            Buyer buyer = buyers[i];
            buyer.UpdateTimer(Time.deltaTime);

            // Find the corresponding buyer card and update the timer display
            BuyerCard card = buyerUIPanel.GetChild(i).GetComponent<BuyerCard>();
            card.UpdateTimerDisplay();

            if (buyer.IsExpired())
            {
                RemoveBuyer(buyer, card.gameObject);
            }
        }
    }

    void GenerateInitialBuyers(int numBuyers)
    {
        for (int i = 0; i < numBuyers; i++)
        {
            CreateBuyer("Buyer " + (i + 1), "Fish for Restaurant", Random.Range(10, 50), Random.Range(5f, 10f), Random.Range(10f, 30f));
        }
    }

    void CreateBuyer(string name, string reason, int demand, float price, float timer)
    {
        Buyer newBuyer = new Buyer(name, reason, demand, price, timer);
        buyers.Add(newBuyer);

        // Instantiate buyer card and set it up
        GameObject buyerCardObj = Instantiate(buyerCardPrefab, buyerUIPanel);
        BuyerCard buyerCard = buyerCardObj.GetComponent<BuyerCard>();

        buyerCard.SetupCard(newBuyer, SupplyBuyer, DenyBuyer);
    }

    void SupplyBuyer(Buyer buyer)
    {
        // Implement your supply logic, e.g., deduct fish stock, add money, etc.
        Debug.Log("Supplied buyer: " + buyer.Name);
        RemoveBuyer(buyer);
    }

    void DenyBuyer(Buyer buyer)
    {
        Debug.Log("Denied buyer: " + buyer.Name);
        RemoveBuyer(buyer);
    }

    void RemoveBuyer(Buyer buyer, GameObject buyerCard = null)
    {
        buyers.Remove(buyer);
        if (buyerCard != null)
        {
            Destroy(buyerCard);
        }
        else
        {
            // Find the buyer card in the UI and remove it
            for (int i = 0; i < buyerUIPanel.childCount; i++)
            {
                BuyerCard card = buyerUIPanel.GetChild(i).GetComponent<BuyerCard>();
                if (card.name == buyer.Name) // Check by name or other unique identifier
                {
                    Destroy(buyerUIPanel.GetChild(i).gameObject);
                    break;
                }
            }
        }

        // Replace with a new buyer
        CreateBuyer("New Buyer", "New Reason", Random.Range(10, 50), Random.Range(5f, 10f), Random.Range(10f, 30f));
    }
}
