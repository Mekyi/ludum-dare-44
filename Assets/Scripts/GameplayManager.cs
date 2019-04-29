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
    [SerializeField] GameObject gameWonCanvas0;
    [SerializeField] GameObject gameWonCanvas1;
    [SerializeField] GameObject gameWonCanvas2;
    [SerializeField] GameObject gameWonCanvas3;
    [SerializeField] GameObject tutorialCanvas;


    public float gameTime = 0;
    AudioSource alertSound;
    

    // Game events
    public bool fixCourse = false;
    public bool fixLeftEngine = false;
    public bool fixRightEngine = false;
    public bool cryoCriticalState = false;

    public bool isCriticalEnergy = false;
    
    public bool isGameOver = false; // Either win or lose
    public bool isPauseToggled = false;
    private bool isMapToggled = false;
    public bool isTutorialToggled = true;
    private int cryoAliveCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        timeSlider.localScale = new Vector3(gameTime /gameLength, 1, 1);
        StartCoroutine(CheckForEvents());
        cryoCriticalIndicator.SetActive(false);
        tutorialCanvas.SetActive(true);
        alertSound = gameObject.GetComponent<AudioSource>();
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
            if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleTutorialMenu();
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
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
        }
    }

    public void ToggleTutorialMenu()
    {
        if (isTutorialToggled == true)
        {
            tutorialCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            tutorialCanvas.SetActive(false);
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
        
        if (isCriticalEnergy && !energyCriticalIndicator.activeSelf)
        {
            alertSound.Play();
            energyCriticalIndicator.SetActive(true);
        }
        else if (!isCriticalEnergy)
        {
            energyCriticalIndicator.SetActive(false);
        }
    }

    private void CheckLeftEngineState()
    {
        
        if (fixLeftEngine && !leftEngineCriticalIndicator.activeSelf)
        {
            alertSound.Play();
            leftEngineCriticalIndicator.SetActive(true);
        }
        else if (!fixLeftEngine)
        {
            leftEngineCriticalIndicator.SetActive(false);
        }
    }

    private void CheckRightEngineState()
    {
        if (fixRightEngine && !rightEngineCriticalIndicator.activeSelf)
        {
            alertSound.Play();
            rightEngineCriticalIndicator.SetActive(true);
        }
        else if (!fixRightEngine)
        {
            rightEngineCriticalIndicator.SetActive(false);
        }

    }

    private void CheckCourseState()
    {
        if (fixCourse && !courseCriticalIndicator.activeSelf)
        {
            alertSound.Play();
            courseCriticalIndicator.SetActive(true);
        }
        else if (!fixCourse)
        {
            courseCriticalIndicator.SetActive(false);
        }
    }

    private void CheckCryoStates()
    {
        // Check
        foreach (GameObject cryopod in cryopods)
        {
            if (cryopod.GetComponent<CryopodControls>()
                .currentCharge < cryoCriticalAlertSensitivity
                && cryopod.GetComponent<CryopodControls>().isAlive)
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
        if (cryoCriticalState && !cryoCriticalIndicator.activeSelf)
        {
            alertSound.Play();
            cryoCriticalIndicator.SetActive(true);
        }
        else if (!cryoCriticalState)
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
        foreach (GameObject cryopod in cryopods)
        {
            if (cryopod.GetComponent<CryopodControls>().isAlive)
            {
                cryoAliveCount += 1;
            }
        }

        // Different endings
        if (cryoAliveCount == 0)
        {
            // PepeHands ending
            gameWonCanvas0.SetActive(true);
        }
        else if (cryoAliveCount == 1)
        {
            gameWonCanvas1.SetActive(true);
        }
        else if (cryoAliveCount == 2)
        {
            gameWonCanvas2.SetActive(true);
        }
        else
        {
            gameWonCanvas3.SetActive(true);
        }
    }
}
