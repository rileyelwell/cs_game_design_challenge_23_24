using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DisplayADAS : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image leftSensorImg, rightSensorImg, backSensorImg, frontSensorImg; 

    // [SerializeField] private Color baseColor = 636363;

    public void DisplaySensorActive(bool front, bool back, bool left, bool right) {
        // if there is a signal from ADAS, display red outline, otherwise keep greyed out
        frontSensorImg.color = front ? Color.red : Color.gray;
        backSensorImg.color = back ? Color.red : Color.gray;
        leftSensorImg.color = left ? Color.red : Color.gray;
        rightSensorImg.color = right ? Color.red : Color.gray;
    }
}
