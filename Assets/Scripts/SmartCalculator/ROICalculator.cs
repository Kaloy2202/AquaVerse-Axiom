using UnityEngine;
using TMPro;
using System.Globalization;

public class SmartCalculator : MonoBehaviour
{
    private CultureInfo philippineCulture = new CultureInfo("en-PH");

    // Input fields
    public TMP_InputField stockingNumberInput;
    public TMP_InputField costPerFingerlingInput;
    public TMP_InputField survivalRateInput;
    public TMP_InputField averageWeightInput;
    public TMP_InputField feedCostInput;
    public TMP_InputField marketPriceInput;
    public TMP_InputField fcrInput;
    public TMP_InputField operationalCostInput;
    public TMP_InputField pondSetupCostInput;

    // Output canvas
    public GameObject outputCanvas;
    public TMP_Text fishFryCostText;
    public TMP_Text harvestedWeightText;
    public TMP_Text grossRevenueText;
    public TMP_Text feedCostText;
    public TMP_Text totalCostText;
    public TMP_Text netProfitText;
    public TMP_Text roiText;

    // Input canvas
    public GameObject inputCanvas;

    public void CalculateROI()
    {
        // Validate inputs
        if (!ValidateInputs())
        {
            Debug.LogError("Invalid input detected! Please ensure all fields are filled correctly.");
            return;
        }

        try
        {
            // Parse inputs
            int stockingNumber = int.Parse(stockingNumberInput.text);
            float costPerFingerling = float.Parse(costPerFingerlingInput.text);
            float survivalRate = float.Parse(survivalRateInput.text) / 100f;
            float averageWeight = float.Parse(averageWeightInput.text);
            float feedCost = float.Parse(feedCostInput.text);
            float marketPrice = float.Parse(marketPriceInput.text);
            float fcr = float.Parse(fcrInput.text);
            float operationalCost = float.Parse(operationalCostInput.text);
            float pondSetupCost = float.Parse(pondSetupCostInput.text);

            // Perform calculations
            float fishFryCost = stockingNumber * costPerFingerling;
            float harvestedWeight = stockingNumber * survivalRate * averageWeight;
            float grossRevenue = harvestedWeight * marketPrice;
            float totalFeedCost = harvestedWeight * fcr * feedCost;
            float totalCost = fishFryCost + totalFeedCost + operationalCost + pondSetupCost;
            float netProfit = grossRevenue - totalCost;
            float roi = (netProfit / totalCost) * 100;

            // Update output canvas texts with localized currency and units
            fishFryCostText.text = $"{fishFryCost.ToString("C", philippineCulture)}";
            harvestedWeightText.text = $"{harvestedWeight:F2} kg";
            grossRevenueText.text = $"{grossRevenue.ToString("C", philippineCulture)}";
            feedCostText.text = $"{totalFeedCost.ToString("C", philippineCulture)}";
            totalCostText.text = $"{totalCost.ToString("C", philippineCulture)}";
            netProfitText.text = $"{netProfit.ToString("C", philippineCulture)}";
            roiText.text = $"ROI: {roi:F2}%";

            // Toggle canvases
            inputCanvas.SetActive(false);
            outputCanvas.SetActive(true);
        }
        catch
        {
            Debug.LogError("An unexpected error occurred during calculation.");
        }
    }

    public void ResetInputs()
    {
        // Clear input fields
        stockingNumberInput.text = "";
        costPerFingerlingInput.text = "";
        survivalRateInput.text = "";
        averageWeightInput.text = "";
        feedCostInput.text = "";
        marketPriceInput.text = "";
        fcrInput.text = "";
        operationalCostInput.text = "";
        pondSetupCostInput.text = "";

        // Toggle back to input canvas
        outputCanvas.SetActive(false);
        inputCanvas.SetActive(true);
    }

    private bool ValidateInputs()
    {
        if (!int.TryParse(stockingNumberInput.text, out int stockingNumber) || stockingNumber <= 0)
        {
            Debug.LogError("Stocking number must be a positive integer.");
            return false;
        }

        TMP_InputField[] decimalFields = {
            costPerFingerlingInput, survivalRateInput, averageWeightInput,
            feedCostInput, marketPriceInput, fcrInput, operationalCostInput, pondSetupCostInput
        };

        foreach (var field in decimalFields)
        {
            if (!float.TryParse(field.text, out float value) || value <= 0)
            {
                Debug.LogError($"Invalid input in field: {field.name}. Must be a positive decimal.");
                return false;
            }
        }

        if (float.Parse(survivalRateInput.text) > 100)
        {
            Debug.LogError("Survival rate must be between 0 and 100.");
            return false;
        }

        return true;
    }
}
