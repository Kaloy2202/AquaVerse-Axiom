using System;
using System.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private SceneMngrState sceneMngrState;
    private float poolTemperature = 27;
    private string poolStatus = "calm";

    private float excessFeed = 0; //amount of feed that is not eaten by fish in grams

    private float dissolvedOxygenContent = 0;

    void Start(){
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        dissolvedOxygenContent = calcDissolvedOxygenContent();//set the initial value for the dissolved oxygen content of the pool
        StartCoroutine(decomposeFeeds());
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
        Debug.Log("remaining oxygen" + dissolvedOxygenContent);
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
    
    
}
