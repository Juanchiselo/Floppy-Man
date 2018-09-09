using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button yesButton;
    public Button noButton;

    [Header("Texts")]
    public Text Score;
    public Text multiplier;
    public Text floppyManText;
    public Text PromptText;
    public Text[] ranks;
    public Text[] names;
    public Text[] scores;
    public Text debug;

    [Header("Images")]
    public Image floppyImage;

    [Header("Input Fields")]
    public InputField playerNameInputField;

    [Header("Panels")]
    public GameObject TitleScreenPanel;
    public GameObject HighScoresPanel { get; set; }
    public GameObject GameOverPanel { get; set; }

    public Image BSODImage { get; set; }
    public Text FileCountText { get; set; }
    public Image[] AntivirusShields { get; set; }

    
    public GameObject GameOverDialogBox { get; set; }
    public GameObject NewHighScoreDialogBox { get; set; }
    public GameObject GameUI { get; set; }

    public static GUIManager Instance = null;

    private Color transparent;
    private Color opaque;
    private Image image;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        AntivirusShields = new Image[3];
        transparent = new Color(1.0f, 1.0f, 1.0f, 0.125f);
        opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        ranks = new Text[10];
        names = new Text[10];
        scores = new Text[10];

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGUI()
    {
        Score.text = GameManager.Instance.score.ToString();
        multiplier.text = "x" + GameManager.Instance.multiplier.ToString();
        UpdateAntivirusGUI();
    }

    public IEnumerator SwapFloppySprite(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        floppyImage.sprite = GameManager.Instance.FloppySprites[1];
    }

    public IEnumerator HideTitleScreenPanel()
    {
        yield return new WaitForSeconds(7);
        GameManager.Instance.backgroundMusic.Stop();
        TitleScreenPanel.SetActive(false);
    }

    public void DisplayGameOver(bool isNewHighScore)
    {
        GameManager.Instance.databaseManager.RetrieveScores();

        if (isNewHighScore)
            NewHighScoreDialogBox.SetActive(true);
        else
            NewHighScoreDialogBox.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    public void DisplayHighScores()
    {
        GameOverPanel.SetActive(false);
        GameUI.SetActive(false);
        HighScoresPanel.SetActive(true);
        GameManager.Instance.UnpauseGame();

        for(int i = 0; i < GameManager.Instance.highScores.Count; i++)
        {
            ranks[i].enabled = true;
            names[i].text = GameManager.Instance.highScores[i].Name;
            names[i].enabled = true;
            scores[i].text = GameManager.Instance.highScores[i].Score.ToString();
            scores[i].enabled = true;
        }

        StartCoroutine(GameManager.Instance.QuitGame());
    }

    private void UpdateAntivirusGUI()
    {
        for (int i = 0; i < 3; i++)
            AntivirusShields[i].GetComponent<Image>().color = transparent;
            
        switch (GameManager.Instance.AntivirusCount)
        {
            case 3:
                AntivirusShields[2].GetComponent<Image>().color = opaque;
                goto case 2;
            case 2:
                AntivirusShields[1].GetComponent<Image>().color = opaque;
                goto case 1;
            case 1:
                AntivirusShields[0].GetComponent<Image>().color = opaque;
                break;
            default:
                break;
        }
    }
}
