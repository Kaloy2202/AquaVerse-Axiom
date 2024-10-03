using UnityEngine;
using ABMU.Core;
using System;
using System.Linq;
using System.Collections;

public class Growth : AbstractAgent
{
    private BoidMovement boid;
    private GameObject feedObject;
    private SceneMngrState sceneMngr;
    //eating
    private bool isEating;
    private float consumedFeed;
    private float feedCap;
    private float biteSize;

    //growth 
    private float FeedConversionRatio; // typical FCR for Bangus: 1.5 to 2.0
    private float Temperature; // water temperature in degrees Celsius, temperature of the water
    private const float ME = 0.6f; // Metabolizable energy coefficient
    private float FeedIntakeRatePerHour; // Feed intake rate per hour in grams
    private float digestionRate;
    private int age; //age in days
    private float weight;
    private float s;
    private float t0;
    private float baseIntakeRate;
    private float optimalTemp;

    //constants
    float averageAdultWeight = 800; //in grams
    public override void Init(){
        base.Init();
        //reference to the boid movement script of the gameobject
        boid = GetComponent<BoidMovement>();
        //reference to the scene manager, the one that holds in-game/in-scene values
        sceneMngr = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        //register the steppers
        CreateStepper(Eat);
        CreateStepper(Grow);
        //growth related
        //rate of consumption of the fish
        //create a function that accepts mass/size as parameter to calculate bite size
        biteSize = 5;
        //necessary for growth
        //double check this later
        s = 0.262f;
        t0 = 69.8f;
        //initial weight in grams
        weight = 50;
        //maximum amount of feed the fish can consume
        //based on data, find it later
        baseIntakeRate = weight *0.1f;
        //optimal temperature for bangus
        optimalTemp = 29;
        //initial curent temperation
        Temperature = 29;
        //calculate the maximum feed the fish can consume 
        calcFeedIntake();
        //date based conversion ratio
        //double check later
        FeedConversionRatio = 0.5f;
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
        }if(feedObject != null){
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
        if(feed.Count() > 0){
            //set the first one on the array as the feed object
            //randomizes the feedobject as Physics.Overlap does not sort the results based on distance
            feedObject = feed[0].gameObject;
            //set the bias direction
            boid.setBiasDirection(feedObject.gameObject.transform.position);
            //let the boid know that it should prioritize the bias
            boid.setHasBias(true);
            //boid should speed up approaching the food
            boid.setSpeedmultiplier(2);
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
            //perform asynchronous digestion
            StartCoroutine(performDigestion());
        }
    }
    //digests the feed consumed
    //necessary for the growth of the fish
    IEnumerator performDigestion(){
        //execute only if fish has consumed feed and is not eating
        while(consumedFeed > 0 && !isEating){
            //calculate the digestion rate of the fish
            calcDigestionRate();
            //decrement the amount of consumed feed by the digestion rate
            consumedFeed -= digestionRate;
            //calculate the growth of the fish
            calcGrowth();
            //do this every one second (1 sec = 1 hour)
            yield return new WaitForSeconds(1);
        }
        //update the maximum feed intake
        calcFeedIntake();
    }
    //checks if the fish is hungry or not
    private bool isHungry(){
        //fish is hungry when thre is difference in consumed feed and feed cap
        return consumedFeed < feedCap;
    }

    //calculations
    private void calcDigestionRate(){
        //gram per hour
        //amount of feed in grams that is digested by the fish in 1 hour
        double weightFactor = Math.Pow(weight,s);
        double tempFactor = Math.Exp(0.0806 * (20 -Temperature));
        float dig = (float)(weight * Math.Pow(1/(t0 * weightFactor * tempFactor), 1/0.62));
        digestionRate = dig;
    }
    private void calcFeedIntake() {
        double exponent = -Math.Pow(Temperature - optimalTemp, 2) / Math.Pow(3, 2);
        double tempFactor = Math.Exp(exponent);
        //24 hour eating capacity
        feedCap = (float)(weight/50 * tempFactor * baseIntakeRate); 
        //one hour feeding capacity
        FeedIntakeRatePerHour = feedCap/24;
        biteSize = FeedIntakeRatePerHour; //divide the feed intake per hour to the number of bites it takes a fish to eat 1 gram of feed
    }
    //calculates the new weight of the fihs
    private void calcGrowth(){
        // the additional weight is the amount of digested feed in an hour time the feed conversion ratio
        weight += digestionRate * FeedConversionRatio;
    }

    //visual growth
    private void Grow (){
        //fish gameobject scales with weight
        // transform.localScale = new Vector3(weight/averageAdultWeight,weight/averageAdultWeight, weight/averageAdultWeight);
    }
    //caculates the speed/acceleration of the fish based on weight
    public float getWeightBasedSpeedMultiplier(){
        // not final
        return 2 * weight /averageAdultWeight;
    }
}
