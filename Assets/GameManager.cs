using System;
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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip  backgroundMusic;

    [SerializeField] private PlayerController _playerControllerPrefab;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private MoveLeft _background;

    [Header("UI")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _gameOverScreen;
    
    private PlayerController _playerController;

    public void StartGame(int difficulty)
    {
        _titleScreen.gameObject.SetActive(false);

        float gravityMultiplier = 0;
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

        _playerController = Instantiate(_playerControllerPrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0, 90,0)));
        _background.enabled = true;
        _spawnManager.StartGame((DifficultyTypes)difficulty);
        audioSource.clip = backgroundMusic;
        audioSource.Play();

        _playerController.OnDeath += GameOver;
        _playerController.OnAccelerate += Accelerate;
    }

    private void GameOver()
    {
        audioSource.Stop();
        _spawnManager.GameOver();
        _background.enabled = false;
        _gameOverScreen.SetActive(true);
    }

    private void Accelerate(bool isActive)
    {
        _spawnManager.accelerate = isActive;
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