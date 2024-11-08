using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float timeCycle = 2f;
    [SerializeField] private AudioClip clip;
    public TextMeshProUGUI points;
    private Transform playerTrans;
    private bool start = false;
    private float timer;
    public float currentObjects = 0;
    private float minDistance = 0.3f;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    private int currentPoints = 0;

    private void Start()
    {
        timer = timeCycle;
    }

    private void Update()
    {
        if (!start)
        {
            CheckForPlayer();
        }
        else
        {
            if (timer <= 0)
            {
                if (currentObjects <= 10)
                {
                    SpawnObject();
                    currentObjects++;
                }
   
                timer = timeCycle;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void CheckForPlayer()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerTrans = playerController.transform;
            start = true;
        }
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition;

        // Try to find a valid spawn position
        for (int attempts = 0; attempts < 10; attempts++) // Limit attempts to avoid infinite loop
        {
            // Generate random X position between -0.5 and -0.1, or between 0.1 and 0.5
            float xPosition = Random.Range(-0.5f, -0.1f);
            if (Random.value > 0.5f)
            {
                xPosition = Random.Range(0.1f, 0.5f);
            }

            // Generate random Z position between -0.5 and -0.1, or between 0.1 and 0.5
            float zPosition = Random.Range(-0.5f, -0.1f);
            if (Random.value > 0.5f)
            {
                zPosition = Random.Range(0.1f, 0.5f);
            }

            spawnPosition = new Vector3(playerTrans.position.x + xPosition, playerTrans.position.y, playerTrans.position.z + zPosition);

            // Check if the position is valid
            if (IsPositionValid(spawnPosition))
            {
                // If valid, instantiate the object and add the position to the list
                Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                spawnedPositions.Add(spawnPosition);
                return; // Exit the method after successful spawn
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        // Check if the new position is too close to any existing positions
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false; // Position is too close to an existing object
            }
        }
        return true; // Position is valid
    }

    public void UpdateText()
    {
        currentPoints++;
        if (currentPoints < 9999)
        {
            points.text = (int.Parse(points.text) + 1).ToString("0000");
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        else
        {
            GameManager.Instance.MaxPointsReached();
        }
    }
}
