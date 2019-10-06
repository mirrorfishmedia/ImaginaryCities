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

    // Update is called once per frame
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

    }

    void Generate()
    {
        perlinGenerator.Generate();
        gridSpawner.Generate();
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
