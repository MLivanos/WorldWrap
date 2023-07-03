using System.Linq;
using TMPro;
using UnityEngine;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text frameRateDisplay;
    [SerializeField] private TMP_Text ballNumberDisplay;
    [SerializeField] private TMP_Text treeNumberDisplay;
    [SerializeField] private int frameRateInterval;
    private float[] frameRates;
    private int frameCounter;
    private int numberOfBallsInScene;
    private int numberOfTreesInScene;

    private void Start()
    {
        frameCounter = 0;
        frameRates = new float[frameRateInterval];
    }

    private void Update()
    {
        TrackFrameRate();
    }

    private void TrackFrameRate()
    {
        if (frameCounter == frameRateInterval)
        {
            frameRateDisplay.text = ("FPS: " + Queryable.Average(frameRates.AsQueryable()));
            frameCounter = 0;
        }
        frameRates[frameCounter] = 1.0f / Time.deltaTime;
        frameCounter ++;
    }

    public void AddBalls(int newNumberOfBalls)
    {
        numberOfBallsInScene += newNumberOfBalls;
        ballNumberDisplay.text = ("Number of balls: " + numberOfBallsInScene);
    }

    public void AddTrees(int newNumberOfTrees)
    {
        numberOfTreesInScene += newNumberOfTrees;
        treeNumberDisplay.text = ("Number of child GameObjects: " + numberOfTreesInScene);
    }
}