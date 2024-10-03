using UnityEngine;
using ABMU.Core;

public class FishController : AbstractController
{
    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private int numberOfAgents = 100;

    public override void Init(){
        base.Init();

        for (int i = 0; i < numberOfAgents; i++)
        {
            GameObject a = Instantiate(agentPrefab);
            a.GetComponent<BoidMovement>().Init();
            a.GetComponent<Growth>().Init();
        }
    }

}
