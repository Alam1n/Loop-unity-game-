using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int output = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddToOutput(int value)
    {
        output += value;
        Debug.Log("Output Total: " + output);
    }
}
