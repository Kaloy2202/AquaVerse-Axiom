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

    public void spawnFish(Vector3 pos, Vector3 dimension){
        Debug.Log("spawning");
        float top = sceneMngrState.getTop();
        Vector3 loc, dir;
        BoidMovement mov;
        for(int i = 0; i < numberOfAgents; i++){
            loc = new Vector3(Random.Range((float)(pos.x - .5), (float)(pos.x + .5)), top, Random.Range((float)(pos.z - .5), (float)(pos.z + .5)));
            dir = loc - pos;
            GameObject a = Instantiate(agentPrefab);
            a.GetComponent<BoidMovement>().Init();
            a.GetComponent<Growth>().Init();

            a.transform.position = loc;
            mov = a.GetComponent<BoidMovement>();
            mov.setBounds(pos, dimension);
            mov.setAgentVelocity(dir.normalized);
        }
        

    }

}
