using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    // GAME DATA
    public int AntivirusCount { get; set; }
    public float fileSeconds;
    public int infectedProbability;
    public float speed;
    public int multiplier;
    public int score;
    public int highScoresShownTime = 15;
    public string playerName;
    public List<ScoreData> highScores;

    public Camera mainCamera;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public List<AudioClip> musicPlaylist;    

    [Header("File Prefabs")]
    private GameObject fileSpawner;
    public List<GameObject> filePrefabs;
    public GameObject[] targets;
    public List<KeyValuePair<int, double>> elements;

    public DatabaseManager databaseManager;

    public List<Sprite> FloppySprites;

    public static GameManager Instance = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(backgroundMusic);
    }

    // Use this for initialization
    void Start()
    {
        ResetStats();

        highScores = new List<ScoreData>();

        // Sets the probabilities of appearance for the files.
        elements = new List<KeyValuePair<int, double>>();
        elements.Add(new KeyValuePair<int, double>(0, 0.05));
        elements.Add(new KeyValuePair<int, double>(1, 0.15));
        elements.Add(new KeyValuePair<int, double>(2, 0.15));
        elements.Add(new KeyValuePair<int, double>(3, 0.05));
        elements.Add(new KeyValuePair<int, double>(4, 0.15));
        elements.Add(new KeyValuePair<int, double>(5, 0.15));
        elements.Add(new KeyValuePair<int, double>(6, 0.15));
        elements.Add(new KeyValuePair<int, double>(7, 0.15));

        LoadMainMenuScene();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void ResetStats()
    {
        AntivirusCount = 0;
        fileSeconds = 3;
        infectedProbability = 15;
        speed = 5.0f;
        multiplier = 1;
        score = 0;
        filePrefabs = new List<GameObject>();
    }

    public void ResetGame()
    {
        ResetStats();
        CancelInvoke();
        StopAllCoroutines();
        SceneManager.LoadScene("GameScene");
        UnpauseGame();
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = GUIManager.Instance.playerNameInputField.text;
        GUIManager.Instance.floppyManText.text = @"C:\Users\" + playerName + "> cd Desktop\n\n"
            + @"C:\Users\" + playerName + @"\Desktop> Floppy-Man.exe";
        LoadGameScene();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "BSODScene":
                backgroundMusic.clip = musicPlaylist[0];
                GUIManager.Instance.TitleScreenPanel = GameObject.Find("TitleScreenPanel");
                GUIManager.Instance.floppyImage = GameObject.Find("FloppyImage").GetComponent<Image>();
                StartCoroutine(GUIManager.Instance.SwapFloppySprite(5));
                StartCoroutine(GUIManager.Instance.HideTitleScreenPanel());
                backgroundMusic.Play();
                break;
            case "MainMenuScene":
                //backgroundMusic.clip = musicPlaylist[1];
                //backgroundMusic.loop = true;
                GUIManager.Instance.PromptText = GameObject.Find("PromptText").GetComponent<Text>();
                GUIManager.Instance.PromptText.enabled = false;
                GUIManager.Instance.playerNameInputField = GameObject.Find("PlayerNameInputField").GetComponent<InputField>();
                GUIManager.Instance.playerNameInputField.onEndEdit.AddListener(SetPlayerName);
                GUIManager.Instance.floppyManText = GameObject.Find("FloppyManText").GetComponent<Text>();
                break;
            case "GameScene":
                mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                GUIManager.Instance.GameUI = GameObject.Find("GameUI");

                for (int i = 0; i < 10; i++)
                {
                    GUIManager.Instance.ranks[i] = GameObject.Find("Rank" + i).GetComponent<Text>();
                    GUIManager.Instance.names[i] = GameObject.Find("Name" + i).GetComponent<Text>();
                    GUIManager.Instance.scores[i] = GameObject.Find("Score" + i).GetComponent<Text>();

                    GUIManager.Instance.ranks[i].enabled = false;
                    GUIManager.Instance.names[i].enabled = false;
                    GUIManager.Instance.scores[i].enabled = false;
                }

                GUIManager.Instance.HighScoresPanel = GameObject.Find("HighScores");
                GUIManager.Instance.HighScoresPanel.SetActive(false);

                GUIManager.Instance.GameOverPanel = GameObject.Find("GameOverPanel");
                GUIManager.Instance.NewHighScoreDialogBox = GameObject.Find("NewHighScoreDialog");
                GUIManager.Instance.GameOverDialogBox = GameObject.Find("GameOverDialog");
                GUIManager.Instance.yesButton = GameObject.Find("YesButton").GetComponent<Button>();
                GUIManager.Instance.noButton = GameObject.Find("NoButton").GetComponent<Button>();
                GUIManager.Instance.yesButton.onClick.AddListener(GameManager.Instance.ResetGame);
                GUIManager.Instance.noButton.onClick.AddListener(GUIManager.Instance.DisplayHighScores);
                GUIManager.Instance.GameOverPanel.SetActive(false);


                GUIManager.Instance.Score = GameObject.Find("Score").GetComponent<Text>();
                GUIManager.Instance.multiplier = GameObject.Find("Multiplier").GetComponent<Text>();

                for (int i = 0; i < 3; i++)
                    GUIManager.Instance.AntivirusShields[i] = GameObject.Find("AntivirusIcon" + i).GetComponent<Image>();

                fileSpawner = GameObject.Find("FileSpawner");
                filePrefabs.Add(GameObject.Find("AntivirusFilePrefab"));
                filePrefabs.Add(GameObject.Find("AudioFilePrefab"));
                filePrefabs.Add(GameObject.Find("BatchFilePrefab"));
                filePrefabs.Add(GameObject.Find("DocumentFilePrefab"));
                filePrefabs.Add(GameObject.Find("EmailFilePrefab"));
                filePrefabs.Add(GameObject.Find("FilePrefab"));
                filePrefabs.Add(GameObject.Find("ImageFileAPrefab"));
                filePrefabs.Add(GameObject.Find("VideoFilePrefab"));

                

                //GameObject.Find("HUD").SetActive(false);


                targets = new GameObject[2];
                targets[0] = GameObject.Find("UpperBoundTarget");
                targets[1] = GameObject.Find("LowerBoundTarget");
                backgroundMusic.clip = musicPlaylist[1];
                StartCoroutine(InstantiateFile());
                InvokeRepeating("IncreaseSeconds", 0.0f, 5.0f);
                InvokeRepeating("IncreaseDifficulty", 0.0f, 10.0f);
                backgroundMusic.Play();

                databaseManager.RetrieveScores();
                break;            
            default:
                break;
        }
        
    }

    public void SaveHighScores(string highScoresResult)
    {
        // Gets the High Scores from the database.
        highScores = new List<ScoreData>();

        string[] players = highScoresResult.Split('¬');
        
        for(int i = 0; i < players.Length - 1; i++)
        {
            string[] data = players[i].Split('~');
            highScores.Add(new ScoreData(data[0], int.Parse(data[1]), data[2]));
        }
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        GUIManager.Instance.UpdateGUI();
    }
    

    private void IncreaseSeconds()
    {
        if(fileSeconds > 0.5f)
            fileSeconds -= 0.2f;
    }

    private void IncreaseDifficulty()
    {
        if(infectedProbability > 2)
            infectedProbability -= 1;

        speed += 1.0f;
    }

    private bool IsScoreAHighScore(int score)
    {
        bool isNewHighScore = false;

        if (highScores.Count < 10)
            isNewHighScore = true;
        else
        {
            for(int i = highScores.Count - 1; i >= 0; i--)
            {
                if (score > highScores[i].Score)
                {
                    isNewHighScore = true;
                    break;
                }
            }
        }
        
        

        return isNewHighScore;
    }

    private IEnumerator InvertGravity(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Quaternion rotation = mainCamera.GetComponent<Transform>().rotation;
        rotation.z = 180.0f;
        mainCamera.GetComponent<Transform>().rotation = rotation;
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }

    private IEnumerator InstantiateFile()
    {
        yield return new WaitForSeconds(fileSeconds);

        int prefabIndex = RandomNumberGenerator.RandomIntegerProbability(elements);
        
        GameObject prefab = Instantiate(filePrefabs[prefabIndex], fileSpawner.transform.position,
                     fileSpawner.transform.rotation);

        StartCoroutine(InstantiateFile());
    }

    IEnumerator LoadSceneAsync(string scene, int delay)
    {
        yield return new WaitForSeconds(delay);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            yield return null;
        }
    }

    #region SCENE_LOADING_HANDLERS

    /// <summary>
    /// Loads the Main Menu Scene.
    /// </summary>
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("BSODScene");
        StartCoroutine(LoadSceneAsync("MainMenuScene", 12));
    }

    /// <summary>
    /// Loads the Game Scene.
    /// </summary>
    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync("GameScene", 3));
    }

    public void GameOver()
    {
        StopAllCoroutines();
        CancelInvoke();
        databaseManager.SendScore(playerName, score);

        foreach (GameObject file in GameObject.FindGameObjectsWithTag("File"))
            Destroy(file);
        foreach (GameObject antivirus in GameObject.FindGameObjectsWithTag("Antivirus"))
            Destroy(antivirus);
        GUIManager.Instance.DisplayGameOver(IsScoreAHighScore(score));
        PauseGame();
    }

    /// <summary>
    /// Quits the game. Checks if the game is playing in editor or game mode
    /// in order to properly quit the game.
    /// </summary>
    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(highScoresShownTime);

        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
        #endif
    }
    #endregion
}
