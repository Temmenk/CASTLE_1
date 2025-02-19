using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    public GameBehavior gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealPlayer(); // Heals the player to full
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Player healed to full health!");
        }
    }

    private void HealPlayer()
    {
        if (gameManager != null)
        {
            gameManager.PlayerHealth = gameManager.MaxPlayerHealth; // Full restore
        }
        else
        {
            Debug.LogWarning("GameManager not found! Health not restored.");
        }
    }
}