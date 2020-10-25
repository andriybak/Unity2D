using TMPro;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private TextMeshProUGUI UIHealthText;

    public int currentHealth = 100;

    private void SetUIHealthText()
    {
        if (this.UIHealthText != null)
        {
            this.UIHealthText.text = $"HP: {this.currentHealth}";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = 100;
        this.UIHealthText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(int health)
    {
        this.currentHealth = health;
        this.SetUIHealthText();
    }

    public void DecreaseHealth(int decreaseNumber)
    {
        this.currentHealth -= decreaseNumber;
        this.SetUIHealthText();
    }
}
