[System.Serializable]
public class ScoreData
{
    public int score;
    public string date;

    public ScoreData(int score)
    {
        this.score = score;
        this.date = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}
