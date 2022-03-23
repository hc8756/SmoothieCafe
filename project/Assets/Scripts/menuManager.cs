using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menuManager : MonoBehaviour
{
    public static menuManager instance;
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one menu manager in scene");
        }
        instance = this;

        if (SceneManager.GetActiveScene().name == "Ending") { 
            blackBox= GameObject.FindGameObjectWithTag("blackbox");
            blackBoxCG = blackBox.GetComponent<CanvasGroup>();
            blackBoxCG.alpha = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (blackBoxCG != null) {
            blackBoxCG.alpha -= 0.3f * Time.deltaTime;
        }
        
    }
    //Add code that fades in if it is the ending scene
    public void loadScene() {
        SceneManager.LoadScene("Master_Scene");
    }
}
