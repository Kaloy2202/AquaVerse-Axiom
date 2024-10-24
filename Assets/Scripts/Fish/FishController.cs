using UnityEngine;
using ABMU.Core;

public class FishController : AbstractController
{
    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private int numberOfAgents = 100;

    private SceneMngrState sceneMngrState;

    public override void Init(){
        base.Init();
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();
    }

    public void spawnFish(PoolManager poolManager){
        Vector3 pos = poolManager.getCenter();
        Vector3 dimension = poolManager.getDimensions();

        Debug.Log("spawning");
        float top = poolManager.getCenter().y + poolManager.getDimensions().y/2;
        Vector3 loc, dir;
        BoidMovement mov;
        Growth growth;
        for(int i = 0; i < numberOfAgents; i++){
            Debug.Log("location vector is: " +pos.ToString());
            loc = new Vector3(Random.Range((float)(pos.x - .5), (float)(pos.x + .5)), top, Random.Range((float)(pos.z - .5), (float)(pos.z + .5)));
            dir = loc - pos;
            GameObject a = Instantiate(agentPrefab);
            mov = a.GetComponent<BoidMovement>();
            growth = a.GetComponent<Growth>();
            a.transform.position = loc;
            mov.setBounds(pos, dimension);
            mov.setAgentVelocity(dir.normalized);
            growth.setPoolMngrRef(poolManager);
            a.GetComponent<BoidMovement>().Init();
            a.GetComponent<Growth>().Init();
        }
        

    }

    internal void spawnFish(Vector3 pos)
    {
        throw new System.NotImplementedException();
    }
}
