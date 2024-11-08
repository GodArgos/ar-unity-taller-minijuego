using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private GameObject beginCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private AudioClip gameClip;
    private AudioSource audioSource;

    public enum GameState
    {
        START,
        GAME,
        END
    }
    private GameState state;

    private void Start()
    {
        beginCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        endCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (state == GameState.START)
        {
            CheckForPlayer();
        }

        if (state == GameState.GAME && !audioSource.isPlaying)
        {
            audioSource.clip = gameClip;
            audioSource.Play();
        }
        else if (state != GameState.GAME)
        {
            audioSource.Stop();
        }
    }

    private void CheckForPlayer()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            beginCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            state = GameState.GAME;
        }
    }

    public void MaxPointsReached()
    {
        gameCanvas.SetActive(false);
        endCanvas.SetActive(true);
    }
}
