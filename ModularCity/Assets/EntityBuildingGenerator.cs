using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class EntityBuildingGenerator : MonoBehaviour
{
    public int maxPieces = 20;
    public float perlinScaleFactor = 2f;

    public int randomVariationMin = -5;
    public int randomVariationMax = 10;

    public void Build(EntityGridSpawner entityGridSpawner, Vector3 position)
    {
        float sampledValue = PerlinGenerator.instance.PerlinSteppedPosition(transform.position);

        int targetPieces = Mathf.FloorToInt(maxPieces * (sampledValue));
        targetPieces += Random.Range(randomVariationMin, randomVariationMax);

        if (targetPieces <= 0)
        {
            return;
        }

        float heightOffset = 0;
        heightOffset += SpawnPieceLayer(entityGridSpawner, entityGridSpawner.baseEntities, position, heightOffset);

        for (int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(entityGridSpawner, entityGridSpawner.middleEntities, position, heightOffset);
        }

        SpawnPieceLayer(entityGridSpawner, entityGridSpawner.topEntities, position, heightOffset);
    }

    float SpawnPieceLayer(EntityGridSpawner entityGridSpawner, EntityGridSpawner.EntityWithBounds[] entityArray, Vector3 spawnPosition, float inputHeight)
    {
        EntityGridSpawner.EntityWithBounds entityWithBounds = entityArray[Random.Range(0, entityArray.Length)];

        float heightOffset = entityWithBounds.boxHeight;

        //GameObject clone = Instantiate(selectedEntity.gameObject, this.transform.position + new Vector3(0, inputHeight, 0), transform.rotation) as GameObject;
        var instance = entityGridSpawner.entityManager.Instantiate(entityWithBounds.entity);
        var position = spawnPosition + new Vector3(0, inputHeight, 0);
        entityGridSpawner.entityManager.SetComponentData(instance, new Translation { Value = position });

        return heightOffset;
    }
}
