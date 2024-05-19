using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodHealthbar : MonoBehaviour
{
    [SerializeField] private Image foodBar;

    public void SetHealth(int health)
    {
        if (health <= 0 & health <= 1.0)
        {
            foodBar.fillAmount = health;
        }

        // change the color of the health bar fill based on the current health
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
