using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void StartGame()
    {
        _titleScreen.gameObject.SetActive(false);
        _playerController = Instantiate(_playerControllerPrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0, 90,0)));
        _background.enabled = true;
        _spawnManager.StartGame();
        audioSource.clip = backgroundMusic;
        audioSource.Play();

        _playerController.OnDeath += GameOver;
    }

    private void GameOver()
    {
        audioSource.Stop();
        _spawnManager.GameOver();
        _background.enabled = false;
        _gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Prototype 3", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}