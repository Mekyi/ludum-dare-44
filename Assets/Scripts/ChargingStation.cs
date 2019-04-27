using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] float energy = 100;
    [SerializeField] float maxEnergy = 100;
    [SerializeField] float energyTransferSpeed = 10;
    [SerializeField] float energyRegen = 1;

    BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        energy += energyRegen * Time.deltaTime;
        if (collider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {

        }
    }
}
