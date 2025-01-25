using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] goalPoints;
    private int lastPoint = 0;
    public int score;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        goalPoints[lastPoint].SetActive(true);
    }

    public void SetNewGoalPoint()
    {
        int newGoalPointIndex = (int)(Random.value * goalPoints.Length);
        while(newGoalPointIndex == lastPoint)
        {
            newGoalPointIndex = (int)(Random.value * goalPoints.Length);
        }

        goalPoints[lastPoint].SetActive(false);
        score++;
        lastPoint = newGoalPointIndex;
        goalPoints[lastPoint].SetActive(true);
    }

}
