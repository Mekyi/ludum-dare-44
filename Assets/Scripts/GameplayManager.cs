using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] float gameLength = 600f;
    [SerializeField] RectTransform timeSlider;

    public float gameTime = 0;

    public bool isGameOver = false; // Either win or lose

    // Start is called before the first frame update
    void Start()
    {
        timeSlider.localScale = new Vector3(gameTime /gameLength, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
            timeSlider.localScale = new Vector3(gameTime / gameLength, 1, 1);
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
