using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlSystem : MonoBehaviour
{
    
    [SerializeField] GameObject gameplayManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject guideText;
    [SerializeField] float randomEventChance = 0.05f;
    [SerializeField] float diceRollInterval = 1f;

    [SerializeField] bool isControlPanel = false;
    [SerializeField] bool isLeftEngine = false;
    [SerializeField] bool isRightEngine = false;

    public bool isRandomEvent;
    private BoxCollider2D triggerArea;

    // Start is called before the first frame update
    void Start()
    {
        guideText.SetActive(false);
        triggerArea = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(RollForEvent());
    }

    // Update is called once per frame
    void Update()
    {

        // Player contact
        if (triggerArea.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            CheckForEvent();
            if (isRandomEvent)
            {
                guideText.SetActive(true);
                if (Input.GetButton("Interact") && player.GetComponent<Player>().isAlive)
                {
                    if (isControlPanel)
                    {
                        gameplayManager.GetComponent<GameplayManager>().fixCourse = false;
                    }
                    else if (isLeftEngine)
                    {
                        gameplayManager.GetComponent<GameplayManager>().fixLeftEngine = false;
                    }
                    else if (isRightEngine)
                    {
                        gameplayManager.GetComponent<GameplayManager>().fixRightEngine = false;
                    }

                    guideText.SetActive(false);
                }
            }
        }
        else
        {
            guideText.SetActive(false);
        }

        HandleRandomEvents();

    }

    private void HandleRandomEvents()
    {
    }

    IEnumerator RollForEvent()
    {
        yield return new WaitForSeconds(diceRollInterval);
        if (randomEventChance > UnityEngine.Random.value)
        {
            if (isControlPanel)
            {
                gameplayManager.GetComponent<GameplayManager>().fixCourse = true;
            }
            else if (isLeftEngine)
            {
                gameplayManager.GetComponent<GameplayManager>().fixLeftEngine = true;
            }
            else if (isRightEngine)
            {
                gameplayManager.GetComponent<GameplayManager>().fixRightEngine = true;
            }
        }
        StartCoroutine(RollForEvent());
    }

    private void CheckForEvent()
    {
        if (isControlPanel)
        {
            isRandomEvent = gameplayManager.GetComponent<GameplayManager>().fixCourse;
        }
        else if (isLeftEngine)
        {
            isRandomEvent = gameplayManager.GetComponent<GameplayManager>().fixLeftEngine;
        }
        else if (isRightEngine)
        {
            isRandomEvent = gameplayManager.GetComponent<GameplayManager>().fixRightEngine;
        }
    }
}
