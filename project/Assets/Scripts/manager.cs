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

    //sound 
    private AudioSource audioSource;
    public AudioClip blenderClip;
    public AudioClip pickupClip;

    //characters
    public GameObject player;
    public GameObject friend;
    public GameObject customer1;
    public GameObject customer2;
    private string[] names = { "Greg", "Kattie", "Tailsy" };

    //variables related to dialogue UI
    public Canvas dialogueBox;
    public static bool dialogueOpen;
    public Text speakerName;
    public Text speakerDialogue;

    //Ink dialogue variables
    private Story currentStory;
    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;
    [SerializeField] private Text responseText;
    public static int convoNum;
    private string convoName;

    //variables related to cooking
    public Text Instructions;
    public GameObject sDrink;
    public GameObject bDrink;
    public GameObject sbDrink;
    public GameObject eDrink;

    private int[,] smoothies;
    private int[,] orders;

    private bool canMakeSmoothie = false;
    private bool canGiveSmoothie = false;

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one manager in scene");
        }
        instance = this;
        audioSource= GetComponent<AudioSource>();
    }

    void Start()
    {
        //dialogue related
        convoNum =0;
        convoName = String.Concat("Convo", convoNum);
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        dialogueOpen = false;
        ContinueStory();
        //initialize 2D arrays for cooking
        //each column represents ice, strawberries, bananas, and milk
        smoothies =new int[,]{
           { 0, 0, 0, 0 },
           { 0, 0, 0, 0 },
           { 0, 0, 0, 0 }
        };
        orders = new int[,]{
            { 1, 1, 1, 0 },
            { 0, 1, 0, 1 },
            { 1, 0, 1, 1 }
        };
    }

    void Update()
    {
        //general dialogue setup
        if (convoNum >= 0) { speakerName.text = names[convoNum]; }
        dialogueBox.enabled = dialogueOpen;
        //if the story runs out of lines, close the dialogue box and alter correct variables depending on the conversation that just ended
        if (speakerDialogue.text == "")
        {
            friend.GetComponent<friend>().anim.SetBool("Talking", false);
            customer1.GetComponent<customer1>().anim.SetBool("Talking", false);
            customer2.GetComponent<customer2>().anim.SetBool("Talking", false);
            if (convoName == "Convo0" || convoName == "Convo1" || convoName == "Convo2") {
                canMakeSmoothie = true;
            }
            if (convoName == "Convo0Good" || convoName == "Convo0Bad") { 
                canMakeSmoothie = false; 
                friend.GetComponent<friend>().anim.SetBool("Leaving", true); 
            }
            else if (convoName == "Convo1Good" || convoName == "Convo1Bad") { 
                canMakeSmoothie = false; 
                customer1.GetComponent<customer1>().anim.SetBool("Leaving", true); 
            }
            else if (convoName == "Convo2Good" || convoName == "Convo2Bad") {
                canMakeSmoothie = false;
                customer2.GetComponent<customer2>().anim.SetBool("Leaving", true);
            }
        }

        //if we are currently in smoothie making mode && player is facing ingredients
        if (canMakeSmoothie)
        {
            if (playerAngle() > -120 && -60 > playerAngle())
            {
                if (player.GetComponent<Transform>().position.x > 3)
                {
                    Instructions.text = "Press E to blend ingredients";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Instructions.text = "";
                        StartCoroutine(WaitForBlender());
                    }
                }
                else if (3 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > 1)
                {
                    Instructions.text = "Press E to put ice in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        smoothies[convoNum,0] = 1;
                        audioSource.PlayOneShot(pickupClip, 1);
                    }
                }
                else if (1 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x >= 0)
                {
                    Instructions.text = "Press E to put strawberries in blender";
                    if (Input.GetKeyDown(KeyCode.E) && canMakeSmoothie)
                    {
                        smoothies[convoNum,1] = 1;
                        audioSource.PlayOneShot(pickupClip, 1);
                    }
                }
                else if (0 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > -3)
                {
                    Instructions.text = "Press E to put bananas in blender";
                    if (Input.GetKeyDown(KeyCode.E) && canMakeSmoothie)
                    {
                        smoothies[convoNum,2] = 1;
                        audioSource.PlayOneShot(pickupClip, 1);
                    }
                }
                else if (-3 > player.GetComponent<Transform>().position.x)
                {
                    Instructions.text = "Press E to put milk in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        smoothies[convoNum,3] = 1;
                        audioSource.PlayOneShot(pickupClip, 1);
                    }
                }
                else
                {
                    Instructions.text = "";
                }
            }
            else
            {
                Instructions.text = "";
            }
        }

        //if we finished the smoothie and are facing customer
        if (canGiveSmoothie)
        {
            if (playerAngle() > 60 && 120 > playerAngle())
            {
                if (1 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > -1)
                {
                    Instructions.text = "Press E to give drink to customer";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Instructions.text = "";
                        sbDrink.SetActive(false);
                        sDrink.SetActive(false);
                        bDrink.SetActive(false);
                        eDrink.SetActive(false);
                        canGiveSmoothie = false;
                        canMakeSmoothie = false;
                        if (convoNum == 0)
                        {
                            friend.GetComponent<friend>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothies, orders, 0))
                            {
                                LoadNewStory("Convo0Good");
                            }
                            else
                            {
                                LoadNewStory("Convo0Bad");
                            }
                        }
                        if (convoNum == 1)
                        {
                            customer1.GetComponent<customer1>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothies, orders, 1))
                            {
                                LoadNewStory("Convo1Good");
                            }
                            else
                            {
                                LoadNewStory("Convo1Bad");
                            }
                        }
                        if (convoNum == 2)
                        {
                            customer2.GetComponent<customer2>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothies, orders, 2))
                            {
                                LoadNewStory("Convo2Good");
                            }
                            else
                            {
                                LoadNewStory("Convo2Bad");
                            }
                        }
                    }
                }
                else
                {
                    Instructions.text = "";
                }
            }
            else
            {
                Instructions.text = "";
            }
        }
    }

    //loads next dialogue from story as long as there are more lines to read
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            speakerDialogue.text = currentStory.Continue();
            if (currentStory.currentChoices.Count > 0) { 
            responseText.text = currentStory.currentChoices[0].text;}
        }
    }

    //hooked to response button
    public void MakeChoice()
    {
        currentStory.ChooseChoiceIndex(0);
        ContinueStory();
    }

    public void LoadNewStory(string newConvo) {
        convoName = newConvo;
        currentStory.ChoosePathString(newConvo);
        ContinueStory();
    }

    private bool CompareArray(int[,] a1, int[,] a2, int rowNum) {
        bool equals = true;
        for (int i = 0; i < 4; i++) { 
            if (a1[rowNum,i] != a2[rowNum, i]) { equals = false; } 
        }
        return equals;
    }

    //wait for blender to run and then create drink
    IEnumerator WaitForBlender()
    {
        audioSource.PlayOneShot(blenderClip, 0.4f);
        convoName = "";
        canMakeSmoothie = false;
        yield return new WaitForSeconds(4);
        canGiveSmoothie = true;
        if (smoothies[convoNum,1] == 1 && smoothies[convoNum, 2] == 1)
        {
            sbDrink.SetActive(true);
        }
        else if (smoothies[convoNum, 1] == 1)
        {
            sDrink.SetActive(true);
        }
        else if (smoothies[convoNum, 2] == 1)
        {
            bDrink.SetActive(true);
        }
        else {
            eDrink.SetActive(true);
        }
    }

    private float playerAngle() {
        return Mathf.Atan2(player.GetComponent<Transform>().forward.z, player.GetComponent<Transform>().forward.x) * Mathf.Rad2Deg;
    }
}
