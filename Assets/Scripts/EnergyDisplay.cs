using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField] GameObject player;
    private TextMeshProUGUI energyText;

    // Start is called before the first frame update
    void Start()
    {
        energyText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        energyText.text = Mathf.Round(player.GetComponent<Player>().GetEnergy()).ToString() + "%";
    }
}
