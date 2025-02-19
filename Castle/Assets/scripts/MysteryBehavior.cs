using UnityEngine;

public class MysteryBehavior : MonoBehaviour
{
    public GameBehavior gameManager;
    public GameObject hiddenObject; // Assign the hidden object in the Inspector

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure the player has the correct tag
        {
            UnhideObject();
            GrantArmor(); // Give +5 Armor
            Destroy(gameObject); // Destroy the pickup item
            gameManager.Items += 1;
        }
    }

    private void UnhideObject()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true); // Reveal the object
            gameManager.UpdateMessage("A hidden power has awakened!"); // Show in UI box
        }
        else
        {
            gameManager.UpdateMessage("Something went wrong... No hidden object found.");
        }
    }

    private void GrantArmor()
    {
        gameManager.Armor += 5; // Adds +5 Armor
        gameManager.UpdateMessage("Armor increased by 5! Current Armor: " + gameManager.Armor); // UI message
    }
}
