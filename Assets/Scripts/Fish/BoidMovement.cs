using UnityEngine;
using ABMU.Core;
using System.Collections.Generic;

public class BoidMovement : AbstractAgent
{
    private Vector3 velocity;
    private List<GameObject> fishInRange = new List<GameObject>();
    private SceneMngrState sceneMngr;
    private float speedMultiplier = 1;
    private Vector3 biasDirection;
    private bool hasBias;

    private float top, bottom, left, right, front, back;
    private PoolManager poolManager;
    public override void Init()
    {
        base.Init();
        //reference to the scene manager
        sceneMngr = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        CreateStepper(Move);
        //initial speed multiplier
        speedMultiplier = 1;
    }

    public void setBounds(Vector3 center, Vector3 dimensions){
        float x = dimensions.x/2;
        float y = dimensions.y/2;
        float z = dimensions.z/2;

        top = center.y+ y;
        bottom = center.y- y;
        left = center.x -x;
        right = center.x+x;
        front = center.z + z;
        back = center.z- z;
    }

    //moves the boid based on the value of the velocity property
    private void Move(){
        velocity += calcAlignmentVector() * sceneMngr.getMatchingFactor();
        velocity += calcSeperationVector() * sceneMngr.getAvoidFactor();
        velocity += calcCohesionVector() * sceneMngr.getCenteringFactor();
        // if there is a bias e.g., there is a food source, prioritize going to the biased position
        if(hasBias){
            calcBiasEffect();
        }
        //seperation is important for the movement

        // apply only 80% percent of the direction in the y axis
        //prevents too much movement of the boid along the y axis
        velocity.y *= 0.8f;
        //normalize the value
        velocity = velocity.normalized;
        //check if boid is outside of the specified pond area
        //mechanic for preventing the boid from going out the area and provide a smooth transition
        agentOutsideOfPond();

        //if velocity is less than the set min speed, make it equal to the min speed
        if(velocity.magnitude < sceneMngr.getMinSpeed()){
            velocity = velocity.normalized * sceneMngr.getMinSpeed();
        }
        //if velocity is greater than the set max speed, make it equal to the max speed
        if(velocity.magnitude > sceneMngr.getMaxSpeed()){
            velocity = velocity.normalized * sceneMngr.getMaxSpeed();
        }
        //change the position of the boid based on the calculated velocity
        transform.position += velocity * Time.deltaTime * speedMultiplier; //* growth.getWeightBasedSpeedMultiplier();
        //rotate the boid to face the direction stated by the velocity
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    void OnTriggerEnter(Collider collider){
        //adds neighbor 
        // used to efficiently manage the neighbors and keeps the number of neighbors to limited number for the sake of performance
        if(collider.CompareTag("fish") && !fishInRange.Contains(collider.gameObject) && fishInRange.Count< 10){
            fishInRange.Add(collider.gameObject);
        }
    }
    //remove the gameobject from the neighbors list when it is outside of the range of the boid
    void OnTriggerExit(Collider collider){
        if(fishInRange.Contains(collider.gameObject)){
            fishInRange.Remove(collider.gameObject);
        }
    }
//calculates the amount of seperation the boid will make from close neighbors
private Vector3 calcSeperationVector() {
    Vector3 vect = Vector3.zero;
    //loop through the registered neighbors
    foreach (GameObject gameObj in fishInRange) {
        Vector3 difference = transform.position - gameObj.transform.position;
        //distance of the boid from its neighbor
        float distanceSquared = difference.sqrMagnitude;
        //if distance of neighbor is less than or equal to the 80%  of the length of the boid then include it to the neighbors to keep distance from
        if (distanceSquared <= transform.localScale.x * 2) {
            vect += difference.normalized / Mathf.Max(distanceSquared, 0.01f); // Prevent division by extremely small values
        }
    }
    return vect.normalized;
}
    //calculates the amount of adjustment the boid will take to center itself within the mass of neighbor boids
    private Vector3 calcCohesionVector() {
        Vector3 vect = Vector3.zero;
        //loop through the registered neighbor boids
        foreach(GameObject gameObj in fishInRange) {
            float angle = Vector3.Angle(transform.forward, (gameObj.transform.position - transform.position).normalized);
            if(angle <= 90) { // Check if within 90 degrees of the forward direction, 180 degree field of view
                vect += gameObj.transform.position;
            }
        }
        //do this when there is neighbor, otherwise there is no mass to center the boid
        if (fishInRange.Count > 0) {
            vect /= fishInRange.Count; // Average position
            vect -= transform.position; // Steer towards the center of mass
        }
        return vect.normalized;
    }
    //calculates the amount of adjustment the boid will make to keep up with the neighbors velocity
    private Vector3 calcAlignmentVector() {
        Vector3 vect = Vector3.zero;
        //loop through the neigboring boids
        foreach(GameObject gameObj in fishInRange) {
            float angle = Vector3.Angle(transform.forward, (gameObj.transform.position - transform.position).normalized);
            //check if neighbor is within the 180 degeree field of view of the boid
            // if it is then add its velocity to the velocity the boid will need to catch up to
            if(angle <= 90) {
                BoidMovement boid = gameObj.GetComponent<BoidMovement>();
                vect += boid.getAgentVelocity();  
            }
        }
        if (fishInRange.Count > 0) {
            vect /= fishInRange.Count + 1; // Average the velocities
            vect -= velocity;
        }
        return vect.normalized;
    }
    //calculates the bias in direction of the boid
    //used when there is an event that will change the movement of the boid, such as when food is found
    private void calcBiasEffect(){
        //calculate the direction of the bias direction from the boid and muultiply the value of the velocity with the bias factor
        velocity += (biasDirection- transform.position ) * sceneMngr.getBiasFactor();
    }
    //prevents the boid from going out of the specified range
    private void agentOutsideOfPond(){
        Vector3 pos = transform.position;
        //the amount of rotation to apply to the boid to prevent it from going outside of range
        float turnFac = sceneMngr.getRotateFactor();
        //when position is outside of specified range for right
        if(pos.x > sceneMngr.getRight()){
            velocity.x -= turnFac;
        }
        //when position is more than the specified range for left
        if(pos.x < sceneMngr.getLeft()){
            velocity.x += turnFac;
        }
        //when position is more than the specified range for top
        //bigger penalty here to prevent the boid from going outside of water
        if(pos.y > sceneMngr.getTop()){
            velocity.y -= 0.5f;
        }
        //when position is more than the specified range for bot
        if(pos.y < sceneMngr.getBot()){
            velocity.y += turnFac;
        }
        //when position is more than the specfied range for back
        if(pos.z < sceneMngr.getBack()){
            velocity.z += turnFac;
        }
        //when position is more than the specified range for front
        if(pos.z > sceneMngr.getFront()){
            velocity.z -= turnFac;
        }
    }
    // used to get the value of the velocity of the boid
    public Vector3 getAgentVelocity(){
        return this.velocity;
    }

    public void setAgentVelocity(Vector3 velocity){
        this.velocity = velocity;
    }
    //used to set the value of the biasdirection
    public void setBiasDirection(Vector3 dir){
        biasDirection = dir;
    }
    //used to set if the boid is biased or not
    //biasDirection cannot do this since Vector3 is non-nullable
    public void setHasBias(bool value){
        hasBias = value;
    }
    //set the speed multiplier/ acceleration of the object
    //constant
    //used to have variety of speed for the boid for individual activity (eating, moving, etc)
    public void setSpeedmultiplier(float newValue){
        speedMultiplier = newValue;
    }


}
