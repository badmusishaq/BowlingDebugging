using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform PinSetSpawnPosition;
    [SerializeField]
    private GameObject PinSetPrefab;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private float throwTimeout = 5.5f;
    [SerializeField]
    private FrameScoreUI[] frameScoreUis; // from oldest frame to current
    [SerializeField]
    private FrameScoreUI lastGameFrame;


    private List<Frame> frames;
    private int currentThrowScore;
    private int currentFrame;
    private Frame LastFrame => frames[frames.Count - 1];

    private Pin[] currentPins = new Pin[0];
    private BowlingBall ball;

    private bool throwStarted;
    private float remainingTimeout;

    public void BallKnockedDown()
    {
        ball = null;
    }

    public void PinKnockedDown()
    {
        currentThrowScore++;
    }

    public void BallThrown(BowlingBall bowlingBall)
    {
        ball = bowlingBall;
    }

    private void Update()
    {
        if (!throwStarted || !playerController.wasBallThrown) return;

        remainingTimeout -= Time.deltaTime;

        if (remainingTimeout <= 0 || CheckIfPiecesAreStatic())
            FinishThrow();
    }

    private void FinishThrow()
    {
        throwStarted = false;

        foreach (var pin in currentPins)
        {
            if (pin != null && pin.DidPinFall)
            {
                currentThrowScore++;
                Destroy(pin.gameObject);
            }
        }

        int newExtraPoints = 0;
        for (int i = 0; i < 2; i++)
        {
            var frameIndex = currentFrame - 3 + i;
            if (frameIndex < 0) continue;

            var frame = frames[frameIndex];
            if (frame.AddExtraScore(currentThrowScore, newExtraPoints))
                newExtraPoints += currentThrowScore;
        }

        LastFrame.SetThrowScore(currentThrowScore, newExtraPoints);
        UpdateScoreUI();

        if (!LastFrame.IsFrameOver)
            Invoke(nameof(SetupThrow), 1);

        else if (!LastFrame.IsLastGameFrame)
            Invoke(nameof(SetupFrame), 1);

        else
            Invoke(nameof(FinishGame), 10);
    }


    private void FinishGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private bool CheckIfPiecesAreStatic()
    {
        foreach (var pin in currentPins)
        {
            if (pin != null && pin.DidPinMove())
                return false;
        }

        var ballStatus = ball == null || !ball.DidBallMove();
        return ballStatus;
    }

    private void SetupThrow()
    {
        currentThrowScore = 0;

        if (LastFrame.IsStrike || LastFrame.IsLastFrameStrike2)
            SpawnPins();
        else
        {
            foreach (var pin in currentPins)
            {
                if (pin != null)
                    pin.ResetPosition();
            }
        }

        if (ball != null)
            Destroy(ball.gameObject);

        playerController.StartAiming();
        throwStarted = true;
        remainingTimeout = throwTimeout;
    }

    private void UpdateScoreUI()
    {
        for (int i = 0; i < 3; i++)
        {
            var frameIndex = currentFrame - 3 + i;
            var frameUI = frameScoreUis[i];

            if (frameIndex < 0)
            {
                frameUI.gameObject.SetActive(false);
                continue;
            }

            if (currentFrame == 10 && i == 2)
            {
                frameUI.gameObject.SetActive(false);
                frameUI = lastGameFrame;
            }

            frameUI.SetScoreUI(frames[frameIndex]);
            frameUI.gameObject.SetActive(true);
        }
    }

    private void SpawnPins()
    {
        Instantiate(PinSetPrefab, PinSetSpawnPosition.position, PinSetSpawnPosition.rotation);
        currentPins = FindObjectsOfType<Pin>();
    }

    private void SetupFrame()
    {
        DisposeLastFrame();

        currentFrame++;
        var totalScore = frames.Count > 0 ? LastFrame.TotalFrameScore : 0;
        frames.Add(new Frame(currentFrame, totalScore));

        UpdateScoreUI();
        SpawnPins();
        SetupThrow();
    }

    private void DisposeLastFrame()
    {
        foreach (var pin in currentPins)
        {
            if (pin != null) Destroy(pin.gameObject);
        }
    }

    private void Start()
    {
        frames = new List<Frame>();
        Invoke(nameof(SetupFrame), 1);
    }
}