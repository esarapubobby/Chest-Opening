using System.Collections.Generic;
using UnityEngine;

public class SeaCreaturesSpawner : MonoBehaviour
{
   

    [Header("Prefabs")]
    public GameObject[] creaturePrefabs;
    private List<GameObject> creaturesPool = new List<GameObject>();
    public int poolSize = 10;

    [Header("Spawn Settings")]
    public Transform player;
    public float spawnDistanceAhead = 20f;
    public float minLateralOffset = -4f;
    public float maxLateralOffset = 4f;

    [Header("Spawn Timing")]
    public float initialInterval = 4f;
    public float minInterval = 1.5f;
    public float intervalDecreaseRate = 0.02f;

    private float currentInterval;
    private float timer;
    private float gameTime;

    private void Start()
    {
        currentInterval = initialInterval;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(creaturePrefabs[Random.Range(0, creaturePrefabs.Length)]);
            obstacle.SetActive(false);
            creaturesPool.Add(obstacle);
        }
    }
    GameObject GetACreature()
    {
        List<GameObject> available = new List<GameObject>();
        foreach (GameObject obstacle in creaturesPool)
        {
            if (!obstacle.activeInHierarchy)
            {
                available.Add(obstacle);
            }
        }
        if (available.Count == 0)
        {
            return null;
        }
        return available[Random.Range(0, available.Count)];
    }


    private void Update()
    {
        if (player == null)
            return;

        gameTime += Time.deltaTime;
        currentInterval = Mathf.Max(minInterval, initialInterval - gameTime * intervalDecreaseRate);

        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            SpawnCreatures();
            timer = 0f;
        }
        foreach (GameObject obstacle in creaturesPool)
        {
            if (obstacle.activeInHierarchy)
            {
                if (player.position.z > obstacle.transform.position.z + 10f)
                {
                    obstacle.SetActive(false);
                }
            }
        }
    }

    private void SpawnCreatures()
    {
        if (creaturePrefabs == null || creaturePrefabs.Length == 0)
            return;


        float lateralOffset = Random.Range(minLateralOffset, maxLateralOffset);

        Vector3 spawnPos =
            player.position + player.forward * spawnDistanceAhead + player.right * lateralOffset;

        GameObject creature = GetACreature();
        if (creature == null)
        {
            return;
        }
        creature.transform.position = spawnPos;
        creature.transform.rotation = Quaternion.identity;
        float randomSizeValue = Random.Range(1f, 2f);
        if (creature.CompareTag("MantaRay"))
        {
            randomSizeValue = Random.Range(0.1f, 0.4f);
        }
        creature.transform.localScale = new Vector3(randomSizeValue,randomSizeValue,randomSizeValue);
        creature.SetActive(true);
    }
}

