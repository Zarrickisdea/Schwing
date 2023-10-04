using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject walls;
    [SerializeField] private GameObject swingCube;
    [SerializeField] private GameObject startPlane;
    [SerializeField] private GameObject spawn;
    [SerializeField] private int levelLength;
    [SerializeField] private float cubeZSeperation;
    [SerializeField] private float initialYHeight;
    [SerializeField] private Button button;

    private void Awake()
    {
        CreateWalls();
        button.onClick.AddListener(CreateLevel);
    }

    private void CreateLevel()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Swing");
        if (cubes.Length > 0)
        {
            DestroyCubes(cubes);
        }

        for (int i = 0; i < levelLength; i++)
        {
            float randomX = Random.value;
            float randomY = Random.value;
            float positionOffset = Mathf.PerlinNoise((i + 1) * randomX, (i + 1) * randomY) * cubeZSeperation;

            Vector3 cubeSpawnPosition = new Vector3(0, initialYHeight + positionOffset, (i + 1) * cubeZSeperation);

            GameObject cube = Instantiate(swingCube, cubeSpawnPosition, Quaternion.identity);

            if (i == levelLength / 2)
            {
                Instantiate(spawn, cubeSpawnPosition, Quaternion.Euler(0, 90, 0));
            }
        }
    }

    private static void DestroyCubes(GameObject[] cubes)
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
