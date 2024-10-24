using UnityEngine;
using ABMU.Core;
using System.Collections;



public class FeedAgent : AbstractAgent
{
    private float content;
    private Rigidbody rb;

    private float maxMagnitude = 1;

    //reference to the pool end of dimension position
    private float top, bottom, left, right, front, back;

    private PoolManager parentPool;
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

        SceneMngrState sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
        StartCoroutine(decay());
    }
    //determines the movement of the feed
    void Behave(){
        //if content is all consumed, destroy the game object
        if(content <= 0){
            Die();
        // use add force to determine the mvoement of the feed
        }
        adjustWhenOutsidePool();
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
    public Vector3 calculateMagnitude(Vector3 pos, float radius){
        float distance = Vector3.Distance(transform.position, pos);

        // If the point is outside the radius, return the minimum value
        if (distance > radius)
        {
            return Vector3.zero;
        }else{
            // Calculate the value based on the distance
            float magnitude = maxMagnitude * (1 - (distance / maxMagnitude));

            Vector3 direction = (pos - transform.position).normalized * magnitude;
            direction.y = 0;
            return direction;
        }
    }
    //makes sure that the feed object does not move out of the pool
    private void adjustWhenOutsidePool (){
        Vector3 velocity = rb.velocity;
        if(transform.position.x < left || transform.position.x > right){
            velocity.x *= -1;
        }
        if(transform.position.z > front || transform.position.z < back){
            velocity.z *= -1;
        }
        rb.velocity = velocity;
    }

    public void applyForce(Vector3 forceSource, float forceRadius){
        Vector3 dir = calculateMagnitude(forceSource, forceRadius);
        rb.AddForce(dir);
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

    IEnumerator decay(){
        yield return new WaitForSeconds(8);
        rb.useGravity = true;
        rb.drag = 30;
        yield return new WaitForSeconds(4);
        parentPool.updateExcessFeed(content);
        Destroy(gameObject);
    }

    public void setPoolMngr(PoolManager pool){
        this.parentPool = pool;
    }

}
