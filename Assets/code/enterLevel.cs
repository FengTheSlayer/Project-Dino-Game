using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enterLevel : MonoBehaviour
{
    public GameObject button;
    public string levelSelect = "Stage Test";
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            button.gameObject.SetActive(true);
            
            if (Input.GetKey(KeyCode.Z))
            {
                SceneManager.LoadScene(levelSelect);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        button.gameObject.SetActive(false);
    }
    
}
