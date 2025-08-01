using UnityEngine;

public class Node : MonoBehaviour
{
    public int nodeValue = 1; // or whatever value you want
    public Sprite unlitSprite;
    public Sprite litSprite;

    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unlitSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActivated) return;

        if (collision.gameObject.CompareTag("Player")) // or "Ball", etc.
        {
            ActivateNode();
        }
    }

    void ActivateNode()
    {
        isActivated = true;
        spriteRenderer.sprite = litSprite;

        // Inform game manager
        GameManager.Instance.AddToOutput(nodeValue);

        Debug.Log("Node Activated! Value: " + nodeValue);
    }
}
