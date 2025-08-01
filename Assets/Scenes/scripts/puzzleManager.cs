using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private List<int> correctOrder = new List<int>(); // e.g., [1, 2, 3]
    private List<int> currentOrder = new List<int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Define the expected order of node activations
        correctOrder = new List<int> { 1, 2, 3 }; // Update this as needed
    }

    public void RegisterNodeActivation(int nodeValue)
    {
        currentOrder.Add(nodeValue);
        Debug.Log("Current Sequence: " + string.Join(", ", currentOrder));

        if (currentOrder.Count == correctOrder.Count)
        {
            if (IsCorrectOrder())
            {
                Debug.Log("Puzzle Solved!");
                // Trigger success event
            }
            else
            {
                Debug.Log("Wrong Order. Try again.");
                // Reset logic here
                currentOrder.Clear();
                // You may want to reset the nodes too (e.g., via an event)
            }
        }
    }

    private bool IsCorrectOrder()
    {
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (currentOrder[i] != correctOrder[i])
                return false;
        }
        return true;
    }
}
