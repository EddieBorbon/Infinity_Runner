using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float score;
    public int level = 1; // Nivel actual del jugador
    public int scoreToNextLevel = 10; // Puntaje necesario para subir de nivel (valor inicial más alto)
    public float scoreIncrementRate = 1f; // Puntos que se ganan por segundo (ajustable)

    private PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText; // Texto para mostrar el nivel
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Transform startingPoint;
    public GameObject gameOverPanel;
    public float lerpSpeed;
    private bool isIntro;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        score = 0;
        playerController.gameOver = true;
        StartCoroutine(PlayIntro());
        UpdateLevelText(); // Actualizar el texto del nivel al inicio
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        playerController.doubleJumpUsed = false;
        playerController.ResetGravity();
        playerController.GameStart();
    }

    IEnumerator PlayIntro()
    {
        isIntro = true;
        Vector3 startPos = playerController.transform.position;
        Vector3 endPos = startingPoint.position;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        playerController.GetComponent<Animator>().SetFloat("Speed_Multiplier", 0.5f);
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerController.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }
        playerController.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f);
        playerController.gameOver = false;
        isIntro = false;
    }

    void Update()
    {
        if (!playerController.gameOver)
        {
            // Incrementar el puntaje gradualmente
            if (playerController.doubleSpeed)
            {
                score += scoreIncrementRate * 2 * Time.deltaTime; // Aumenta más rápido en doble velocidad
            }
            else
            {
                score += scoreIncrementRate * Time.deltaTime; // Aumenta a ritmo normal
            }
            scoreText.text = "Score: " + (int)(score); // Mostrar el score sin decimales

            // Verificar si el jugador sube de nivel
            if (score >= scoreToNextLevel)
            {
                IncreaseLevel();
            }
        }

        if (playerController.gameOver && isIntro == false)
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
    }

    void IncreaseLevel()
    {
        level++; // Aumentar el nivel
        scoreToNextLevel = (int)(scoreToNextLevel * 1.8f); // Aumentar el puntaje necesario para el próximo nivel (factor más bajo)
        UpdateLevelText();
        //Debug.Log("Level Up! Current Level: " + level);
    }

    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }
}