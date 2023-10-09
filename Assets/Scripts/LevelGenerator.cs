using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #endregion

    #region Private
    private Queue<GameObject> cubes = new Queue<GameObject>();
    private Queue<GameObject> activeCubes = new Queue<GameObject>();
    private GameObject lastCubePlaced;
    private BoxCollider spawnPoint;
    #endregion

    #region Public
    #endregion

    #region Singleton
    private static LevelGenerator _instance;
    public static LevelGenerator Instance { get { return _instance; } }
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        CreateWalls();
        CreateLevelObjectPool();
        CreateInitialLevel();
    }

    private void CreateLevelObjectPool()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject cube = Instantiate(swingCube, new Vector3(0, 0, 0), Quaternion.identity);
            cube.SetActive(false);
            cubes.Enqueue(cube);
        }
    }

    private void CreateInitialLevel()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            float xPosition = Random.value < 0.5 ? cubeXPosition : -cubeXPosition;
            float zPosition = (i + 1) * cubeZSeperation;

            float randomX = Random.value;
            float randomY = Random.value;

            float yPosition = initialYHeight + (Mathf.PerlinNoise(randomX * (i + 1), randomY * (i + 1)) * 10);

            GameObject cube = GetCube();
            cube.SetActive(true);
            SetCubePositions(cube, xPosition, yPosition, zPosition);
            lastCubePlaced = cube;
            activeCubes.Enqueue(cube);

            if (i == numberOfCubes / 2)
            {
                GameObject spawnObject = Instantiate(spawn, new Vector3(0, yPosition + 1, zPosition), Quaternion.Euler(0, 90, 0));
                spawnPoint = spawnObject.GetComponent<BoxCollider>();
            }
        }
        Debug.Log("Level Created");
        Debug.Log(cubes.Count);
        Debug.Log(activeCubes.Count);
    }

    private void SetCubePositions(GameObject cube, float xPosition, float yPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, yPosition, zPosition);
        StartCoroutine(CubeMovement(cube, newPosition, 4f));
    }

    private GameObject GetCube()
    {
        if (cubes.Count > 0)
        {
            GameObject cube = cubes.Dequeue();
            return cube;
        }
        return null;
    }

    private GameObject GetActiveCube()
    {
        if (activeCubes.Count > 0)
        {
            GameObject cube = activeCubes.Dequeue();
            return cube;
        }
        return null;
    }

    private void ReturnObjectToPool (GameObject cube)
    {
        cubes.Enqueue(cube);
        cube.SetActive(false);
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
        GameObject start = Instantiate(startPlane, new Vector3(0, -2f, 0), Quaternion.identity);
    }

    private IEnumerator CubeMovement(GameObject cube, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = cube.transform.position;
        float elapsedTime = 0f;

        float xDuration = duration * 0.4f;
        float zDuration = duration * 0.4f;
        float yDuration = duration * 0.2f;

        while (elapsedTime < xDuration)
        {
            float t = elapsedTime / xDuration;
            float newX = Mathf.Lerp(startPosition.x, newPosition.x, t);
            cube.transform.position = new Vector3(newX, startPosition.y, startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < xDuration + zDuration)
        {
            float t = (elapsedTime - xDuration) / zDuration;
            float newZ = Mathf.Lerp(startPosition.z, newPosition.z, t);
            cube.transform.position = new Vector3(cube.transform.position.x, startPosition.y, newZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < xDuration + zDuration + yDuration)
        {
            float t = (elapsedTime - xDuration - zDuration) / yDuration;
            float newY = Mathf.Lerp(startPosition.y, newPosition.y, t);
            cube.transform.position = new Vector3(cube.transform.position.x, newY, cube.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = newPosition;
    }

    public void ChangeHalfLevel()
    {
        int count = numberOfCubes / 2;

        Vector3 currentEndPoint = lastCubePlaced.transform.position;

        for (int i = 0; i < count; i++)
        {
            float xPosition = Random.value < 0.5 ? cubeXPosition : -cubeXPosition;
            float zPosition = (currentEndPoint.z + cubeZSeperation);
            
            float randomX = Random.value;
            float randomY = Random.value;

            float yPosition = initialYHeight + (Mathf.PerlinNoise(randomX * (i + 1), randomY * (i + 1)) * 10);

            Vector3 newPosition = new Vector3(xPosition, yPosition, zPosition);

            GameObject cube = GetActiveCube();
            currentEndPoint = newPosition;
            lastCubePlaced = cube;
            activeCubes.Enqueue(cube);
            SetCubePositions(cube, xPosition, yPosition, zPosition);
        }
    }

    public void MoveTrigger()
    {
        spawnPoint.enabled = false;
        spawnPoint.gameObject.transform.position = new Vector3(0, 0, lastCubePlaced.transform.position.z);
        spawnPoint.enabled = true;
    }
}
