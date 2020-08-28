using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button newGame;
    public Button loadGame;
    public SceneLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        newGame.onClick.RemoveAllListeners();
        loadGame.onClick.RemoveAllListeners();

        newGame.onClick.AddListener(delegate { loader.LoadSceneByID(1); });
        loadGame.onClick.AddListener(delegate { loader.LoadSceneByID(1); });
    }


}
