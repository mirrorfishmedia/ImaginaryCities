using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinGenerator : MonoBehaviour
{
    public int perlinTextureSizeX;
    public int perlinTextureSizeY;
    public bool randomizeNoiseOffset;

    public Vector2 perlinOffset;
    public float noiseScale = 1f;
    public Renderer visualizationRenderer;
    public static PerlinGenerator instance = null;
    private Texture2D perlinTexture;

    public int perlinGridSizeX;
    public int perlinGridSizeY;
    public float[,] perlinGrid;

    bool visualizeGrid = false;
    public GameObject visualizationCube;
    public float visualizationHeightScale = 5f;

    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetNoiseScaleFromSlider(Slider slider)
    {
        noiseScale = slider.value;
    }

    public void Generate()
    {
        GenerateNoise();
        GeneratePerlinGrid();
        if (visualizeGrid)
        {
            VisualizeGrid();
        }
    }

    public void GeneratePerlinGrid()
    {
        perlinGrid = new float[perlinGridSizeX, perlinGridSizeY];

        int gridStepSizeX = perlinTextureSizeX / perlinGridSizeX;
        int gridStepSizeY = perlinTextureSizeY / perlinGridSizeY;

        for (int x = 0; x < perlinGridSizeX; x++)
        {
            for (int y = 0; y < perlinGridSizeY; y++)
            {
                float sampledFloat = perlinTexture.GetPixel
                    ((Mathf.FloorToInt(x * gridStepSizeX)), (Mathf.FloorToInt(y * gridStepSizeX))).grayscale;

                perlinGrid[x, y] = sampledFloat;
            }
        }
    }

    public float PerlinGridValueFromWorldposition(Vector3 worldPosition)
    {

        int xToSample = Mathf.FloorToInt(worldPosition.x + perlinGridSizeX * .5f);
        int yToSample = Mathf.FloorToInt(worldPosition.z + perlinGridSizeY * .5f);

        xToSample = xToSample % perlinGridSizeX;
        yToSample = yToSample % perlinGridSizeY;

        float sampledValue = perlinGrid[xToSample, yToSample];

        return sampledValue;
    }

    void VisualizeGrid()
    {
        GameObject visualizationParent = new GameObject("VisualizationParent");
        visualizationParent.transform.SetParent(this.transform);

        for (int x = 0; x < perlinGridSizeX; x++)
        {
            for (int y = 0; y < perlinGridSizeY; y++)
            {
                GameObject clone = Instantiate(visualizationCube, new Vector3(x, perlinGrid[x,y] * visualizationHeightScale, y) + transform.position, transform.rotation);
                clone.transform.SetParent(visualizationParent.transform);
            }
        }
        visualizationParent.transform.position = new Vector3(-perlinGridSizeX * .5f, -visualizationHeightScale * .5f, -perlinGridSizeY * .5f);

    }

    public float SampleMasterTexture(float x, float y)
    {
        int pixelX = Mathf.FloorToInt(perlinTexture.width * x);
        int pixelY = Mathf.FloorToInt(perlinTexture.height * y);

        float sampledValue = perlinTexture.GetPixel(pixelX, pixelY).grayscale;

        return sampledValue;
    }

    void GenerateNoise()
    {

        if (randomizeNoiseOffset)
        {
            perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
        }

        perlinTexture = new Texture2D(perlinTextureSizeX, perlinTextureSizeY);

        for (int x = 0; x < perlinTextureSizeX; x++)
        {
            for (int y = 0; y < perlinTextureSizeY; y++)
            {
                perlinTexture.SetPixel(x, y, SampleNoise(x, y));
            }
        }

        perlinTexture.Apply();
        if (visualizationRenderer != null)
        {
            visualizationRenderer.material.mainTexture = perlinTexture;
        }
    }

    Color SampleNoise(int x, int y)
    {

        float xCoord = (float)x / perlinTextureSizeX * noiseScale + perlinOffset.x;
        float yCoord = (float)y / perlinTextureSizeY * noiseScale + perlinOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color perlinColor = new Color(sample,sample,sample);

        return perlinColor;
    }

}
