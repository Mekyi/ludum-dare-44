using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] float energy = 100;
    [SerializeField] float maxEnergy = 100;
    [SerializeField] float energyTransferSpeed = 10;
    [SerializeField] float energyRegen = 1;

    [SerializeField] GameObject player;
    [SerializeField] GameObject valueComponent;

    private CapsuleCollider2D triggerArea;
    private TextMeshPro chargeText;

    // Start is called before the first frame update
    void Start()
    {
        triggerArea = gameObject.GetComponent<CapsuleCollider2D>();
        chargeText = valueComponent.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        energy += energyRegen * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0f, 100f);
        chargeText.text = Mathf.Round(energy).ToString() + "%";
        if (triggerArea.IsTouchingLayers(LayerMask.GetMask("Player"))
            && player.GetComponent<Player>().GetEnergy() < 100f)
        {
            energy -= Time.deltaTime * energyTransferSpeed;
            player.GetComponent<Player>().IncreaseEnergy(Time.deltaTime * energyTransferSpeed);
        }
    }
}
