using UnityEngine;
using System.Collections;

public enum HexType
{
    WOOD,
    GRAIN,
    BRICK,
    WOOL,
    NONE
}


public class ScriptHex
{
    public HexType hexType;
    public HexType prevHexType = HexType.NONE;
    public Vector2 hexCenter;
    public Vector2[] hexCorners = new Vector2[6];
    public bool isActive = false;
    public int hexNum;
    public int oldHexNum = 0;

    public ScriptHex(Vector2 center, float size)
    {
        hexType = HexType.NONE;
        hexNum = 0;
        hexCenter = center;
        for (int i = 0; i < 6; i++)
        {
            hexCorners[i] = GenerateHexPoint(hexCenter, size, i);
        }
    }

    public ScriptHex()
    {
        hexType = HexType.NONE;
        hexNum = 0;
    }

    public Vector2 GenerateHexPoint(Vector2 center, float size, int numCorner)
    {
        Vector2 returnPoint;

        float angleDegree = 60f * numCorner + 30f;
        var angleRadian = Mathf.PI/180*angleDegree;
        returnPoint.x = center.x + size * Mathf.Cos(angleRadian);
        returnPoint.y = center.y + size * Mathf.Sin(angleRadian);

        return returnPoint;

    }
}
