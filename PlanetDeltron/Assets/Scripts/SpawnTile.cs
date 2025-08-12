using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTile : MonoBehaviour
{
    public GameObject tile;
    public GameObject referenceObject;
    public GameObject slideTile;
    public GameObject jumpTile;
    private GameObject tileToSpawn;

    private PlayerController playerController;

    private float distanceBetweenTiles = 4.0f;
    private float randomValue = 0.8f;
    private float obstacleValue = 0.9f;
    private int rotation = 0;
    public int numberOfTiles;
    public int numberOfTilesAtOneTime = 25;

    private bool lastTileWasObstacle = false;
    private bool firstTile = true;

    private Vector3 previousTilePosition;
    private Vector3 direction, mainDirection = new Vector3(0, 0, 1), otherDirection = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Get last tile position
        previousTilePosition = referenceObject.transform.position;
        // Get playerController script
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if game is over
        if (!playerController.isGameOver)
        {
            // Get total number of tiles in the scene
            numberOfTiles = GameObject.FindGameObjectsWithTag("Tile").Length;

            // If tiles are less than max number of tiles allowed in scene, spawn tiles
            if (numberOfTiles < numberOfTilesAtOneTime)
            {

                // If random value is less than randomValue or is first tile, or if the last tile was an obstacle, set direction to forward direction
                if (Random.value < randomValue || firstTile || lastTileWasObstacle)
                {    
                    direction = mainDirection;
                    firstTile = false;
                }
                else
                {
                    // Else set direction to turn (make a corner)
                    Vector3 temp = direction;
                    direction = otherDirection;
                    mainDirection = direction;
                    otherDirection = temp;
                }

                // If obstacle odd > than obstacle value set tile to spawn to a random obstacle tile else spawn regular tile
                float obstacleOdd = Random.value;
                if (obstacleOdd >= obstacleValue && obstacleOdd < 1.0f - (1.0f - obstacleValue) / 2 && !lastTileWasObstacle)
                {
                    tileToSpawn = jumpTile;
                    lastTileWasObstacle = true;
                }
                else if (obstacleOdd >= 1.0f - (1.0f - obstacleValue) / 2 && !lastTileWasObstacle)
                {
                    lastTileWasObstacle = true;
                    tileToSpawn = slideTile;
                }
                else
                {
                    tileToSpawn = tile;
                    lastTileWasObstacle = false;
                }

                // Get direction of tile to spawn and set rotation (for obstacles)
                if (direction.x == 1)
                    rotation = 90;
                else
                    rotation = 0;

                // Spawn tiles
                Vector3 spawnPos = previousTilePosition + distanceBetweenTiles * direction;
                Instantiate(tileToSpawn, spawnPos, Quaternion.Euler(0, rotation, 0));
                previousTilePosition = spawnPos;
            }
        }
    }
}
