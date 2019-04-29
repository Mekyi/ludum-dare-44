using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] float gameLength = 600f;
    [SerializeField] RectTransform timeSlider;
    [SerializeField] RectTransform shipIcon;

    // Alerts
    [SerializeField] float cryoCriticalAlertSensitivity = 8f;
    [SerializeField] GameObject cryoCriticalIndicator;
    [SerializeField] GameObject courseCriticalIndicator;
    [SerializeField] GameObject leftEngineCriticalIndicator;
    [SerializeField] GameObject rightEngineCriticalIndicator;
    [SerializeField] GameObject energyCriticalIndicator;

    [SerializeField] GameObject[] cryopods;
    [SerializeField] GameObject minimapCamera;
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject gameWonCanvas;


    public float gameTime = 0;
    

    // Game events
    public bool fixCourse = false;
    public bool fixLeftEngine = false;
    public bool fixRightEngine = false;
    public bool cryoCriticalState = false;

    public bool isCriticalEnergy = false;
    
    public bool isGameOver = false; // Either win or lose
    public bool isPauseToggled = false;
    private bool isMapToggled = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        timeSlider.localScale = new Vector3(gameTime /gameLength, 1, 1);
        StartCoroutine(CheckForEvents());
        cryoCriticalIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            Timer();
            ToggleMap();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
            CheckForCompletion();
        }
    }

    public void TogglePauseMenu()
    {
        isPauseToggled = !isPauseToggled;
        if (isPauseToggled == true)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void UnpauseGameWithButton()
    {
        Time.timeScale = 1;
        FindObjectOfType<GameplayManager>().isPauseToggled = false;
    }

    private void ToggleMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapToggled = !isMapToggled;
            if (isMapToggled == true)
            {
                minimapCamera.SetActive(true);
                uiCanvas.SetActive(false);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                minimapCamera.SetActive(false);
                uiCanvas.SetActive(true);
            }

        }
    }

    private void Timer()
    {
        if (!fixCourse && !fixLeftEngine && !fixRightEngine)
        {
            gameTime += Time.deltaTime;
        }
        else if (fixCourse)
        {
            gameTime -= Time.deltaTime;
        }
        else if (fixLeftEngine && fixRightEngine)
        {
            // Do nothing
        }
        else if (fixLeftEngine || fixRightEngine)
        {
            gameTime += Time.deltaTime * 0.5f;
        }
        gameTime = Mathf.Clamp(gameTime, 0f, gameLength);
        timeSlider.localScale = new Vector3 (gameTime / gameLength, 1, 1);
        // shipIcon.transform.position = new Vector2 (timeSlider.right.x, 0);
    }

    private void CheckForCompletion()
    {
        if (gameTime >= gameLength)
        {
            isGameOver = true;
            WinGame();
        }
    }

    IEnumerator CheckForEvents()
    {
        CheckForCompletion();
        CheckCryoStates();
        CheckCourseState();
        CheckLeftEngineState();
        CheckRightEngineState();
        CheckEnergyState();

        yield return new WaitForSeconds(1f);
        if (!isGameOver)
        {
            StartCoroutine(CheckForEvents());
        }
    }

    private void CheckEnergyState()
    {
        if (isCriticalEnergy)
        {
            energyCriticalIndicator.SetActive(true);
        }
        else
        {
            energyCriticalIndicator.SetActive(false);
        }
    }

    private void CheckLeftEngineState()
    {
        if (fixLeftEngine)
        {
            leftEngineCriticalIndicator.SetActive(true);
        }
        else
        {
            leftEngineCriticalIndicator.SetActive(false);
        }
    }

    private void CheckRightEngineState()
    {
        if (fixRightEngine)
        {
            rightEngineCriticalIndicator.SetActive(true);
        }
        else
        {
            rightEngineCriticalIndicator.SetActive(false);
        }

    }

    private void CheckCourseState()
    {
        if (fixCourse)
        {
            courseCriticalIndicator.SetActive(true);
        }
        else
        {
            courseCriticalIndicator.SetActive(false);
        }
    }

    private void CheckCryoStates()
    {
        // Check
        foreach (GameObject cryopod in cryopods)
        {
            if (cryopod.GetComponent<CryopodControls>().currentCharge 
                                    < cryoCriticalAlertSensitivity)
            {
                cryoCriticalState = true;
                break;
            }
            else
            {
                cryoCriticalState = false;
            }
        }

        // Handle
        if (cryoCriticalState)
        {
            cryoCriticalIndicator.SetActive(true);
        }
        else
        {
            cryoCriticalIndicator.SetActive(false);
        }
    }

    public void StartGameLostEvent()
    {
        StartCoroutine(LoseGame());
    }

    IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(4f);
        gameOverCanvas.SetActive(true);
    }

    public void WinGame()
    {
        gameWonCanvas.SetActive(true);
    }
}
