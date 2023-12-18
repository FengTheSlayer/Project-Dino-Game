using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

public class menuScript : MonoBehaviour
{
    //character selection
    public GameObject[] skins;
    public int selectedCharacter;
    public Animator transition;

    /*
    private void Awake()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject player in skins)
        {
            player.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);
    }
    */
    public void next()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        
        if (selectedCharacter == skins.Length)
        {
            selectedCharacter = 0;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
    }
    
    public void back()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        
        if (selectedCharacter == -1)
        {
            selectedCharacter = skins.Length -1;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
    }
    //person presses Start Game, they will be sent to the game
    public void mainGame()
    {
        transition.SetTrigger("start");
        StartCoroutine(sceneTransition());
    }

    public IEnumerator sceneTransition()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("mainGame");
    }

    public void testStage()
    {
        SceneManager.LoadScene("Stage Test");
    }
    //player quits or exits game entirely
    public void Quit()
    {
        Application.Quit();
    }
    
    public void backTomainMenu()
    {
        SceneManager.LoadScene("GUImenu");
    }
}
