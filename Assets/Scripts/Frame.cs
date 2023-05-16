public class Frame
{
    public int TotalFrameScore => previousFrameScore + TotalThrowScore + TotalExtraScore;
    public bool IsStrike => throwScores[0] == 10;
    public bool IsLastFrameStrike2 => IsStrike && throwScores[1] == 10;
    public bool IsSpare => throwScores[0] + throwScores[1] == 10;
    public bool IsLastGameFrame => frameNumber == 10;

    public bool IsFrameOver => IsLastGameFrame ? IsLastFrameOver : IsNormalFrameOver;

    public int frameNumber;

    public readonly int[] throwScores = new int[3];
    private readonly int[] extraScores = new int[3];

    private int TotalThrowScore => throwScores[0] + throwScores[1] + throwScores[2];
    private int TotalExtraScore => extraScores[0] + extraScores[1] + extraScores[2];
    private bool IsNormalFrameOver => IsStrike || throwNumber == 2;
    private bool IsLastFrameOver
    {
        get
        {
            if (throwNumber == 2 && !IsStrike && !IsSpare)
                return true;

            return throwNumber == 3;
        }
    }

    public int throwNumber;
    private int extraScoresAdded;
    private int previousFrameScore;

    public Frame(int frameNumber, int previousFrameScore)
    {
        this.frameNumber = frameNumber;
        this.previousFrameScore = previousFrameScore;
    }

    public void SetThrowScore(int value, int extraScoreFromPreviousFrame)
    {
        previousFrameScore += extraScoreFromPreviousFrame;
        throwScores[throwNumber] = value;
        throwNumber++;

        if (!IsLastGameFrame) return;

        if (throwNumber >= 2 && IsStrike)
            extraScores[throwNumber - 2] = value;

        if (throwNumber == 3)
        {
            if (IsSpare)
                extraScores[0] = value;
            if (IsLastFrameStrike2)
                extraScores[2] = value;
        }
    }

    public bool AddExtraScore(int value, int extraScoreFromPreviousFrame)
    {
        if (extraScoresAdded == 2) return false;

        previousFrameScore += extraScoreFromPreviousFrame;

        if (IsSpare && extraScoresAdded == 0)
            extraScores[0] = value;

        if (IsStrike)
            extraScores[extraScoresAdded] = value;

        extraScoresAdded++;

        return true;
    }
}