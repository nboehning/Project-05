using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//@author Nathan
public enum HexType
{
    WOOD,
    GRAIN,
    BRICK,
    WOOL,
    NONE
}

/// <summary>
/// @author Marshall Mason
/// </summary>
public class ScriptBoardHex : MonoBehaviour
{

    //Edge angle notes
    //Z rotation

    //-56.03992 /
    //56.03992  \
    //0   	  |
    public const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f; //Mathf.Sqrt(.75f)

    public GameObject cornerPrefab;
    public GameObject edgePrefab;
    public float hexSideLength;
    public int hexDieValue;
    public HexType resource;


    void Start()
    {
        CheckAndGenerateCorners();
        CheckAndGenerateEdges();
        ScriptSaveLoad saveScript = GameObject.Find("GameEngine").GetComponent<ScriptSaveLoad>();
        saveScript.hexes.Add(this);
    }

    public void CheckAndGenerateEdges()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 6; i++)
        {
            float edgeDistance = hexSideLength * MAGIC_EDGE_DISTANCE_CONVERT;
            //@ref Nathan
            float angle = Mathf.PI * (60f * i) / 180;
            Vector3 cornerPos = new Vector3((center.x + edgeDistance * Mathf.Cos(angle)),
                                    (center.y + edgeDistance * Mathf.Sin(angle)), (center.z - .5f));
            //@endRef Nathan

            Collider[] hitColliders = Physics.OverlapSphere(cornerPos, .15f);
            bool edgeFound = false;
            foreach (Collider other in hitColliders)
            {
                if (other.tag == "Road")
                {
                    edgeFound = true;
                    
                }
            }
            if (!edgeFound)
            {
                Instantiate(edgePrefab, cornerPos, Quaternion.identity);
            }
        }
    }

    public void CheckAndGenerateCorners()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 6; i++)
        {
            //@ref Nathan
            float angle = Mathf.PI * (60f * i + 30) / 180;
            Vector3 cornerPos = new Vector3((center.x + hexSideLength * Mathf.Cos(angle)),
                                    (center.y + hexSideLength * Mathf.Sin(angle)), (center.z - 1));
            //@endRef Nathan


            Collider[] hitColliders = Physics.OverlapSphere(cornerPos, .15f);
            bool settlementFound = false;
            foreach (Collider other in hitColliders)
            {
                if (other.tag == "Settlement")
                {
                    settlementFound = true;
                    if (!other.GetComponent<ScriptBoardCorner>().adjacentHexes.Contains(this))
                    {
                        other.gameObject.GetComponent<ScriptBoardCorner>().adjacentHexes.Add(this);
                    }
                }
            }
            if (!settlementFound)
            {
                Quaternion rotationQ = Quaternion.identity;
                Vector3 rotationV;
                switch (i % 3)
                {
                    case 1:
                        rotationV = new Vector3(0, 0, -56.03992f);
                        rotationQ = Quaternion.Euler(rotationV);
                        break;
                    case 2:
                        rotationV = new Vector3(0, 0, 56.03992f);
                        rotationQ = Quaternion.Euler(rotationV);
                        break;
                }
                GameObject temp = (GameObject)Instantiate(cornerPrefab, cornerPos, rotationQ);
                temp.gameObject.GetComponent<ScriptBoardCorner>().adjacentHexes.Add(this);
            }
        }
    }

}
