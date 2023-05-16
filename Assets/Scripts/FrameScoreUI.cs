using UnityEngine;
using TMPro;

public class FrameScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text frameNumber;
    [SerializeField]
    private TMP_Text frameTotalScore;
    [SerializeField]
    private TMP_Text frame1stThrowScore;
    [SerializeField]
    private TMP_Text frame2ndThrowScore;
    [SerializeField]
    private TMP_Text frame3rdThrowScore;

    public void UpdateScoreUI(int throwNumber, int throwScore, int frameScore, int totalScore)
    {
        if (throwNumber == 0)
        {
            if (throwScore == 10)
                frame2ndThrowScore.text = "X";
            else
                frame1stThrowScore.text = throwScore.ToString();
        }
        else
            frame2ndThrowScore.text = frameScore == 10 ? "/" : throwScore.ToString();

        frameTotalScore.text = totalScore.ToString();
    }

    public void SetScoreUI(Frame frame)
    {
        frameNumber.text = frame.frameNumber.ToString();
        frameTotalScore.text = frame.TotalFrameScore.ToString();

        if (frame.IsStrike)
        {
            frame2ndThrowScore.text = "X";
            frame1stThrowScore.text = "";
        }
        else
        {
            frame1stThrowScore.text = frame.throwNumber < 1 ? "" : frame.throwScores[0].ToString();

            if (frame.IsSpare)
                frame2ndThrowScore.text = "/";
            else if (frame.IsLastFrameStrike2)
                frame2ndThrowScore.text = "X";
            else
                frame2ndThrowScore.text = frame.throwNumber < 2 ? "" : frame.throwScores[1].ToString();

            if (frame.throwNumber == 3)
                frame3rdThrowScore.text =  frame.throwScores[2].ToString();
        }
    }
}
