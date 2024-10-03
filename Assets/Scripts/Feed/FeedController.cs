using UnityEngine;
using ABMU.Core;
using System.Collections.Generic;
public class FeedController : AbstractController
{
    [SerializeField] private GameObject feedObject;
    private SceneMngrState sceneMngrState;
    public override void Init()
    {
        base.Init();
        sceneMngrState = GameObject.Find("SceneManager").GetComponent<SceneMngrState>();

    }

    public void generateFeeds (int count, Vector3 pos){
        float top = sceneMngrState.getTop();
        for(int i =0; i < count; i++){
            GameObject a = Instantiate(feedObject);
            a.GetComponent<FeedAgent>().Init();
            a.transform.position = new Vector3(Random.Range(pos.x -1, pos.x + 1), top, Random.Range(pos.z -1, pos.z + 1));
        }
        sceneMngrState.createSplash(1000, pos);
    }
}
