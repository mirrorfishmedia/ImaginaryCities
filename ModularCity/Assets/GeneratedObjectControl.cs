using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratedObjectControl : MonoBehaviour
{

    public static GeneratedObjectControl instance;
    public List<GameObject> generatedObjects = new List<GameObject>();

    public PerlinGenerator perlinGenerator;
    public GridSpawner gridSpawner;
    public EntityGridSpawner entitySpawner;

    public bool useEntityGridSpawner;
    public bool spawnAroundCamera;
    public Transform viewPointReference;
    public Vector3 cameraForwardOffset = new Vector3(0,0,50f);
    public float spawnRadius = 50f;
    public float spawnRate = .1f;
    public float nextSpawnTime;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void AddObject(GameObject objectToAdd)
    {
        generatedObjects.Add(objectToAdd);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ClearAllObjects();
            Generate();            
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;
            GenerateAroundCamera();
        }
       
    }

    void GenerateAroundCamera()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 spawnPoint = viewPointReference.position;
            spawnPoint.y = 0f;

            entitySpawner.Generate(spawnPoint);
        }
    }

    void Generate()
    {
        perlinGenerator.Generate();


        
        if (useEntityGridSpawner)
        {
            entitySpawner.Generate(Vector3.zero);
        }
        else
        {
            gridSpawner.Generate();
        }
    }

    void ClearAllObjects()
    {
        for (int i = generatedObjects.Count - 1; i >= 0; i--)
        {
            generatedObjects[i].SetActive(false);
            generatedObjects.RemoveAt(i);
        }
    }
}
