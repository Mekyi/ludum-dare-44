using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlSystem : MonoBehaviour
{
    
    [SerializeField] GameObject gameplayManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject guideText;
    [SerializeField] GameObject alertIndicator;
    [SerializeField] float randomEventChance = 0.05f;
    [SerializeField] float diceRollInterval = 1f;
    [SerializeField] float fixEnergyCost = 10f;
    [SerializeField] float fixCooldown = 20f;

    [SerializeField] bool isControlPanel = false;
    [SerializeField] bool isLeftEngine = false;
    [SerializeField] bool isRightEngine = false;

    public bool isRandomEvent;
    public float fixCooldownTimer;

    private BoxCollider2D triggerArea;
    private AudioSource consoleAudio;

    // Start is called before the first frame update
    void Start()
    {
        guideText.SetActive(false);
        alertIndicator.SetActive(false);
        triggerArea = gameObject.GetComponent<BoxCollider2D>();
        consoleAudio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(RollForEvent());
        fixCooldownTimer = fixCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEvent();
        HandleRandomEvents();

        // Player contact
        if (triggerArea.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
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

                    consoleAudio.Play();
                    player.GetComponent<Player>().DecreaseEnergy(fixEnergyCost);
                    guideText.SetActive(false);
                    fixCooldownTimer = fixCooldown;
                }
            }
        }
        else
        {
            guideText.SetActive(false);
        }


    }

    private void HandleRandomEvents()
    {
        if (isRandomEvent)
        {
            alertIndicator.SetActive(true);
        }
        else
        {
            alertIndicator.SetActive(false);
        }
    }

    IEnumerator RollForEvent()
    {
        yield return new WaitForSeconds(diceRollInterval);
        if (randomEventChance > UnityEngine.Random.value && fixCooldownTimer < 0f)
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
        fixCooldownTimer -= Time.deltaTime;

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
