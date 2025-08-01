using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 3f; // How far up and down it moves
    public bool startMovingUp = true; // Direction it starts moving

    [Header("Level Restart Settings")]
    public string playerTag = "Player"; // Tag to check for collision
    public float restartDelay = 0.5f; // Delay before restarting (optional)

    private Vector3 startPosition;
    private Vector3 topPosition;
    private Vector3 bottomPosition;
    private bool movingUp;
    private bool levelRestarting = false;

    void Start()
    {
        // Store the starting position
        startPosition = transform.position;

        // Calculate the top and bottom positions
        topPosition = startPosition + Vector3.up * moveDistance;
        bottomPosition = startPosition - Vector3.up * moveDistance;

        // Set initial movement direction
        movingUp = startMovingUp;
    }

    void Update()
    {
        if (levelRestarting) return; // Stop moving if level is restarting

        MoveUpAndDown();
    }

    void MoveUpAndDown()
    {
        // Move the obstacle
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // Check if we've reached the top position
            if (transform.position.y >= topPosition.y)
            {
                movingUp = false; // Switch direction
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // Check if we've reached the bottom position
            if (transform.position.y <= bottomPosition.y)
            {
                movingUp = true; // Switch direction
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with this obstacle
        if (collision.gameObject.CompareTag(playerTag) && !levelRestarting)
        {
            RestartLevel();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Alternative trigger-based collision detection
        if (other.CompareTag(playerTag) && !levelRestarting)
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        levelRestarting = true;

        // Optional: Add a delay before restarting
        if (restartDelay > 0)
        {
            Invoke("DoRestart", restartDelay);
        }
        else
        {
            DoRestart();
        }
    }

    void DoRestart()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Optional: Visual debug to see the movement range in the editor
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            // Draw the movement range
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(topPosition, bottomPosition);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(topPosition, 0.1f);
            Gizmos.DrawWireSphere(bottomPosition, 0.1f);
        }
        else
        {
            // Show approximate range in edit mode
            Vector3 pos = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pos + Vector3.up * moveDistance, pos - Vector3.up * moveDistance);
        }
    }
}