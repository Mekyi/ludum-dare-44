using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] float gameLength = 600f;
    [SerializeField] Slider timeSlider;

    public float gameTime = 0;

    public bool isGameOver = false; // Either win or lose

    // Start is called before the first frame update
    void Start()
    {
        timeSlider.maxValue = gameLength;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
            timeSlider.value = gameTime;
            CheckForCompletion();
        }
    }

    private void CheckForCompletion()
    {
        if (gameTime >= gameLength)
        {
            isGameOver = true;
        }
    }
}
