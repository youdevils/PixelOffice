using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public enum GameState {
        Playing,
        DisplayingUI
    }
    private GameState gameState;
    public GameState GetState {
        get {
            return gameState;
        }
    }

    public static GameStateManager instance;
    // Use this for initialization
    public GameStateManager() {
        if(instance != null) {
            Debug.LogError("Cant have more than 1 Game State Manager");
            return;
        }
        instance = this;
        gameState = GameState.Playing;
    }

    public void SetState(GameState _new) {
        gameState = _new;
    }

}
