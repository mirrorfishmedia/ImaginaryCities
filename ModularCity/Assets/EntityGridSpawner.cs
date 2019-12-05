using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class EntityGridSpawner : MonoBehaviour
{

    public int gridX = 4;
    public int gridZ = 4;
    public EntityBuildingGenerator entityBuilding;
    public Vector3 gridOrigin = Vector3.zero;
    public float gridOffset = 2f;
    public bool generateOnEnable;

    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;

    public EntityWithBounds[] baseEntities;
    public EntityWithBounds[] middleEntities;
    public EntityWithBounds[] topEntities;

    public EntityManager entityManager;
    public GameObjectConversionSettings gameObjectConversionSettings;





    public struct EntityWithBounds
    {
        public Entity entity;
        public float boxHeight;
    }

    void OnEnable()
    {
        gameObjectConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        baseEntities = GameObjectArrayToEntityArray(baseParts);
        middleEntities = GameObjectArrayToEntityArray(middleParts);
        topEntities = GameObjectArrayToEntityArray(topParts);

        if (generateOnEnable)
        {
            Generate(gridOrigin);
        }
    }

    EntityWithBounds[] GameObjectArrayToEntityArray(GameObject[] arrayToConvert)
    {
        EntityWithBounds[] entityArray = new EntityWithBounds[arrayToConvert.Length];

        for (int i = 0; i < arrayToConvert.Length; i++)
        {
            Entity entityFromGameObject = GameObjectConversionUtility.ConvertGameObjectHierarchy(arrayToConvert[i], gameObjectConversionSettings);

            Mesh cloneMesh = arrayToConvert[i].GetComponentInChildren<MeshFilter>().sharedMesh;
            Bounds baseBounds = cloneMesh.bounds;

            entityArray[i].entity = entityFromGameObject;
            entityArray[i].boxHeight = baseBounds.size.y;
        }

        return entityArray;
    }

    

    public void Generate(Vector3 spawnOrigin)
    {
        SpawnGrid(spawnOrigin);
    }


    void SpawnGrid(Vector3 spawnOrigin)
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                entityBuilding.Build(this, transform.position + spawnOrigin + new Vector3(gridOffset * x, 0, gridOffset * z));
            }
        }
    }

    
}
