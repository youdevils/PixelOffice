using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StreetLight : MonoBehaviour
{
    [SerializeField]
    private GameObject myLight;

    [SerializeField]
    private DayandNight dayandNight;
    // Update is called once per frame
    void Update()
    {
        if (!dayandNight.IsDay) {
            myLight.SetActive(true);
        } else {
            myLight.SetActive(false);
        }
    }
}
