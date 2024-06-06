using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    private bool isVerticalAxisInUse = false;
    private bool isHorizontalAxisInUse = false;

    private void Update()
    {
        if (Input.inputString != "") Debug.Log(Input.inputString);
        HandleNavigationInput();
        HandleContinueInput();
    }

    private Selectable FindFirstSelectable()
    {
        // Find the first selectable UI element in the canvas
        Selectable firstSelectable = null;
        foreach (Selectable selectable in Selectable.allSelectablesArray)
        {
            if (selectable.gameObject.activeInHierarchy && selectable.interactable)
            {
                firstSelectable = selectable;
                break;
            }
        }
        return firstSelectable;
    }

    private void HandleNavigationInput()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Optionally set a default button if none is selected
            EventSystem.current.SetSelectedGameObject(FindFirstSelectable().gameObject);
            return;
        }

        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null)
        {
            Selectable currentSelectable = selected.GetComponent<Selectable>();

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            if (vertical > 0.1f && !isVerticalAxisInUse)
            {
                MoveSelection(currentSelectable.FindSelectableOnUp());
                isVerticalAxisInUse = true;
            }
            else if (vertical < -0.1f && !isVerticalAxisInUse)
            {
                MoveSelection(currentSelectable.FindSelectableOnDown());
                isVerticalAxisInUse = true;
            }
            else if (Mathf.Abs(vertical) < 0.1f)
            {
                isVerticalAxisInUse = false;
            }

            if (horizontal > 0.1f && !isHorizontalAxisInUse)
            {
                MoveSelection(currentSelectable.FindSelectableOnRight());
                isHorizontalAxisInUse = true;
            }
            else if (horizontal < -0.1f && !isHorizontalAxisInUse)
            {
                MoveSelection(currentSelectable.FindSelectableOnLeft());
                isHorizontalAxisInUse = true;
            }
            else if (Mathf.Abs(horizontal) < 0.1f)
            {
                isHorizontalAxisInUse = false;
            }
        }
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

    private void MoveSelection(Selectable nextSelectable)
    {
        if (nextSelectable != null)
            EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
    }
}
