using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public static manager instance;

    //characters
    public GameObject player;
    public GameObject friend;
    public GameObject customer1;
    public GameObject customer2;

    //variables related to dialogue UI
    public Canvas dialogueBox;
    public static bool dialogueOpen;
    public Text speakerName;
    public Text speakerDialogue;

    //Ink dialogue variables
    private Story currentStory;
    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private Text[] choicesText;

    public static int convoNum;
    private string convoName;
    private bool knowledge1;
    //positions of buttons when there are all 3 
    private Vector3 butPos1 = new Vector3(0, 36, 0);
    private Vector3 butPos2 = new Vector3(0, 0, 0); //this is also pos of button when there is 1
    private Vector3 butPos3 = new Vector3(0, -36, 0);

    //positions of buttons when there are 2
    private Vector3 butPos4 = new Vector3(0, 18, 0);
    private Vector3 butPos5 = new Vector3(0, -18, 0);

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one manager in scene");
        }
        instance = this;
    }

    void Start()
    {
        dialogueOpen = true;
        choicesText = new Text[choices.Length];
        convoNum=0;
        convoName = "Convo0";
        knowledge1 = false;
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<Text>();
            index++;
        }
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        dialogueOpen = true;
        ContinueStory();
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)currentStory.variablesState["knowledge1"] > 0) { knowledge1 = true; }
        dialogueBox.enabled = dialogueOpen;
        //set correct speaker name 
        if ((string)currentStory.variablesState["speaker"] == "Narrator")
        {
            speakerName.text = "";
        }
        else
        {
            speakerName.text = (string)currentStory.variablesState["speaker"];
        }
        //if the story runs out of lines, close the dialogue box and judge if there is another convo that can happen
        if (speakerDialogue.text == "")
        {
            dialogueOpen = false;
            if (convoNum < 2)
            {
             
                convoName = String.Concat("Convo", convoNum+1);

                currentStory = new Story(inkJSON.text);
                currentStory.ChoosePathString(convoName);

                if (convoNum == 0)
                {
                    friend.GetComponent<friend>().anim.SetBool("leaving", true);
                    customer1.GetComponent<customer1>().anim.SetBool("entering", true);
                    currentStory.variablesState["speaker"] = "Customer1";
                    if (knowledge1) { currentStory.variablesState["knowledge1"] = 1; }
                }
                
                else if (convoNum == 1)
                {
                    customer1.GetComponent<customer1>().anim.SetBool("standing", false);
                    customer1.GetComponent<customer1>().anim.SetBool("leaving", true);
                    customer2.GetComponent<customer2>().anim.SetBool("entering", true);
                    currentStory.variablesState["speaker"] = "Customer2";
                    if (knowledge1) { currentStory.variablesState["knowledge1"] = 1; }
                }
                ContinueStory();
            }
            else {
                customer2.GetComponent<customer2>().anim.SetBool("standing", false);
                customer2.GetComponent<customer2>().anim.SetBool("leaving", true);
            }
            
        }
    }


    //loads next dialogue from story as long as there are more lines to read
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            speakerDialogue.text = currentStory.Continue();
            DisplayChoices();
        }
    }

    //loads user choices from story
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //just in case
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than UI can support");
        }

        int index = 0;
        List<int> realIndices = new List<int>();
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            if (choice.text == "Padding")
            {
                choices[index].gameObject.SetActive(false);
            }
            else
            {
                realIndices.Add(index);
            }
            index++;
        }

        if (realIndices.Count == 3)
        {
            choices[realIndices[0]].transform.localPosition = butPos1;
            choices[realIndices[1]].transform.localPosition = butPos2;
            choices[realIndices[2]].transform.localPosition = butPos3;
        }
        else if (realIndices.Count == 2)
        {
            choices[realIndices[0]].transform.localPosition = butPos4;
            choices[realIndices[1]].transform.localPosition = butPos5;
        }
        else if (realIndices.Count == 1)
        {
            choices[realIndices[0]].transform.localPosition = butPos2;
        }

        realIndices.Clear();
    }

    //linked to user choice buttons
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
