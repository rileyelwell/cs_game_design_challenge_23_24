using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigationCustom : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // Array to store your buttons

    private int currentIndex = 0;
    private bool isVerticalAxisInUse = false;
    private bool isHorizontalAxisInUse = false;
    public bool isSubmitting;

    private void Start()
    {
        // Set the initial selection to the first button
        // buttons[currentIndex].Select();
        isSubmitting = false;
    }

    private void Update()
    {
        HandleNavigation();
        HandleContinueInput();
    }

    private void MoveSelection(int newIndex)
    {
        // Deselect the current button
        buttons[currentIndex].GetComponent<Selectable>().OnDeselect(null);

        // Update the current index
        currentIndex = newIndex;

        // Select the new button
        buttons[currentIndex].Select();
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
                    selectedButton.onClick.Invoke();
            }
        }
    }

    private void HandleNavigation()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (vertical > 0.1f && !isVerticalAxisInUse)
        {
            // Move up
            if (currentIndex == 2)
                MoveSelection(1); 
            else if (currentIndex == 3) 
                MoveSelection(0);
            isVerticalAxisInUse = true;
        } 
        else if (vertical < -0.1f && !isVerticalAxisInUse)
        {
            // Move down
            if (currentIndex == 0)
                MoveSelection(3); 
            else if (currentIndex == 1) 
                MoveSelection(2);
            isVerticalAxisInUse = true;
        }
        else if (Mathf.Abs(vertical) < 0.1f)
        {
            isVerticalAxisInUse = false;
        }

        if (horizontal > 0.1f && !isHorizontalAxisInUse)
        {
            // Move right
            if (currentIndex == 0)
                MoveSelection(1); 
            else if (currentIndex == 3) 
                MoveSelection(2);
            isHorizontalAxisInUse = true;
        }
        else if (horizontal < -0.1f && !isHorizontalAxisInUse)
        {
            // Move left
            if (currentIndex == 1)
                MoveSelection(0); 
            else if (currentIndex == 2) 
                MoveSelection(3);
            isHorizontalAxisInUse = true;
        }
        else if (Mathf.Abs(horizontal) < 0.1f)
        {
            isHorizontalAxisInUse = false;
        }
    }

    public string GetSelectedButtonName()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            return EventSystem.current.currentSelectedGameObject.name;
        else return "none";
    }
}
