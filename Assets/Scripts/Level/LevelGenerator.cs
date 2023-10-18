using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour, IObserver
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
    [SerializeField] private float cubeMovementTotalDuration;
    [SerializeField] private float initialCubeMovementXOffset;

    #endregion

    #region Private
    private Queue<GameObject> cubes = new Queue<GameObject>();
    private Queue<GameObject> activeCubes = new Queue<GameObject>();
    private GameObject lastCubePlaced;
    private BoxCollider spawnPoint;
    #endregion

    private void Awake()
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
    }

    private void SetCubePositions(GameObject cube, float xPosition, float yPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, yPosition, zPosition);
        StartCoroutine(CubeMovement(cube, newPosition, cubeMovementTotalDuration));
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

        float initialXDuration = duration * 0.25f;
        float zDuration = duration * 0.25f;
        float yDuration = duration * 0.25f;
        float finalXDuration = duration * 0.25f;

        float tempX = newPosition.x + (newPosition.x > 0 ? initialCubeMovementXOffset : -initialCubeMovementXOffset);
        while (elapsedTime < initialXDuration)
        {
            float t = elapsedTime / initialXDuration;
            float newX = Mathf.Lerp(startPosition.x, tempX, t);
            cube.transform.position = new Vector3(newX, startPosition.y, startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < initialXDuration + zDuration)
        {
            float t = (elapsedTime - initialXDuration) / zDuration;
            float newZ = Mathf.Lerp(startPosition.z, newPosition.z, t);
            cube.transform.position = new Vector3(cube.transform.position.x, startPosition.y, newZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < initialXDuration + zDuration + yDuration)
        {
            float t = (elapsedTime - initialXDuration - zDuration) / yDuration;
            float newY = Mathf.Lerp(startPosition.y, newPosition.y, t);
            cube.transform.position = new Vector3(cube.transform.position.x, newY, cube.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < initialXDuration + zDuration + yDuration + finalXDuration)
        {
            float t = (elapsedTime - initialXDuration - zDuration - yDuration) / finalXDuration;
            float newX = Mathf.Lerp(tempX, newPosition.x, t);
            cube.transform.position = new Vector3(newX, newPosition.y, newPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = newPosition;
        SwingCube swingCube = cube.GetComponent<SwingCube>();
        if (swingCube != null)
        {
            swingCube.ChangeState(swingCube.StartState);
        }
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

    public void OnNotify()
    {
        MoveTrigger();
        ChangeHalfLevel();
    }
}
