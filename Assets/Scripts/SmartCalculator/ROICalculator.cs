using UnityEngine;
using TMPro;

public class ROICalculator : MonoBehaviour
{
    // Input fields
    public TMP_InputField stockingNumberField;
    public TMP_InputField survivalRateField;
    public TMP_InputField averageWeightField;
    public TMP_InputField marketPriceField;
    public TMP_InputField FCRField;
    public TMP_InputField feedCostPerKgField;
    public TMP_InputField operationalCostField;
    public TMP_InputField pondSetupCostField;
    public TMP_InputField fingerlingCostField;

    // Output fields
    public TMP_Text fishFryCostText;
    public TMP_Text harvestedWeightText;
    public TMP_Text grossRevenueText;
    public TMP_Text feedCostText;
    public TMP_Text totalCostsText;
    public TMP_Text netProfitText;
    public TMP_Text ROIText;

    // Containers
    public GameObject InputContainer;
    public GameObject OutputContainer;

    public void CalculateROI()
    {
        // Get user inputs
        float stockingNumber = float.Parse(stockingNumberField.text);
        float survivalRate = float.Parse(survivalRateField.text);
        float averageWeight = float.Parse(averageWeightField.text);
        float marketPrice = float.Parse(marketPriceField.text);
        float FCR = float.Parse(FCRField.text);
        float feedCostPerKg = float.Parse(feedCostPerKgField.text);
        float operationalCost = float.Parse(operationalCostField.text);
        float pondSetupCost = float.Parse(pondSetupCostField.text);
        float fingerlingCost = float.Parse(fingerlingCostField.text);

        // Calculations
        float harvestedFishWeight = stockingNumber * survivalRate * averageWeight;
        float fishFryCost = stockingNumber * fingerlingCost;
        float grossRevenue = harvestedFishWeight * marketPrice;
        float feedCost = harvestedFishWeight * FCR * feedCostPerKg;
        float totalCosts = fishFryCost + feedCost + operationalCost + pondSetupCost;
        float netProfit = grossRevenue - totalCosts;
        float ROI = (netProfit / (fishFryCost + pondSetupCost + operationalCost)) * 100;

        // Populate output fields
        fishFryCostText.text = $"Fish Fry Cost: {fishFryCost:C}";
        harvestedWeightText.text = $"Harvested Weight: {harvestedFishWeight:F2} kg";
        grossRevenueText.text = $"Gross Revenue: {grossRevenue:C}";
        feedCostText.text = $"Feed Cost: {feedCost:C}";
        totalCostsText.text = $"Total Costs: {totalCosts:C}";
        netProfitText.text = $"Net Profit: {netProfit:C}";
        ROIText.text = $"ROI: {ROI:F2}%";

        // Switch to Output Container
        InputContainer.SetActive(false);
        OutputContainer.SetActive(true);
    }

    public void ResetInputs()
    {
        // Clear input fields
        stockingNumberField.text = "";
        survivalRateField.text = "";
        averageWeightField.text = "";
        marketPriceField.text = "";
        FCRField.text = "";
        feedCostPerKgField.text = "";
        operationalCostField.text = "";
        pondSetupCostField.text = "";
        fingerlingCostField.text = "";

        // Switch back to Input Container
        OutputContainer.SetActive(false);
        InputContainer.SetActive(true);
    }
}
