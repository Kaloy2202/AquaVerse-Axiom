using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

    private BuyerManager buyerManager;

    public void SetupCard(Buyer buyer, System.Action<Buyer> onSupply, System.Action<Buyer> onDeny)
    {
        this.buyer = buyer;

        nameText.text = buyer.Name;
        reasonText.text = buyer.Reason;
        demandText.text = buyer.Demand + " kg";
        priceText.text = buyer.Price + " per kg";
        timerText.text = Mathf.Ceil(buyer.Timer) + "s";

        supplyButton.onClick.AddListener(() => onSupply(buyer));
        denyButton.onClick.AddListener(() => onDeny(buyer));
        denyButton.onClick.AddListener(()=>destroyObject());
        StartCoroutine(destroyAfter(buyer.Timer));
    }

    void Update(){

        UpdateTimerDisplay();
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

    public void setBuyerMngr(BuyerManager buyerManager){
        this.buyerManager = buyerManager;
    }

    IEnumerator destroyAfter(float time){
        yield return new WaitForSeconds(time);
        buyerManager.RemoveBuyer(buyer);
    }
    
    public void destroyObject(){
        Destroy(gameObject);
    }

    private void attemptToSupply(){
        // if()
    }
}
