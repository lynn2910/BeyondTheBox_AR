using TMPro;
using UnityEngine;
using System;

public class KeypadPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text displayCodeText;
    
    [Tooltip("The text displayed before the user's input.")]
    [SerializeField] private string prefixText = "192.168.1."; 
    
    [Tooltip("The correct 3-digit answer.")]
    [SerializeField] private string correctPin = "137"; 
    
    private string currentInput = "";

    public event Action OnKeypadSolved;

    private void OnEnable()
    {
        ClearInput();
    }

    public void AddDigit(string digit)
    {
        if (currentInput.Length < 3)
        {
            currentInput += digit;
            UpdateDisplay();
        }
    }

    // 'E' Button
    public void SubmitCode()
    {
        if (currentInput == correctPin)
        {
            displayCodeText.color = Color.green;
            Debug.Log("Keypad: Code Correct!");
            OnKeypadSolved?.Invoke(); 
        }
        else
        {
            displayCodeText.color = Color.red;
            Debug.Log("Keypad: Code Incorrect!");
            Invoke(nameof(ClearInput), 1f);
            Invoke(nameof(ResetColor), 1f);
        }
    }

    public void DeleteLastDigit()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    public void ClosePanel()
    {
        ClearInput();
        gameObject.SetActive(false);
    }

    private void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayCodeText.text = prefixText + currentInput.PadRight(3, '_');
    }

    private void ResetColor() => displayCodeText.color = Color.white;
}