using UnityEngine;

public class ROICalculator : MonoBehaviour
{
    public int fingerlingTaps = 0;
    public int feedTaps = 0;
    public int waterChangeTaps = 0;

    public float FingerlingCost = 3.0f;  // Cost per fingerling
    public float FeedCostPerKg = 40.0f;  // Cost per kilogram of feed
    public float WaterChangeCost = 500.0f; // Cost per water change

    private float totalRevenue = 0f;
    private float totalWeight = 0f;  // Total weight of harvest

    // Add expenses based on activity
    public void AddFingerlingExpense(int taps)
    {
        fingerlingTaps += taps;
    }

    public void AddFeedExpense(int taps)
    {
        feedTaps += taps;
    }

    public void AddWaterChangeExpense(int taps)
    {
        waterChangeTaps += taps;
    }

    // Add revenue from sales and track total harvest weight
    public void AddRevenue(float revenue, float harvestWeight)
    {
        totalRevenue += revenue;
        totalWeight += harvestWeight;
    }

    // Calculate total expenses
    public float CalculateTotalExpenses()
    {
        float fingerlingExpense = fingerlingTaps * 100 * FingerlingCost;
        float feedExpense = feedTaps * 0.2f * FeedCostPerKg;
        float waterExpense = waterChangeTaps * WaterChangeCost;

        return fingerlingExpense + feedExpense + waterExpense;
    }

    // Calculate ROI
    public float CalculateROI()
    {
        float totalExpenses = CalculateTotalExpenses();
        return (totalRevenue - totalExpenses) / totalExpenses * 100f;
    }

    // Getters for displaying detailed info
    public float GetFingerlingExpense() => fingerlingTaps * 100 * FingerlingCost;
    public float GetFeedExpense() => feedTaps * 0.2f * FeedCostPerKg;
    public float GetWaterExpense() => waterChangeTaps * WaterChangeCost;
    public float GetTotalRevenue() => totalRevenue;
    public float GetTotalWeight() => totalWeight;
}
