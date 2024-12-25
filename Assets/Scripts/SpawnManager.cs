using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array de obstáculos
    public Vector3 spawnPos = new Vector3(25, 0, 0); // Posición de spawn
    private float startDelay = 2; // Tiempo inicial antes de empezar a spawnear
    private float repeatRate = 5; // Frecuencia de spawn inicial
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript; // Referencia al GameManager
    private int randomObstacle;

    void Start()
    {
        // Obtener referencias a los scripts
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Iniciar el spawn de obstáculos
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            // Seleccionar un obstáculo aleatorio
            randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomObstacle], spawnPos, obstaclePrefabs[randomObstacle].transform.rotation);

            // Ajustar la frecuencia de spawn basado en el nivel
            AdjustSpawnRate();
        }
    }

    void AdjustSpawnRate()
    {
        // Obtener el nivel actual del GameManager
        int currentLevel = gameManagerScript.level;

        // Ajustar el repeatRate basado en el nivel
        repeatRate = Mathf.Max(0.5f, 2f - (currentLevel * 0.1f)); // Disminuye el repeatRate cada nivel

        // Reiniciar el InvokeRepeating con la nueva frecuencia
        CancelInvoke("SpawnObstacle");
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }
}