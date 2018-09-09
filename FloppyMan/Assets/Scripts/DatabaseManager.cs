using System.Collections;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private string secretKey;
    public string addScoreURL;
    public string highscoreURL;

    void Start()
    {
        secretKey = "Nam-Yppolf";
        addScoreURL = "http://www.josejuansandoval.com/programming/projects/floppy-man/addscore.php?";
        highscoreURL = "http://www.josejuansandoval.com/programming/projects/floppy-man/getscores.php";
        DontDestroyOnLoad(this);
    }

    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScore(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
                
        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
                
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            GameManager.Instance.SaveHighScores(hs_get.text);
        }
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public void SendScore(string name, int score)
    {
        StartCoroutine(PostScore(name, score));
    }

    public void RetrieveScores()
    {
        StartCoroutine(GetScores());
    }
}
