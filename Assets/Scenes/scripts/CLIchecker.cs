using UnityEngine;
using UnityEngine.UI;

public class CLIChecker : MonoBehaviour
{
    public InputField inputField; // assign this from the inspector
    public string expectedCommand = "run";

    void Start()
    {
        inputField.onEndEdit.AddListener(CheckCommand);
    }

    void CheckCommand(string playerInput)
    {
        if (playerInput.Trim().ToLower() == expectedCommand.ToLower())
        {
            Debug.Log("You Won");
            // Later: GameManager.Instance.NextLevel();
        }
        else
        {
            Debug.Log("Incorrect command");
        }

        inputField.text = ""; // Clear input
        inputField.ActivateInputField(); // Reactivate input
    }
}
