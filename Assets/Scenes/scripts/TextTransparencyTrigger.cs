using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTransparencyTrigger : MonoBehaviour
{
    [Header("Text References")]
    public Text uiText; // For legacy UI Text
    public TextMeshProUGUI tmpText; // For TextMeshPro UI Text
    public TextMeshPro tmpWorldText; // For TextMeshPro World Text

    [Header("Background References")]
    public Image backgroundImage; // For UI Image/Panel background
    public SpriteRenderer backgroundSprite; // For world space sprite background

    [Header("Transparency Settings")]
    [Range(0f, 1f)]
    public float transparentAlpha = 0.1f; // How transparent it becomes
    [Range(0f, 1f)]
    public float normalAlpha = 1f; // Normal opacity

    [Header("Fade Settings")]
    public bool useFadeTransition = true;
    public float fadeSpeed = 5f;

    private Color originalTextColor;
    private Color originalBackgroundColor;
    private Color originalUnderlayColor;
    private bool isPlayerInside = false;
    private float targetAlpha;

    void Start()
    {
        // Store the original colors and set initial target alpha
        if (uiText != null)
            originalTextColor = uiText.color;
        else if (tmpText != null)
        {
            originalTextColor = tmpText.color;
            originalUnderlayColor = tmpText.fontSharedMaterial.GetColor("_UnderlayColor");
        }
        else if (tmpWorldText != null)
        {
            originalTextColor = tmpWorldText.color;
            originalUnderlayColor = tmpWorldText.fontSharedMaterial.GetColor("_UnderlayColor");
        }

        // Store background color
        if (backgroundImage != null)
            originalBackgroundColor = backgroundImage.color;
        else if (backgroundSprite != null)
            originalBackgroundColor = backgroundSprite.color;

        targetAlpha = normalAlpha;
    }

    void Update()
    {
        if (useFadeTransition)
        {
            // Smoothly fade between transparent and opaque
            float currentAlpha = Mathf.Lerp(GetCurrentAlpha(), targetAlpha, fadeSpeed * Time.deltaTime);
            SetTextAlpha(currentAlpha);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            targetAlpha = transparentAlpha;

            if (!useFadeTransition)
            {
                SetTextAlpha(transparentAlpha);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            targetAlpha = normalAlpha;

            if (!useFadeTransition)
            {
                SetTextAlpha(normalAlpha);
            }
        }
    }

    // Alternative collision methods if you're not using triggers
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            targetAlpha = transparentAlpha;

            if (!useFadeTransition)
            {
                SetTextAlpha(transparentAlpha);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
            targetAlpha = normalAlpha;

            if (!useFadeTransition)
            {
                SetTextAlpha(normalAlpha);
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        // Set text alpha
        Color newTextColor = originalTextColor;
        newTextColor.a = alpha;

        if (uiText != null)
            uiText.color = newTextColor;
        else if (tmpText != null)
        {
            tmpText.color = newTextColor;

            // Handle TextMeshPro underlay (background) alpha
            Color newUnderlayColor = originalUnderlayColor;
            newUnderlayColor.a = alpha;
            tmpText.fontSharedMaterial.SetColor("_UnderlayColor", newUnderlayColor);
        }
        else if (tmpWorldText != null)
        {
            tmpWorldText.color = newTextColor;

            // Handle TextMeshPro underlay (background) alpha
            Color newUnderlayColor = originalUnderlayColor;
            newUnderlayColor.a = alpha;
            tmpWorldText.fontSharedMaterial.SetColor("_UnderlayColor", newUnderlayColor);
        }

        // Set background alpha
        Color newBackgroundColor = originalBackgroundColor;
        newBackgroundColor.a = alpha;

        if (backgroundImage != null)
            backgroundImage.color = newBackgroundColor;
        else if (backgroundSprite != null)
            backgroundSprite.color = newBackgroundColor;
    }

    private float GetCurrentAlpha()
    {
        if (uiText != null)
            return uiText.color.a;
        else if (tmpText != null)
            return tmpText.color.a;
        else if (tmpWorldText != null)
            return tmpWorldText.color.a;

        return 1f;
    }
}