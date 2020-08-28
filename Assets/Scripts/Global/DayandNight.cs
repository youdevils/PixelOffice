using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayandNight : MonoBehaviour
{
    [SerializeField]
    private Light2D light2D;

    private float maxLight = 1f;
    private float minLight = 0.2f;
    private float currentLight;

    private bool isTurningDay = true;

    private bool isDay;
    public bool IsDay {
        get {
            return isDay;
        }
    }

    private void Start() {
        currentLight = minLight;
    }
    // Update is called once per frame
    void Update()
    {
        if (isTurningDay) {
            if (currentLight < maxLight) {
                currentLight += (0.01f * Time.deltaTime);
            } else {
                isTurningDay = false;
            }
        } else {
            if (currentLight > minLight) {
                currentLight -= (0.01f * Time.deltaTime);
            } else {
                isTurningDay = true;
            }
        }
        light2D.intensity = currentLight;
        if(currentLight > 0.65f) {
            isDay = true;
        } else {
            isDay = false;
        }
    }
}
