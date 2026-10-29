using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public string pendingLevel { get; private set; }
    public bool awaitingNextLevel => !string.IsNullOrEmpty(pendingLevel);

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        lives = 3;
        coins = 0;
        string[] current = SceneManager.GetActiveScene().name.Split('-');
        if (current.Length == 2 && int.TryParse(current[0], out int currentWorld) &&
            int.TryParse(current[1], out int currentStage)) {
            world = currentWorld;
            stage = currentStage;
        } else {
            NewGame();
        }
    }

    public void NewGame()
    {
        lives = 3;
        coins = 0;

        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        // TODO: show game over screen

        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        string sceneName = $"{world}-{stage}";
        if (!Application.CanStreamedLevelBeLoaded(sceneName)) {
            pendingLevel = sceneName;
            return;
        }

        CancelInvoke();
        pendingLevel = null;
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene(sceneName);
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }

}
