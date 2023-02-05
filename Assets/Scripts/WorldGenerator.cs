using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Light2D worldLight;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] boosts;
    [SerializeField] private GameObject background;

    [Tooltip("Spawn interval in seconds")]
    [SerializeField] private float interval = 1f;

    [Tooltip("Obstacle spawn rate")]
    [SerializeField] private int spawnRate = 1;
    [SerializeField] int spawnAreaMinY = -9;
    [SerializeField] int spawnAreaMaxY = 10;

    float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= interval)
        {
            elapsed = elapsed % interval;

            for(int i = 0; i < spawnRate; i++)
            {
                SpawnObject(obstacles);
            }

            SpawnObject(boosts);
        }

        CleanupObjects();
    }

    private void FixedUpdate()
    {
        if(worldLight.intensity >= 0.2f) 
        {
            worldLight.intensity = 1 - (-mainCamera.position.y / 200);
        }

        if((0 - mainCamera.position.y) % 10 >= 0 && (0 - mainCamera.position.y) % 10 <= 0.2f)
        {
            SpawnBackground(mainCamera.position.y - 10);
        }
    }

    void SpawnObject(GameObject[] spawnObjects)
    {
        Vector2 position = new Vector2(Random.Range(spawnAreaMinY, spawnAreaMaxY), mainCamera.position.y - Random.Range(10, 30));
        GameObject newObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], position, Quaternion.identity);

        //Not great but maybe it works
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Water"))
        {
            if (obj.GetComponent<Collider2D>().bounds.Intersects(newObject.GetComponent<Collider2D>().bounds) && obj != newObject)
            {
                Destroy(newObject);
            }
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (obj.GetComponent<Collider2D>().bounds.Intersects(newObject.GetComponent<Collider2D>().bounds) && obj != newObject)
            {
                Destroy(newObject);
            }
        }
    }

    void SpawnBackground(float yPos)
    {
        Vector2 position = new Vector2(0, yPos);
        GameObject newObject = Instantiate(background, position, Quaternion.identity);
    }

    void CleanupObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Water"))
        {
            if (obj.transform.position.y > mainCamera.position.y + 20)
            {
                Destroy(obj);
            }
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (obj.transform.position.y > mainCamera.position.y + 20)
            {
                Destroy(obj);
            }
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Background"))
        {
            if (obj.transform.position.y > mainCamera.position.y + 20)
            {
                Destroy(obj);
            }
        }
    }
}
