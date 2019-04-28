using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlSystem : MonoBehaviour
{
    
    [SerializeField] GameObject gameplayManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject guideText;

    public bool isRandomEvent;
    private BoxCollider2D triggerArea;

    // Start is called before the first frame update
    void Start()
    {
        guideText.SetActive(false);
        triggerArea = gameObject.GetComponent<BoxCollider2D>();
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
                    gameplayManager.GetComponent<GameplayManager>().fixCourse = false;
                    guideText.SetActive(false);
                }
            }
        }
        else
        {
            guideText.SetActive(false);
        }

    }

    private void CheckForEvent()
    {
        isRandomEvent = gameplayManager.GetComponent<GameplayManager>().fixCourse;
    }
}
