using TMPro;
using UnityEngine;
using System;

public class BinaryKeypadPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text displayCodeText;
    
    [Tooltip("13 in binary is 1101")]
    [SerializeField] private string correctBinaryPin = "1101"; 
    
    private string currentInput = "";

    public event Action OnBinarySolved;

    private void OnEnable()
    {
        ClearInput();
    }

    public void AddDigit(string digit)
    {
        if (currentInput.Length < correctBinaryPin.Length)
        {
            currentInput += digit;
            UpdateDisplay();
        }
    }

    public void SubmitCode()
    {
        if (currentInput == correctBinaryPin)
        {
            displayCodeText.color = Color.green;
            Debug.Log("Binary Keypad: Code Correct!");
            OnBinarySolved?.Invoke(); 
        }
        else
        {
            displayCodeText.color = Color.red;
            Debug.Log("Binary Keypad: Code Incorrect!");
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

    private void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayCodeText.text = currentInput.PadRight(correctBinaryPin.Length, '_');
    }

    private void ResetColor() => displayCodeText.color = Color.white;
}