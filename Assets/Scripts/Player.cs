using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    // Movement
    [SerializeField] float horizontalSpeed = 50f;
    [SerializeField] float verticalSpeed = 50f;

    // Energy
    [SerializeField] GameObject energyText;
    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float energyLevel = 100f;
    [SerializeField] float passiveEnergyDrain = 1f;
    [SerializeField] float energyDrainInterval = 1f;

    public bool isMovable = true;
    public bool isAlive = true;

    private Rigidbody2D rb;
    Vector2 direction = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(DrainEnergy());
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnergy();
        GetInput();
    }

    private void CheckEnergy()
    {
        energyLevel = Mathf.Clamp(energyLevel, 0f, maxEnergy);
        if (GetEnergy() <= 0)
        {
            isMovable = false;
            isAlive = false;
        }
    }

    void FixedUpdate()
    {
        FlipSprite();
        if (isMovable)
        {
            rb.velocity = new Vector2(direction.x * horizontalSpeed * Time.deltaTime, direction.y * verticalSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    #region MOVEMENT
    private void GetInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal") * horizontalSpeed;
        direction.y = Input.GetAxisRaw("Vertical") * verticalSpeed;
        direction.Normalize();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
    #endregion

    public void DecreaseEnergy(float energy)
    {
        energyLevel -= energy;
    }

    public void IncreaseEnergy(float energy)
    {
        energy += energy;
    }

    IEnumerator DrainEnergy()
    {
        yield return new WaitForSeconds(energyDrainInterval);
        if (isAlive)
        {
            DecreaseEnergy(passiveEnergyDrain);
            StartCoroutine(DrainEnergy());
        }
    }

    public float GetEnergy()
    {
        return energyLevel;
    }
}