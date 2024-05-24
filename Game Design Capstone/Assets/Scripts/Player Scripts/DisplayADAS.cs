using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DisplayADAS : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image leftSensorImg, rightSensorImg, backSensorImg, frontSensorImg; 

    public void DisplaySensorActive(bool front, bool back, bool left, bool right) {
        // if there is a signal from ADAS, display red outline, otherwise keep greyed out
        frontSensorImg.color = front ? Color.red : Color.white;
        backSensorImg.color = back ? Color.red : Color.white;
        leftSensorImg.color = left ? Color.red : Color.white;
        rightSensorImg.color = right ? Color.red : Color.white;
    }
}
