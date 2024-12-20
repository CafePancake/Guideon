using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    [Header("Référence UI")]
    [SerializeField] GameObject _instructionSort;
    [SerializeField] GameObject _instructionNormal; 

    public void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
        public void GoPlay()
    {
        SceneManager.LoadScene("Generateur");
    }
        public void GoToInstructions()
    {
        SceneManager.LoadScene("Instruction");
    }
        public void GoToInstructionsSort()
    {
        _instructionNormal.SetActive(false);
        _instructionSort.SetActive(true);
    }
        public void GoToInstructionsNormal()
    {
        _instructionNormal.SetActive(true);
        _instructionSort.SetActive(false);
    }
    public void DefaiteScene()
    {
        SceneManager.LoadScene("Défaite");
    }
    public void VictoireScene()
    {
        SceneManager.LoadScene("Victoire");
    }
        public void QuitGame()
    {
        #if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quit the application in standalone or player mode
        Application.Quit();
        #endif
    }
}
