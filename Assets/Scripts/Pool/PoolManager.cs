using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 dimensions;
    [SerializeField] private GameObject poolQualityIndicator;
    [SerializeField] private GameObject averageWeightIndicator;
    private SceneMngrState sceneMngrState;
    private float poolTemperature = 27;
    private string poolStatus = "calm";
    private float excessFeed = 0; // amount of feed that is not eaten by fish in grams
    private float dissolvedOxygenContent = 0;
    private int numberOfFish = 0;
    private float totalFishMass = 0;
    private bool startSpawn = false;
    public Canvas canvas;
    public TextMeshProUGUI textmsg;
    
    // Temperature
    private float minPoolTemp = 24;
    private float maxPoolTemp = 32;
    
    // Duration of one day in seconds
    private float numberSecondsForHour;

    [SerializeField] private float harvestTimeInSecs = 60f; // Set harvest time here

    void Start()
    {
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        dissolvedOxygenContent = calcDissolvedOxygenContent(); // Set initial value for the dissolved oxygen content of the pool
        StartCoroutine(decomposeFeeds());
        numberSecondsForHour = sceneMngrState.getNumberOfSecondsPerHour();
        StartCoroutine(startDynamicTemp());
        
        // Start the harvesting coroutine
    }

    void Update()
    {
        if (sceneMngrState.getCanDoPondActions())
        {
            SliderIndicatorMngr poolQual = poolQualityIndicator.GetComponent<SliderIndicatorMngr>();
            SliderIndicatorMngr weightIndicatr = averageWeightIndicator.GetComponent<SliderIndicatorMngr>();
            if (poolQual != null)
            {
                float oxygenRatio = getCurrentToIdealOxygenRatio();
                poolQual.setValue(oxygenRatio);
            }
            if (weightIndicatr != null)
            {
                float averageWeight = totalFishMass / numberOfFish;
                weightIndicatr.setValue(averageWeight / 1000);
            }
        }
        Debug.Log("Total fish mass: " + totalFishMass);
        Debug.Log("number of fish: "+numberOfFish);
        if (numberOfFish > 0 && startSpawn == false)
        {
            startSpawn = true;
            StartCoroutine(HarvestFishRoutine());
            Debug.Log("CoRoutine is repeating");
        }

    }

    IEnumerator HarvestFishRoutine()
    {
        yield return new WaitForSeconds(harvestTimeInSecs); // Wait for the specified harvest time
        PlayerStats.Instance.AddStocks((int)totalFishMass); // Add total fish mass to player inventory
        // PlayerStats.Instance.GainExperience(500);
        Debug.Log("Fish harvested and added to player inventory." + (int)totalFishMass);
        canvas.gameObject.SetActive(true);
        textmsg.text = $"You gained {totalFishMass} grams of fish! Press F2 to access the store and sell your proceeds!";
        totalFishMass = 0; // Reset total fish mass after harvesting
        numberOfFish = 0;
        GameObject[] fishes = GameObject.FindGameObjectsWithTag("fish");
        foreach(GameObject go in fishes){
            Destroy(go);
        }
        startSpawn = false;

    }

    public float getPoolTemperature() { return this.poolTemperature; }
    public void setPoolTemperature(float temperature) { this.poolTemperature = temperature; }
    public string getPoolStatus() { return this.poolStatus; }
    public void setPoolStatus(string status) { this.poolStatus = status; }
    public void updateExcessFeed(float value) { this.excessFeed += value; }
    public float getExcessFeed() { return this.excessFeed; }
    public float calcPoolDOSaturation()
    {
        float denominator = (float)(1 + 0.032f * poolTemperature + 0.0002 * Math.Pow(poolTemperature, 2));
        return (float)14.6 / denominator;
    }
    public float calcDissolvedOxygenContent()
    {
        float doSaturation = calcPoolDOSaturation();
        Vector3 poolDimension = sceneMngrState.getPoolDimension();
        return (float)(poolDimension.x * poolDimension.y * poolDimension.z * doSaturation * 1000);
    }
    
    public float calcFeedDecompositionFeedRequirement() { return (float)Math.Pow(poolTemperature / 20, 1.2); }
    public float calcOxygenRequirementByDecomposition()
    {
        float decompositionRate = (float)(0.1f * Math.Pow(poolTemperature / 20, 1.2f));
        excessFeed -= excessFeed * decompositionRate;
        float oxygenRequirementPerGram = calcFeedDecompositionFeedRequirement();
        return decompositionRate * excessFeed * oxygenRequirementPerGram;
    }
    IEnumerator decomposeFeeds()
    {
        while (true)
        {
            float decompositionOxyReq = calcOxygenRequirementByDecomposition();
            dissolvedOxygenContent -= decompositionOxyReq;
            yield return new WaitForSeconds(1);
        }
    }
    public bool isOxygenEnough(float value)
    {
        if (value > dissolvedOxygenContent)
        {
            dissolvedOxygenContent = 0;
            return false;
        }
        else
        {
            dissolvedOxygenContent -= value;
            return true;
        }
    }
    public float calcChangeInPHDueToTemp(float newTem)
    {
        float changeInTemp = newTem - poolTemperature;
        return -((float)(changeInTemp / 10) * 0.45f);
    }
    public Vector3 getCenter() { return this.center; }
    public Vector3 getDimensions() { return this.dimensions; }
    private void calcTemperature()
    {
        float timeFactor = (float)(1 + Math.Cos(2 * Math.PI / 24 * (sceneMngrState.getInGameTime() - 14)));
        poolTemperature = minPoolTemp + ((maxPoolTemp - minPoolTemp) * (timeFactor / 2));
    }
    IEnumerator startDynamicTemp()
    {
        while (true)
        {
            calcTemperature();
            yield return new WaitForSeconds(numberSecondsForHour);
        }
    }
    public float getCurrentToIdealOxygenRatio()
    {
        float ideal = calcDissolvedOxygenContent();
        if (dissolvedOxygenContent > ideal)
        {
            dissolvedOxygenContent = ideal;
        }
        float ratio = dissolvedOxygenContent / ideal;
        if (ratio > 0.75f) { poolStatus = "calm"; }
        else if (ratio > 0.3f) { poolStatus = "mild"; }
        else { poolStatus = "stressed"; }
        return ratio;
    }
    public void setFishNumber(int num) { numberOfFish = num; }
    public float getNumberOfFish() { return numberOfFish; }
    public void updateNumberOfFish(int num) { numberOfFish += num; }
    public void updateTotalFishMass(float mass) { totalFishMass += mass; }
    public void changeWater() { dissolvedOxygenContent = calcDissolvedOxygenContent(); }
}
