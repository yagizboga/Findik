using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] Button NewGameButton;
    [SerializeField] Button ContinueButton;


    void Start()
    {
        if(!DataPersistanceManager.instance.HasGameData()){
            ContinueButton.interactable = false;
        }
    }
    public void OnNewGameClicked(){
        DisableMenuButtons();
        DataPersistanceManager.instance.NewGame();
        SceneManager.LoadSceneAsync(0);
    }
    public void OnContinueClicked(){
        DisableMenuButtons();
        Debug.Log("continue");
        SceneManager.LoadSceneAsync(0);
    }

    void DisableMenuButtons(){
        NewGameButton.interactable =false;
        ContinueButton.interactable = false;
    }
}
