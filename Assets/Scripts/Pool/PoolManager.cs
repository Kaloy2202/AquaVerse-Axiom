using System;
using System.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 dimensions;
    private SceneMngrState sceneMngrState;
    private float poolTemperature = 27;
    private string poolStatus = "calm";

    private float excessFeed = 0; //amount of feed that is not eaten by fish in grams

    private float dissolvedOxygenContent = 0;

    //temperature
    private float minPoolTemp = 24;
    private float maxPoolTemp = 32;

    //duration of one day in seconds
    private float numberSecondsForHour;

    void Start(){
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        dissolvedOxygenContent = calcDissolvedOxygenContent();//set the initial value for the dissolved oxygen content of the pool
        StartCoroutine(decomposeFeeds());
        numberSecondsForHour = sceneMngrState.getNumberOfSecondsPerHour();
        StartCoroutine(startDynamicTemp());
    }

    public float getPoolTemperature(){
        return this.poolTemperature;
    }

    public void setPoolTemperature(float temperature){
        this.poolTemperature = temperature;
    }

    public string getPoolStatus(){
        return this.poolStatus;
    }

    public void setPoolStatus(string status){
        this.poolStatus = status;  
    }

    public void updateExcessFeed(float value){
        this.excessFeed += value;
    }

    public float getExcessFeed(){
        return this.excessFeed;
    }

    public float calcPoolDOSaturation(){
        //in mg per liter
        float denominator =(float)( 1 + 0.032f * poolTemperature + 0.0002 * Math.Pow(poolTemperature, 2)); 
        return (float) 14.6/denominator;
    }

    public float calcDissolvedOxygenContent (){
        float doSaturation = calcPoolDOSaturation();
        Vector3 poolDimension = sceneMngrState.getPoolDimension();
        return (float)(poolDimension.x * poolDimension.y * poolDimension.z * doSaturation * 1000);
    }
    
    public float calcFeedDecompositionFeedRequirement(){
        return (float) Math.Pow(poolTemperature/20, 1.2);
    }

    public float calcOxygenRequirementByDecomposition(){
        float decompositionRate = (float) (0.1f * Math.Pow(poolTemperature/20, 1.2f));
        excessFeed -= excessFeed * decompositionRate;
        float oxygenRequirementPerGram = calcFeedDecompositionFeedRequirement();
        return decompositionRate * excessFeed * oxygenRequirementPerGram;
    }

    IEnumerator decomposeFeeds(){
        while(true){
            float decompositionOxyReq = calcOxygenRequirementByDecomposition();
            dissolvedOxygenContent -= decompositionOxyReq;
            yield return new WaitForSeconds(1);
        }

    }

    public bool isOxygenEnough(float value){
        if(value > dissolvedOxygenContent){
            dissolvedOxygenContent = 0;
            return false;
        }else{
            dissolvedOxygenContent -= value;
            return true;
        }
    }

    public float calcChangeInPHDueToTemp(float newTem){
        float changeInTemp = newTem - poolTemperature;
        return -((float) (changeInTemp/10) * 0.45f);

    }

    public Vector3 getCenter(){
        return this.center;
    }

    public Vector3 getDimensions(){
        return this.dimensions;
    }
    
        private void calcTemperature(){
        float timeFactor = (float)(1 + Math.Cos(2 * Math.PI/24 * (sceneMngrState.getInGameTime() - 14)));
        poolTemperature = minPoolTemp + ((maxPoolTemp - minPoolTemp) * (timeFactor/2));
    }

    IEnumerator startDynamicTemp(){
        while(true){
            calcTemperature();
            yield return new WaitForSeconds(numberSecondsForHour);
        }
    }

    
}
