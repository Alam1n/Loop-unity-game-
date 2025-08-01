using UnityEngine;

public class PuzzleNode : MonoBehaviour
{
    public Sprite litSprite;
    public Sprite unlitSprite;
    public int nodeValue = 1;
    private SpriteRenderer sr;
    private bool isActivated = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = unlitSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            sr.sprite = litSprite;
            isActivated = true;
            Debug.Log("Node activated, value: " + nodeValue);
            // Notify GameManager or accumulate value here
            //PuzzleManager.Instance.RegisterNodeActivation(nodeValue);
        }
    }
}