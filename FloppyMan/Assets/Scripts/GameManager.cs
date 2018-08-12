using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource backgroundMusic;
    public List<AudioClip> musicPlaylist;

    public GameObject fileSpawner;
    public GameObject filePrefab;
    public GameObject[] targetBounds;

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
        LoadMainMenuScene();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)
        //    && (!SceneManager.GetActiveScene().name.Equals("MainMenuScene")
        //            && !SceneManager.GetActiveScene().name.Equals("CreditsScene")
        //            && !SceneManager.GetActiveScene().name.Equals("GameScene")))
        //{
        //    PauseGame();
        //}
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
                GUIManager.Instance.BSODImage = GameObject.Find("BSOD").GetComponent<Image>();
                GUIManager.Instance.BSODImage.enabled = false;
                StartCoroutine(EnableBSODImage());
                break;
            //case "LoadingScene":
            //    print("LoadingScene loaded");
            //    backgroundMusic.clip = null;
            //    backgroundMusic.Stop();
            //    break;
            //case "Tutorial":
            //    backgroundMusic.clip = musicPlaylist[2];
            //    break;
            case "GameScene":
                fileSpawner = GameObject.Find("FileSpawner");
                filePrefab = GameObject.Find("FilePrefab");
                targetBounds = new GameObject[2];
                targetBounds[0] = GameObject.Find("UpperBoundTarget");
                targetBounds[1] = GameObject.Find("LowerBoundTarget");
                backgroundMusic.clip = musicPlaylist[1];
                InvokeRepeating("InstantiateFile", 0.0f, 1.0f);
                break;
            //case "MiniGameScene":
            //    break;
            //case "CreditsScene":
            //    videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
            //    videoPlayer.loopPointReached += EndReached;
            //    backgroundMusic.clip = musicPlaylist[3];
            //    break;
            //case "Forest":
            //    backgroundMusic.clip = musicPlaylist[0];
            //    break;
            default:
                break;
        }
        backgroundMusic.Play();
    }

    //private void PauseGame()
    //{
    //    Time.timeScale = 0.0f;
    //    GUIManager.Instance.DisplayInGameMenu();
    //}

    //private void UnpauseGame()
    //{
    //    Time.timeScale = 1.0f;
    //    GUIManager.Instance.DisplayInGameMenu();
    //}

    //public void EndReached(UnityEngine.Video.VideoPlayer vp)
    //{
    //    LoadMainMenuScene();
    //}

    private void InstantiateFile()
    {
        Instantiate(filePrefab, fileSpawner.transform.position,
                     fileSpawner.transform.rotation);
    }

    IEnumerator EnableBSODImage()
    {
        yield return new WaitForSeconds(7);

        backgroundMusic.Stop();
        GUIManager.Instance.BSODImage.enabled = true;
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        yield return new WaitForSeconds(12);

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
        //if (Time.timeScale == 0.0f)
        //    UnpauseGame();
        SceneManager.LoadScene("BSODScene");
        StartCoroutine(LoadSceneAsync("GameScene"));
    }

    /// <summary>
    /// Loads the Game Scene.
    /// </summary>
    public void LoadGameScene()
    {
        print("Executing LoadGameScene");
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadSceneAsync("GameScene"));
    }

    /// <summary>
    /// Quits the game. Checks if the game is playing in editor or game mode
    /// in order to properly quit the game.
    /// </summary>
    public void QuitGame()
    {
        print("Executing QuitGame");

#if UNITY_EDITOR
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE
        if (Application.isPlaying)
            Application.Quit();
#endif
    }
    #endregion
}
