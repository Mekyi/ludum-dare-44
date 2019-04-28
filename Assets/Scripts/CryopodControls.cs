using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CryopodControls : MonoBehaviour
{
    [SerializeField] float energyTransferSpeed = 5f;
    [SerializeField] float energyDrainSpeed = 0.3f;
    [SerializeField] float fullCharge = 20f;
    [SerializeField] GameObject maintenanceIndicator;
    [SerializeField] GameObject guideTexts;
    [SerializeField] GameObject player;
    [SerializeField] GameObject uiTimer;

    public float currentCharge;
    private RectTransform timeBar;
    private RectTransform uiBar;
    private BoxCollider2D triggerArea;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        guideTexts.SetActive(false);
        triggerArea = gameObject.GetComponent<BoxCollider2D>();
        timeBar = maintenanceIndicator.GetComponent<RectTransform>();
        currentCharge = fullCharge;
        uiBar = uiTimer.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharge >= 0)
        {
            currentCharge -= Time.deltaTime * energyDrainSpeed;
            timeBar.localScale = new Vector3 (
                currentCharge / fullCharge, 1, 1);
            UIBarUpdate();
        }
        else
        {
            isAlive = false;
        }

        if (triggerArea.IsTouchingLayers(LayerMask.GetMask("Player") ) && isAlive)
        {
            guideTexts.SetActive(true);
            if (Input.GetButton("Interact") && player.GetComponent<Player>().isAlive)
            {
                ChargeStation();
            }
        }
        else
        {
            guideTexts.SetActive(false);
        }
    }

    private void ChargeStation()
    {
        if (currentCharge < fullCharge - 0.5f)
        {
            currentCharge = Mathf.Clamp(
                                currentCharge + energyTransferSpeed * Time.deltaTime,
                                0,
                                fullCharge);
            player.GetComponent<Player>().DecreaseEnergy(energyTransferSpeed * Time.deltaTime);
        }
    }

    private void UIBarUpdate()
    {
        uiBar.localScale = new Vector3(
                currentCharge / fullCharge, 1, 1);
    }
}
