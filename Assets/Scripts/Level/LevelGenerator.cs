using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour, IObserver
{
    #region Inspector Variables

    [Header("Level Generation")]
    [SerializeField] private GameObject walls;
    [SerializeField] private SwingCube swingCube;
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
    private Queue<SwingCube> cubes = new Queue<SwingCube>();
    private Queue<SwingCube> activeCubes = new Queue<SwingCube>();
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
            SwingCube cube = Instantiate(swingCube, new Vector3(0, 0, 0), Quaternion.identity);
            cube.gameObject.SetActive(false);
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

            SwingCube cube = GetCube();
            cube.gameObject.SetActive(true);
            SetCubePositions(cube, xPosition, yPosition, zPosition);
            lastCubePlaced = cube.gameObject;
            activeCubes.Enqueue(cube);

            if (i == numberOfCubes / 2)
            {
                GameObject spawnObject = Instantiate(spawn, new Vector3(0, yPosition + 1, zPosition), Quaternion.Euler(0, 90, 0));
                spawnPoint = spawnObject.GetComponent<BoxCollider>();
            }
        }
    }

    private void SetCubePositions(SwingCube cube, float xPosition, float yPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, yPosition, zPosition);
        cube.ChangeState(new SwingCubeChangeState(cube, cubeMovementTotalDuration, newPosition));
    }

    private SwingCube GetCube()
    {
        if (cubes.Count > 0)
        {
            SwingCube cube = cubes.Dequeue();
            return cube;
        }
        return null;
    }

    private SwingCube GetActiveCube()
    {
        if (activeCubes.Count > 0)
        {
            SwingCube cube = activeCubes.Dequeue();
            return cube;
        }
        return null;
    }

    private void CreateWalls()
    {
        //GameObject leftWall = Instantiate(walls, new Vector3(-10, 0, 0), Quaternion.identity);
        //GameObject rightWall = Instantiate(walls, new Vector3(10, 0, 0), Quaternion.identity);
        GameObject start = Instantiate(startPlane, new Vector3(0, -2f, 0), Quaternion.identity);
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

            SwingCube cube = GetActiveCube();
            currentEndPoint = newPosition;
            lastCubePlaced = cube.gameObject;
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
