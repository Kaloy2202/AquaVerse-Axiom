using UnityEngine;

public class SceneMngrState : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float avoidFactor;
    [SerializeField] private float matchingFactor;
    [SerializeField] private float centeringFactor;
    [SerializeField] private float biasFactor;
    [SerializeField] private float minSpeed, maxSpeed;
    [SerializeField] private float rotateFactor;
    [SerializeField] private float width, length, height;
    [SerializeField] private Vector3 center;
    [SerializeField] private GameObject foodObject;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask fishLayer;
    [SerializeField] private int status;
    [SerializeField] private float secondsPerDay;
    private float timeInHours;

    private Vector3 gizPos;
    private FishController fishController;
    private float rad;
    void Start()
    {
        //0 for feed
        //1 for fish 
        status = 1;
        fishController = GameObject.Find("FishController").GetComponent<FishController>();
    }

    public float getAvoidFactor()
    {
        return avoidFactor;
    }

    public float getMatchingFactor()
    {
        return matchingFactor;
    }

    public float getCenteringFactor()
    {
        return centeringFactor;
    }

    public float getMinSpeed()
    {
        return minSpeed;
    }

    public float getMaxSpeed()
    {
        return maxSpeed;
    }

    public float getRotateFactor()
    {
        return rotateFactor;
    }

    public float getTop()
    {
        return center.y + height / 2;
    }
    public float getBot()
    {
        return center.y - height / 2;
    }
    public float getLeft()
    {
        return center.x - length / 2;
    }
    public float getRight()
    {
        return center.x + length / 2;
    }
    public float getFront()
    {
        return center.z + width / 2;
    }
    public float getBack()
    {
        return center.z - width / 2;
    }
    public LayerMask getFoodLayer()
    {
        return this.foodLayer;
    }
    public float getBiasFactor()
    {
        return biasFactor;
    }

    public void setStatus(int value)
    {
        this.status = value;
    }
    public int getStatus()
    {
        return status;
    }
    //creates splash in the water
    //requires mass, the mass of object that caused the splash
    //position, center of the position where the splash will occur
    public void createSplash(float mass, Vector3 pos)
    {
        gizPos = pos;
        //calculate the radius of the splash
        float radius = calculateSplashRadius(mass);
        rad = radius;
        //get the collider given the radius
        Collider[] feeds = Physics.OverlapSphere(pos, radius, foodLayer);
        //reference to the FeedAgent script of the feed
        FeedAgent movement;
        foreach (Collider coll in feeds)
        {
            movement = coll.gameObject.GetComponent<FeedAgent>();
            if (movement)
            {
                //calculate the force of the splash based on the radius and the distance from the center
                movement.applyForce(pos, radius);
            }
        }
    }
    //calculates the radius of the splash
    private static float calculateSplashRadius(float mass)
    {
        //no splash if mass is below or equal zero
        if (mass <= 0)
        {
            return 0;
        }

        // Calculate the splash radius using the formula R = k * m^(1/3)
        float radius = Mathf.Pow(mass / 800, 1f / 3f);
        return radius;
    }

    void OnDrawGizmos()
    {
        // Set the color of the Gizmos
        Gizmos.color = Color.red;

        // Draw a wireframe sphere to visualize the OverlapSphere
        Gizmos.DrawWireSphere(gizPos, rad);
    }
    //temporary
    //place in the pool in the future
    public Vector3 getPoolDimension()
    {
        return new Vector3(length, height, width);
    }
    public void setHour(float hour){
        this.timeInHours = hour;
    }

    public float getTimeInHours(){
        return this.timeInHours;
    }

    public float getSecondsPerHour(){
        return (float) secondsPerDay/24;
    }
}
