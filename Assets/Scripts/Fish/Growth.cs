using UnityEngine;
using ABMU.Core;
using System;
using System.Collections;

public class Growth : AbstractAgent
{
    private BoidMovement boid;
    private GameObject feedObject;
    private SceneMngrState sceneMngr;
    private PoolManager poolManager;
    //eating
    private bool isEating;
    private float consumedFeed;
    private float feedCap; //in grams
    private float biteSize = 0.5f;
    private bool isDigesting = false;

    //growth 
    private float weight;
    private float highestWeightAttained;
    //constants
    private float averageAdultWeight = 800; //in grams

    public override void Init(){
        base.Init();
        //reference to the boid movement script of the gameobject
        boid = GetComponent<BoidMovement>();
        //reference to the scene manager, the one that holds in-game/in-scene values
        sceneMngr = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        weight = 50f; //starting weight
        //register the steppers
        CreateStepper(Eat);
        CreateStepper(Grow);

        feedCap = calcMaxFeedIntake();
        StartCoroutine(performDigestion());
        biteSize = weight * 0.02f;
        poolManager.updateTotalFishMass(weight);
    }

    //behavior of fish when eating
    void Eat(){
        //if hungry, make the fish be able to detect food
        if(isHungry()){
            //make sure the speed of fish is normal
            boid.setSpeedmultiplier(1);
            //look for food within the soroundings
            detectFood();
        //if there is food object
        //fish has found food
        }
        if(feedObject != null){
            //approach the food
            gotoFood();
        //fish is near the food, eat 
        }if(isEating && feedObject != null){
            //make the fish eat the food
            perFormEat();
        }
        //catch boids from rotating on the non existent feed positions
        //may happen if feed becomes null, has bias becomes true but the agent is still hungry
        if(feedObject == null){
            boid.setHasBias(false);
        }
    }
    //makes the fish detect food within the surounding
    private void detectFood(){
        //get the list of gameobjects on the food layer
        Collider[] feed = Physics.OverlapSphere(transform.position, 10, sceneMngr.getFoodLayer());
        //if there is food in the surrounding
        if (feed.Length > 0)
        {
            // Initialize variables to track the nearest feed
            GameObject nearestFeed = null;
            float minDistance = float.MaxValue;

            foreach(Collider coll in feed){
                float distance = Vector3.Distance(transform.position, coll.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestFeed = coll.gameObject;
                }
            }

            // If a nearest feed is found, update the boid's behavior
            if (nearestFeed != null)
            {
                feedObject = nearestFeed;
                boid.setBiasDirection(feedObject.transform.position);
                boid.setHasBias(true);
                boid.setSpeedmultiplier(2); // Speed up when approaching the food
            }
        }
        else
        {
            feedObject = null;
        }
    }
    //approaches the detected food
    private void gotoFood(){
        //moving towards the food is done by the boid movement script
        // when the fish is near the food, not necessarily in the position of the food
        //perform eating
        if((transform.position - feedObject.transform.position).magnitude < 0.5f){
            //set isEating to true
            //let the script know that the next action is that it should eat
            isEating = true;
            //slows down on the food to eat
            boid.setSpeedmultiplier(0.5f);
        }
    }
    //makes the fish eat
    private void perFormEat(){
        //reference to the feed script, FeedAgent
        FeedAgent feed = feedObject.GetComponent<FeedAgent>();
        //increment the consumeFeed variable with the result of updateContentUponChange method which requires the bitesize of the fish
        consumedFeed += feed.updateContentUponChange(biteSize);
        //make a splash, moves feeds within the radius away from the center
        //broadcasts the location to other fishes
        sceneMngr.createSplash(weight, transform.position);
        //if fish is full
        if(!isHungry()){
            //set the bias to false
            //let the boid know that the fish is full
            boid.setHasBias(false);
            //set feedobject to null
            //allows the fish to find food when it is hungry
            feedObject = null;
            //boid to move in normal speed
            boid.setSpeedmultiplier(1);
            //set the isEating to false
            //prevents further execution of this method
            isEating = false;
        }
    }
    private bool isHungry(){
        //fish is hungry when thre is difference in consumed feed and feed cap
        if(consumedFeed > feedCap * 0.75){
            return false;
        }
        return true;
    }

    //visual growth
    private void Grow (){
        float size = 5 * weight/averageAdultWeight;
        if(size > 1){
            size = 1;
        }
        if(size < transform.localScale.x){
            Vector3 prev = transform.localScale;
            transform.localScale = new Vector3(size, prev.y, prev.z);
        }else{
            transform.localScale = new Vector3(size, size, size);
        }
    }
    
    //calculates the amount of energy used for metabolism
    private float calcBasalMetabolism(float temperature){
        //a, b, c and tempRef are species specific values
        float a = 0.25f;
        float b = 0.75f;
        float c = 0.7f;
        float tempRef = 28;
        return (float)(a * Math.Pow(weight/1000, b) * Math.Exp(c * (temperature - tempRef)));
    }

    private float calcTempBaseMetabolism(float bmr, float temperature){
        float q10 = 2;
        float tempRef = 28;
        return (float)(bmr * Math.Pow(q10, (temperature - tempRef)/10));
    }

    private float calcMetabolicCost(){
        float temperature = poolManager.getPoolTemperature();
        float bmr = calcBasalMetabolism(temperature);
        float tempBaseMetabolism = calcTempBaseMetabolism(bmr, temperature);
        return (float)(bmr + tempBaseMetabolism);
    }

    //amount of feeds digesed per hour
    private float calcDigestionrate(){
        //feedcap shoule be set first before calculating digestion rate
        return feedCap/24;
    }

    private float calcFeedingRate(){
        //returns percentage of body weight
        float frBase = calcFRbase();
        float tempDiff = (float)Math.Pow(poolManager.getPoolTemperature() - ((25+30)/2), 2);
        float tempFactor = (float) Math.Exp(tempDiff/(2 * Math.Pow((30-25)/2, 2)));
        return (float)(frBase * tempFactor)/100;
    }

    private float calcMaxFeedIntake(){
        //calculates the amount of feed in grams
        float feedingRate = calcFeedingRate();
        return weight * feedingRate;
    }

    //returns the ideal feeding rate based on weight
    private float calcFRbase(){
        float frBase = weight switch{
            <= 50 => 10,
            > 50 and <=125 => 8,
            > 125 and <= 200 => 6,
            > 200 and <= 250 => 5.5f,
            > 250 and  <=300 => 5,
            > 300 and <= 400 => 4,
            >400 and <= 500 => 3.5f,
            > 500 => 2.5f,
        };
        return frBase;
    }

    private float calcTotalEnergyIntake(){
        float digestionRate = calcDigestionrate();
        if(digestionRate > consumedFeed){
            digestionRate = consumedFeed;
        }
        consumedFeed -= digestionRate;
        return digestionRate * 18792;
    }
    private float calcGrowth(){
        float energyIntake = calcTotalEnergyIntake();
        float metabolism = calcMetabolicCost();
        if(!poolManager.isOxygenEnough(metabolism * 1879.2f * 0.2f)){
            Debug.Log("oxygen not enough");
            poolManager.updateTotalFishMass(-weight);
            poolManager.updateNumberOfFish(-1);
            Destroy(this.gameObject);
            return 0;
        }
        float energyUsedPercentage = calcActivityEnergyPercentage();
        float growthEnergy =  energyIntake - (metabolism * 18792) - (energyUsedPercentage * energyIntake);
        //returns value of converted energy to mass
        return growthEnergy/(10 * 4184);
    }
    
    private float calcActivityEnergyPercentage(){
        string poolStatus = poolManager.getPoolStatus();
        switch (poolStatus){
            case "calm":
                return 0.12f;
            case "mild":
                return 0.2f;
            case "stressed":
                return 0.275f;
        }
        //just in case poolstatus is unset
        return 0.12f;
    }

    IEnumerator performDigestion(){
        float multiplier;
        float weightGain;
        while(true){
            weightGain = calcGrowth();
            if(weightGain> 0){
                multiplier = 50;
            }else{
                multiplier = 10;
            }
            float prevWeight = weightGain;
            weight += multiplier * weightGain;
            if(weight > 1000){
                //cap of growth is 1000 grams
                weight = 1000;
            }
            else{
                if(weight <= highestWeightAttained * 0.5f){
                    poolManager.updateTotalFishMass(-prevWeight);
                    poolManager.updateNumberOfFish(-1);
                    Destroy(this.gameObject);
                    Debug.Log("dead");
                    break;
                }
            }
            poolManager.updateTotalFishMass(weight - prevWeight);
            if(weight > highestWeightAttained){
                highestWeightAttained = weight;
            }
            feedCap = calcMaxFeedIntake();
            biteSize = weight * 0.02f;
            yield return new WaitForSeconds(2);
        }
    }

    public void setPoolMngrRef(PoolManager poolManager){
        this.poolManager = poolManager;
    }

    public void resetFoodObject(){
        feedObject = null;
    }
}

