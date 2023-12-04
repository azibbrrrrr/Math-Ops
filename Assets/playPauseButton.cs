using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour
{
    private bool isPlaying = true;
    public Text buttonTxt;

    void Start()
    {
        // Set the initial state of the game
        SetGameState(isPlaying);
        buttonTxt.text = "Pause";
    }

    // This method is called when the button is clicked
    public void OnButtonClick()
    {
        // Toggle the play/pause state
        isPlaying = !isPlaying;
        buttonTxt.text = isPlaying ? "Pause" : "Play";

        // Update the game state
        SetGameState(isPlaying);
    }

    // Method to set the game state based on play/pause state
    private void SetGameState(bool playState)
    {
        if (playState)
        {
            // Resume the game or start playing
            Time.timeScale = 1f;
            Debug.Log("Game Resumed");
        }
        else
        {
            // Pause the game
            Time.timeScale = 0f;
            Debug.Log("Game Paused");
        }

        // You can add more logic here based on your game requirements
    }
}
