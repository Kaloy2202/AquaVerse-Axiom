using UnityEngine;
using ABMU.Core;
using System.Collections;
public class FeedController : AbstractController
{
    [SerializeField] private GameObject feedObject;
    private SceneMngrState sceneMngrState;
    public override void Init()
    {
        base.Init();
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();

    }

    public void generateFeeds (int count, Vector3 pos, PoolManager poolManager){
        StartCoroutine(generateFeedAndCreateSplash(count,pos, poolManager));
    }

    IEnumerator generateFeedAndCreateSplash(int count, Vector3 pos, PoolManager poolManager){
        Vector3 bounds = poolManager.getDimensions();
        Vector3 center = poolManager.getCenter();
        float top = center.y + bounds.y/2;
        FeedAgent agent;
        for(int i =0; i < count; i++){
            GameObject a = Instantiate(feedObject);
            agent = a.GetComponent<FeedAgent>();
            agent.setBounds(center, bounds);
            agent.setPoolMngr(poolManager);
            a.transform.position = new Vector3(Random.Range(pos.x -1, pos.x + 1), top, Random.Range(pos.z -1, pos.z + 1));
            agent.Init();
        }
        yield return new WaitForEndOfFrame();
        sceneMngrState.createSplash(1000, pos);
    }
}
