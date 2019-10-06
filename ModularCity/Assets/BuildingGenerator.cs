using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 5;
    public int maxPieces = 20;
    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;


    // Start is called before the first frame update
    void Start()
    {
        Build();
    }
    
    void Build()
    {

        int targetPieces = Random.Range(minPieces, maxPieces);
        float heightOffset = 0;
        heightOffset += SpawnPieceLayer(baseParts, heightOffset);
        

        for (int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(middleParts, heightOffset);
        }

        SpawnPieceLayer(topParts, heightOffset);
    }

    float SpawnPieceLayer(GameObject[] pieceArray, float inputHeight)
    {
        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, this.transform.position 
            + new Vector3 (0, inputHeight, 0), transform.rotation) as GameObject;
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
        float heightOffset = bounds.size.y;

        clone.transform.SetParent(this.transform);

        return heightOffset;
    }

}
