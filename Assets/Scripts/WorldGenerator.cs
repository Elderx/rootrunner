using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] boosts;

     float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f) 
        {
            elapsed = elapsed % 1f;
            SpawnObject(obstacles);
            SpawnObject(boosts);
        }
    }

    void SpawnObject(GameObject[] spawnObjects)
    {
        // Generate obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            Vector2 position = new Vector2(Random.Range(-10, 10), mainCamera.position.y - Random.Range(10, 30));
            GameObject newObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], position, Quaternion.identity);

            //Not great but mauybe it works
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Water"))
            {
                if (obj.GetComponent<Collider2D>().bounds.Intersects(newObject.GetComponent<Collider2D>().bounds) && obj != newObject)
                {
                    Destroy(obj);
                }
            }

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                if (obj.GetComponent<Collider2D>().bounds.Intersects(newObject.GetComponent<Collider2D>().bounds) && obj != newObject)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
