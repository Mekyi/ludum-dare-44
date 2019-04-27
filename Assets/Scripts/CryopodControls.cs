using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class CryopodControls : MonoBehaviour
{
    [SerializeField] float fullMaintenanceCost = 20f;
    [SerializeField] float fullMaintenancePoints = 100f;
    [SerializeField] float decradeSpeed = 2f;
    [SerializeField] GameObject maintenanceIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LeanTween.scaleX(maintenanceIndicator, 0, 2f);
    }
}
