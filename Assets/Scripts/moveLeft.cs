using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeft : MonoBehaviour
{
    private float baseSpeed = 10; // Velocidad base
    private float speed; // Velocidad ajustada según el nivel
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript; // Referencia al GameManager
    private float leftBound = -15;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>(); // Obtener referencia al GameManager
        UpdateSpeed(); // Ajustar la velocidad según el nivel al inicio
    }

    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            // Ajustar la velocidad según el nivel
            UpdateSpeed();

            // Mover el objeto
            if (playerControllerScript.doubleSpeed)
            {
                transform.Translate(Vector3.left * Time.deltaTime * (speed * 2));
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }

        // Destruir el objeto si sale de los límites
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    void UpdateSpeed()
    {
        // Ajustar la velocidad según el nivel
        speed = baseSpeed + (gameManagerScript.level * 2); // Aumentar la velocidad en 2 unidades por nivel
    }
}