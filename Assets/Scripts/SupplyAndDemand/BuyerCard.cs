using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text reasonText;
    [SerializeField] private TMP_Text demandText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button supplyButton;
    [SerializeField] private Button denyButton;

    private Buyer buyer;

    public void SetupCard(Buyer buyer, System.Action<Buyer> onSupply, System.Action<Buyer> onDeny)
    {
        this.buyer = buyer;

        nameText.text = buyer.Name;
        reasonText.text = buyer.Reason;
        demandText.text = buyer.Demand + " kg";
        priceText.text = buyer.Price + " per kg";
        timerText.text = Mathf.Ceil(buyer.Timer) + "s";

        supplyButton.onClick.AddListener(() => destroyObject());
        denyButton.onClick.AddListener(() => onDeny(buyer));
    }

    public void UpdateTimerDisplay()
    {
        if (buyer != null && timerText != null)
        {
            timerText.text = Mathf.Ceil(buyer.Timer) + "s";
        }
        else
        {
            Debug.LogError("Buyer or Timer Text is not assigned!");
        }
    }

    private void destroyObject()
    {
        Destroy(gameObject);
    }
}
