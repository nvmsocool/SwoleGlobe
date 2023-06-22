using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoSpawner : MonoBehaviour
{
    public GameObject geo;
    public int Levels = 3;
    public float Bounds = 10;

    // Start is called before the first frame update
    void Start()
    {
        Place(Vector3.one * -Bounds, Vector3.one * Bounds, Levels);
    }

    private void Place(Vector3 min, Vector3 max, int level)
    {
        if (level == 0)
        {
            Instantiate(geo, new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z)), Random.rotation, this.transform);
        }
        else
        {
            int newLevel = level - 1;
            Vector3 newDelta = (max - min) / 2;
            Vector3 newBase = Vector3.zero;
            for (int i = 0; i < 2; i++)
            {
                newBase.x = i * newDelta.x;
                for (int j = 0; j < 2; j++)
                {
                    newBase.y = j * newDelta.y;
                    for (int k = 0; k < 2; k++)
                    {
                        newBase.z = k * newDelta.z;
                        Place(min + newBase, min + newBase + newDelta, newLevel);
                    }
                }
            }
        }
    }
}
