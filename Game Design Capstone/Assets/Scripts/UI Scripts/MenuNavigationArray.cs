using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigationArray : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // Array to store your buttons
    private int currentIndex = 0;
    [SerializeField] private bool isVerticalMenu;
    private bool isCooldown;
    [SerializeField] private float cooldownTime = 0.5f;

    private void Start() {
        isCooldown = false;
    }

    void Update()
    {
        if (!isCooldown)
        {
            if (isVerticalMenu)
            {
                if (Input.GetAxis("Vertical") > 0.1f)
                    MoveSelection(-1); // Move up
                else if (Input.GetAxis("Vertical") < -0.1f)
                    MoveSelection(1); // Move down
            }
            else 
            {
                if (Input.GetAxis("Horizontal") > 0.1f)
                    MoveSelection(-1); // Move left
                else if (Input.GetAxis("Horizontal") < -0.1f)
                    MoveSelection(1); // Move right
            }
            
        }

        HandleContinueInput();
    }

    private void MoveSelection(int direction)
    {
        // Deselect the current button
        buttons[currentIndex].GetComponent<Selectable>().OnDeselect(null);

        // Update the current index
        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length;

        // Select the new button
        buttons[currentIndex].Select();

        StartCoroutine(InputCooldown());
    }

    private void HandleContinueInput()
    {
        if (Input.GetButtonDown("Continue"))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                // Call specific function based on the selected button
                Button selectedButton = selected.GetComponent<Button>();
                if (selectedButton != null)
                    selectedButton.onClick.Invoke();  // Simulate button click
            }
        }
    }

    IEnumerator InputCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
