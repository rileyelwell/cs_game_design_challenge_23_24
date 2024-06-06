using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizerScreen : MonoBehaviour
{
    [SerializeField] private Image currentDisplay;              // The UI Image component to display
    [SerializeField] Sprite[] sprites;
    private int currentIndex = 0;
    [SerializeField] float debounceTime = 0.5f; // Time in seconds to wait before allowing another click
    private bool canClick = true;

    void Start()
    {
        if (sprites.Length > 0)
            UpdateDisplay();
    }

    public void PreviousSelection()
    {
        if (canClick)
        {
            StartCoroutine(ClickCooldown());
            if (sprites.Length == 0) return;

            currentIndex--;
            if (currentIndex < 0)
                currentIndex = sprites.Length - 1; // Wrap around to the last hat
            UpdateDisplay();
        }
        
    }

    public void NextSelection()
    {
        if (canClick)
        {
            print("true_before");
            StartCoroutine(ClickCooldown());
            if (sprites.Length == 0) return;

            currentIndex++;
            if (currentIndex >= sprites.Length)
                currentIndex = 0; // Wrap around to the first hat
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        currentDisplay.sprite = sprites[currentIndex];
    }

    public int GetCurrentIndex() { return currentIndex; }

    private IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSecondsRealtime(debounceTime);
        print("true_after");
        canClick = true;
    }
}
