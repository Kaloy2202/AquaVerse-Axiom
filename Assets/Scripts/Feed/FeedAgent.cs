using UnityEngine;
using ABMU.Core;
using System.Collections;



public class FeedAgent : AbstractAgent
{
    private Vector3 direction;
    private float content;
    private Rigidbody rb;

    private float maxMagnitude = 1;
    public override void Init()
    {
        base.Init();
        //get the reference to the rigidbody of the component
        rb = GetComponent<Rigidbody>();
        //set the initial contenet
        //change in the future
        content = 10;
        //create stepper
        CreateStepper(Behave);
    }
    //determines the movement of the feed
    void Behave(){
        //if content is all consumed, destroy the game object
        if(content <= 0){
            Die();
        // use add force to determine the mvoement of the feed
        }else{
            if(rb){
                rb.AddForce(direction);
            }
        }
    }
    //destroy the game object
    void Die(){
        GameObject.Destroy(this.gameObject);
    }
    //calculates the amount of feed to return to the fish
    public float updateContentUponChange(float biteSize){
        if(biteSize > content){
            biteSize = content;
            content = 0;
            return biteSize;
        }
        content -= biteSize;
        return biteSize;
    }
    //calculates the direction where the feed will go
    //ideal opposite of the direction
    public void calculateMagnitude(Vector3 pos, float radius){
        float distance = Vector3.Distance(transform.position, pos);

        // If the point is outside the radius, return the minimum value
        if (distance > radius)
        {
            direction = Vector3.zero;
        }else{
            // Calculate the value based on the distance
            float magnitude = maxMagnitude * (1 - (distance / maxMagnitude));

            direction = (pos - transform.position).normalized * magnitude;
            direction.y = 0;
        }
    }
}
