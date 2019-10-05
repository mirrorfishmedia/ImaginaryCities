using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawnerNoiseInput : MonoBehaviour
{

    public int gridX = 4;
    public int gridZ = 4;
    public GameObject prefabToSpawn;
    //public Vector3 gridOrigin = Vector3.zero;
    public float gridOffset = 2f;
    private PerlinGenerator perlinGenerator;

    void Awake()
    {
        perlinGenerator = GetComponent<PerlinGenerator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {

                GameObject clone = Instantiate(prefabToSpawn, transform.position + new Vector3(gridOffset * x, 0, gridOffset * z), transform.rotation);
                GeneratedObjectControl.instance.AddObject(clone);
                clone.GetComponent<BuildingGeneratorNoiseInput>().Build();

                clone.transform.SetParent(this.transform);
            }
        }

    }
}
