using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AddComponentMenu("DOTS Samples/SpawnFromMonoBehaviour/Spawner")]
public class EntitySpawner : MonoBehaviour
{    
    public void SpawnEntityFromPrefab(GameObject prefabToSpawn, Vector3 spawnPosition, Quaternion rotation)
    {

        // Create entity prefab from the game object hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabToSpawn, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var instance = entityManager.Instantiate(prefab);
        var position = spawnPosition;
        entityManager.SetComponentData(instance, new Translation { Value = position });
    }
}
