using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro for UI

public class GameBehavior : MonoBehaviour
{
    public bool showWinScreen = false;
    public bool showLossScreen = false;
    public int maxItems = 4;
    private int _itemsCollected = 0;

    public int Items
    {
        get { return _itemsCollected; }
        set
        { 
            _itemsCollected = value;

            if (_itemsCollected >= maxItems)
            {
                UpdateMessage("You've found all the items!");
                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                UpdateMessage("Item found! " + (maxItems - _itemsCollected) + " more to go!");
            }
        } 
    }

    // Player Health Variables
    public int MaxPlayerHealth = 10;
    private int _playerHP = 10;
    public int PlayerHealth 
    {
        get { return _playerHP; }
        set 
        { 
            _playerHP = Mathf.Clamp(value, 0, MaxPlayerHealth);
            Debug.LogFormat("Player Health: {0}", _playerHP);

            if (_playerHP <= 0)
            {
                GameOver();
            }
        }
    }

    // Player Armor Variables
    public int PlayerArmor = 0; // Armor starts at 0
    public int MaxArmor = 20; // Maximum armor

    public int Armor
    {
        get { return PlayerArmor; }
        set
        {
            PlayerArmor = Mathf.Clamp(value, 0, MaxArmor);
            UpdateMessage("Armor Increased! Current Armor: " + PlayerArmor);
            Debug.LogFormat("Player Armor: {0}", PlayerArmor);
        }
    }

    [Header("UI Elements")]
    public TextMeshProUGUI messageText; // UI text element to display messages
    public GameObject messageBox; // The UI message box panel

    void Start()
    {
        if (messageBox != null)
            messageBox.SetActive(false); // Hide message box at start
    }

public void UpdateMessage(string message) // <-- Made public
{
    if (messageText != null && messageBox != null)
    {
        messageText.text = message;
        messageBox.SetActive(true);

        // Hide the message box after 3 seconds
        CancelInvoke(nameof(HideMessageBox));
        Invoke(nameof(HideMessageBox), 3f);
    }
}


    void HideMessageBox()
    {
        if (messageBox != null)
            messageBox.SetActive(false);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health: " + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Armor: " + PlayerArmor);
        GUI.Box(new Rect(20, 80, 150, 25), "Items Collected: " + _itemsCollected);

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!"))
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;    
            }
        }
    }
    public void GameOver()
    {
        UpdateMessage("You died! Game Over! Press 'R' to Restart.");
        Time.timeScale = 0f;
        Debug.Log("Game Over - Player is dead.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _playerHP <= 0) 
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
}

