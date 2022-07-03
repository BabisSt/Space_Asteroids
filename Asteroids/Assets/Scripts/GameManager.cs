using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public int lives = 3;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int score = 0;
    private AudioSource audio;
    public AudioSource audio2;

    public CameraShake cameraShake;

    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject startUI;
    public static bool isPaused = false;
    public Text playerScoreText;
    public Text playerHighscoreText;
    public Text playerLivesText;

    



    private void Start()
    {
        audio = GetComponent<AudioSource>();
        score = 0;
        playerHighscoreText.text = PlayerPrefs.GetInt("HighScoreAsteroid", 0).ToString();
        playerLivesText.text = lives.ToString();
        startUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        playerScoreText.gameObject.SetActive(false);
        playerHighscoreText.gameObject.SetActive(false);
        playerLivesText.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void Update()
    {
       
        if (!startUI.activeSelf)
        {
            playerScoreText.gameObject.SetActive(true);
            playerHighscoreText.gameObject.SetActive(true);
            playerLivesText.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();
        audio2.Play();
        if (asteroid.size < 0.75f)
        {
            this.score += 100;
        }
        else if (asteroid.size < 1.2f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }

        playerScoreText.text = score.ToString();

        if (score > PlayerPrefs.GetInt("HighScoreAsteroid", 0))
        {
            PlayerPrefs.SetInt("HighScoreAsteroid", score);
            playerHighscoreText.text = score.ToString();
        }
        cameraShake.shakeDuration += 0.05f;
    }

    public void PlayerDied()
    {
        audio.Play();
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;
        playerLivesText.text = lives.ToString();
        if (this.lives <= 0)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }


    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        this.lives = 3;
        this.score = 0;
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        Time.timeScale = 1f;
        isPaused = false;
        gameOverUI.SetActive(false);
        this.lives = 3;
        playerLivesText.text = lives.ToString();
        StartRound();
        score = 0;
        Invoke(nameof(Respawn), 0);
        var clones = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
        // Scene scene = SceneManager.GetActiveScene(); 
        // SceneManager.LoadScene(scene.name);
    }

    public void StartRound()
    {
        this.player.transform.position = new Vector3(0, 85, 0);
    }

    private void SetPlayerScore(int score)
    {
        playerScoreText.text = score.ToString();
    }

    public void Resume()
    {
        startUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        // DontDestroyOnLoad(this);
    }

    public void Restart()
    {
        startUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        NewGame();
        GameOver();
        StartRound();
        // DontDestroyOnLoad(this);
    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        //DontDestroyOnLoad(this);
    }
}
