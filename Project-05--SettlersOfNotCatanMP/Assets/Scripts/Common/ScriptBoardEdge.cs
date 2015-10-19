using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptBoardEdge : MonoBehaviour {

    public ScriptEngine engine;
    public ScriptPlayer owner = null;
    public List<ScriptBoardEdge> adjacentRoads = new List<ScriptBoardEdge>(0);
    public List<ScriptBoardCorner> adjacentSettlements = new List<ScriptBoardCorner>(0);

    void Start()
    {
        engine = GameObject.Find("GameEngine").GetComponent<ScriptEngine>();
    }

    public bool CheckValidBuild()
    {
        foreach (ScriptBoardEdge road in adjacentRoads)
        {
            if (road.owner == engine.players[0])
            {
                owner = engine.players[0];
                return true;
            }
        }
        return false;
    }

    public bool CheckStartRoad()
    {
        foreach (ScriptBoardCorner settlement in adjacentSettlements)
        {
            if (settlement.owner == engine.players[0])
            {
                owner = engine.players[0];
                return true;
            }
        }
        return false;
    }

    public void FindAdjacentRoads()
    {
        float colliderRadius = transform.lossyScale.y * 2.5f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, colliderRadius);
        foreach (Collider other in hitColliders)
        {
            if (other.tag == "Road" && other.gameObject != this.gameObject)
            {
                adjacentRoads.Add(other.gameObject.GetComponent<ScriptBoardEdge>());
            }
        }
        colliderRadius = transform.lossyScale.y * 1.5f;
        Vector3 colliderLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z - .5f);
        hitColliders = Physics.OverlapSphere(transform.position, colliderRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Settlement")
            {
                hitColliders[i].GetComponent<ScriptBoardCorner>().adjacentRoads.Add(this);
            }
        }
    }
}
