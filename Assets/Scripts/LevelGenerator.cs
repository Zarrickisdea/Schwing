using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    #region Inspector Variables
    [Header("Level Generation")]
    [SerializeField] private GameObject walls;
    [SerializeField] private GameObject swingCube;
    [SerializeField] private GameObject startPlane;
    [SerializeField] private GameObject spawn;

    [Header("Level Settings")]
    [SerializeField] private int numberOfCubes;
    [SerializeField] private float cubeXPosition;
    [SerializeField] private float initialYHeight;
    [SerializeField] private float cubeZSeperation;
    [SerializeField] private float perlinNoiseScale;

    [Header("UI")]
    [SerializeField] private Button button;
    #endregion

    #region Private
    private List<GameObject> cubes = new List<GameObject>();
    private Queue<GameObject> spawnTriggers = new Queue<GameObject>();
    #endregion

    #region Public
    #endregion

    private void Awake()
    {
        CreateWalls();
        button.onClick.AddListener(CreateLevel);
    }

    private void CreateLevel()
    {
        if (cubes.Count > 0)
        {
            DestroyCubes(cubes);
        }

        if (spawnTriggers.Count > 0)
        {
            GameObject toDestroy = spawnTriggers.Dequeue();
            Destroy(toDestroy);
        }

        for (int i = 0; i < numberOfCubes; i++)
        {
            float randomX = Random.value;
            float randomY = Random.value;
            float positionOffset = Mathf.PerlinNoise((i + 1) * randomX, (i + 1) * randomY) * perlinNoiseScale;

            float side = Random.value < 0.5f ? -1 : 1;
            Vector3 cubeSpawnPosition = new Vector3(side * cubeXPosition, initialYHeight + positionOffset, (i + 1) * cubeZSeperation);

            GameObject cube = Instantiate(swingCube, cubeSpawnPosition, Quaternion.identity);
            cubes.Add(cube);

            if (i == numberOfCubes / 2)
            {
                GameObject spawnTrigger = Instantiate(spawn, cubeSpawnPosition, Quaternion.Euler(0, 90, 0));
                spawnTriggers.Enqueue(spawnTrigger);
            }
        }
    }

    private void DestroyCubes(List<GameObject> cubes)
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
    }

    private void CreateWalls()
    {
        GameObject leftWall = Instantiate(walls, new Vector3(-10, 0, 0), Quaternion.identity);
        GameObject rightWall = Instantiate(walls, new Vector3(10, 0, 0), Quaternion.identity);
        GameObject start = Instantiate(startPlane, new Vector3(-2.5f, -1, 0), Quaternion.identity);
    }
}
