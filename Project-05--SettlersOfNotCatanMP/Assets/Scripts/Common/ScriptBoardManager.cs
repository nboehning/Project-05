using UnityEngine;
using System.Collections;

public class ScriptBoardManager : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in roads)
        {
            road.GetComponent<ScriptBoardEdge>().FindAdjacentRoads();
        }
        Destroy(this);
	}
}
