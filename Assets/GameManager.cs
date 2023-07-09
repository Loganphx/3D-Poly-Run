using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DifficultyTypes
{
    None = 0,
    Easy = 1,
    Medium = 2,
    Hard = 3
}
public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private MoveLeft _background;

    [Header("Prefabs")]
    [SerializeField] private PlayerController _playerControllerPrefab;
    [SerializeField] private AudioClip  backgroundMusic;

    [Header("UI")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _boostVisual;
    
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChanged(score);
        }
    }

    public void StartGame(int difficulty)
    {
        _titleScreen.gameObject.SetActive(false);

        float gravityMultiplier;
        switch ((DifficultyTypes)difficulty)
        {
            case DifficultyTypes.Easy:
                gravityMultiplier = 3f;
                break;
            case DifficultyTypes.Medium:
                gravityMultiplier = 3.25f;
                break;
            case DifficultyTypes.Hard:
                gravityMultiplier = 3.5f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
        }
        
        Physics.gravity = new Vector3(0,-9.81f,0) * gravityMultiplier;

        _background = transform.Find("Background").GetComponent<MoveLeft>();
        _background.enabled = true;

        _spawnManager = transform.Find("Spawn Manager").GetComponent<SpawnManager>();
        _spawnManager.StartGame((DifficultyTypes)difficulty, OnObstacleDestroyed);

        audioSource = transform.Find("Main Camera").GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.Play();

        var canvas = GameObject.Find("Canvas").transform;
        _titleScreen = canvas.Find("TitleScreen").gameObject;
        _scoreText = canvas.Find("Text_Score").GetComponent<TMP_Text>();
        _gameOverScreen = canvas.Find("GameOverScreen").gameObject;

        _playerController = Instantiate(_playerControllerPrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0, 90,0)));
        _playerController.Init(GameOver, Accelerate);
    }

    private void GameOver(PlayerController playerController)
    {
        audioSource.Stop();
        _spawnManager.GameOver();
        _background.enabled = false;
        _gameOverScreen.SetActive(true);
    }

    private void Accelerate(bool isActive)
    {
        _spawnManager.accelerate = isActive;
        _boostVisual.SetActive(isActive);
    }

    private void OnObstacleDestroyed(Obstacle obstacle)
    {
        Score += _spawnManager.accelerate ? 2 : 1 ;
    }
    
    private void OnScoreChanged(int value)
    {
        _scoreText.text = $"Score: {value}";
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}