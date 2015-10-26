using UnityEngine;
using System.Collections;

public class ScriptBoardManager : MonoBehaviour {

    // Update is called once per frame
    /*
	void Update ()
    {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in roads)
        {
            road.GetComponent<ScriptBoardEdge>().FindAdjacentRoads();
        }
        Destroy(this);
	}
    */
    //Added by Victor Haskins
    //run once on activation, compounds upon the creation of arrays for later use
    //ScriptBoardEdge already takes care of all adjacent road and settlement

    public GameObject[] roads;
    public GameObject[] settlements;

    void Awake()
    {
        //populate arrays to use when updating other players' screens
        //with RPC functions from ScriptPlayer
        roads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in roads)
        {
            road.GetComponent<ScriptBoardEdge>().FindAdjacentRoads();
        }
        settlements = GameObject.FindGameObjectsWithTag("Settlement");
    }
}
