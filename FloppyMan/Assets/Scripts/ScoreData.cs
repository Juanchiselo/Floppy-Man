public class ScoreData
{
    public string Name { get; set; }
    public int Score { get; set; }
    public string Date { get; set; }

    public ScoreData(string name, int score, string date)
    {
        Name = name;
        Score = score;
        Date = date;
    }
}
