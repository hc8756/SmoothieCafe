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

    //each num represents ice, strawberry, banana, milk
    private int[] smoothie0 = { 0, 0, 0, 0 };
    private int[] smoothie1 = { 0, 0, 0, 0 };
    private int[] smoothie2 = { 0, 0, 0, 0 };

    private int[] order0 = { 1, 1, 1, 0 };
    private int[] order1 = { 0, 1, 0, 1 };
    private int[] order2 = { 1, 0, 1, 1 };

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
    }

    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        //dialogue related
        convoNum =0;
        convoName = String.Concat("Convo", convoNum);
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        dialogueOpen = false;
        ContinueStory();
    }

    // Update is called once per frame
    void Update()
    {
        //general dialogue box setup
        if (convoNum >= 0) { speakerName.text = names[convoNum]; }
        dialogueBox.enabled = dialogueOpen;
        
        //if the story runs out of lines, close the dialogue box and judge if there is another convo that can happen
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
            if (Mathf.Abs(player.GetComponent<Transform>().eulerAngles.y) > 150 && 200 > Mathf.Abs(player.GetComponent<Transform>().eulerAngles.y))
            {
                if (player.GetComponent<Transform>().position.x > 3)
                {
                    Instructions.text = "Press E to blend ingredients";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(WaitForBlender());
                        canMakeSmoothie = false;
                        audioSource.PlayOneShot(blenderClip,0.6f);
                    }
                }

                else if (3 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > 1)
                {
                    Instructions.text = "Press E to put ice in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (convoNum == 0) { smoothie0[0] = 1; }
                        if (convoNum == 1) { smoothie1[0] = 1; }
                        if (convoNum == 2) { smoothie2[0] = 1; }
                        audioSource.PlayOneShot(pickupClip, 2);
                    }
                }
                else if (1 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x >= 0)
                {
                    Instructions.text = "Press E to put strawberries in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (convoNum == 0) { smoothie0[1] = 1; }
                        if (convoNum == 1) { smoothie1[1] = 1; }
                        if (convoNum == 2) { smoothie2[1] = 1; }
                        audioSource.PlayOneShot(pickupClip, 2);
                    }
                }
                else if (0 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > -3)
                {
                    Instructions.text = "Press E to put bananas in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (convoNum == 0) { smoothie0[2] = 1; }
                        if (convoNum == 1) { smoothie1[2] = 1; }
                        if (convoNum == 2) { smoothie2[2] = 1; }
                        audioSource.PlayOneShot(pickupClip, 2);
                    }
                }
                else if (-3 > player.GetComponent<Transform>().position.x)
                {
                    Instructions.text = "Press E to put milk in blender";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (convoNum == 0) { smoothie0[3] = 1; }
                        if (convoNum == 1) { smoothie1[3] = 1; }
                        if (convoNum == 2) { smoothie2[3] = 1; }
                        audioSource.PlayOneShot(pickupClip, 2);
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

        if (canGiveSmoothie) { 
            if (Mathf.Abs(player.GetComponent<Transform>().eulerAngles.y) > 280 || 20 > Mathf.Abs(player.GetComponent<Transform>().eulerAngles.y))
            {
                if (1 > player.GetComponent<Transform>().position.x && player.GetComponent<Transform>().position.x > -1)
                {
                    Instructions.text = "Press E to give drink to customer";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        sbDrink.SetActive(false);
                        sDrink.SetActive(false);
                        bDrink.SetActive(false);
                        eDrink.SetActive(false);
                        if (convoNum == 0)
                        {
                            friend.GetComponent<friend>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothie0, order0))
                            {
                                friend.GetComponent<friend>().result = "Convo0Good";
                            }
                            else
                            {
                                friend.GetComponent<friend>().result = "Convo0Bad";
                            }
                        }
                        if (convoNum == 1)
                        {
                            customer1.GetComponent<customer1>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothie1, order1))
                            {
                                customer1.GetComponent<customer1>().result = "Convo1Good";
                            }
                            else
                            {
                                customer1.GetComponent<customer1>().result = "Convo1Bad";
                            }
                        }
                        if (convoNum == 2)
                        {
                            customer2.GetComponent<customer2>().anim.SetBool("GotDrink", true);
                            if (CompareArray(smoothie2, order2))
                            {
                                customer2.GetComponent<customer2>().result = "Convo2Good";
                            }
                            else
                            {
                                customer2.GetComponent<customer2>().result = "Convo2Bad";
                            }
                        }
                        canGiveSmoothie = false;
                        canMakeSmoothie = false;
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
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        ContinueStory();
    }

    private bool CompareArray(int[] a1, int[] a2) {
        bool equals = true;
        for (int i = 0; i < 4; i++) {
            if (a1[i] != a2[i]) { equals = false; }
        }
        return equals;
    }

    //wait for blender to run and then create drink
    IEnumerator WaitForBlender()
    {
        yield return new WaitForSeconds(4);
        canGiveSmoothie = true;
        if (convoNum == 0) {
            if (smoothie0[1] == 1 && smoothie0[2] == 1)
            {
                sbDrink.SetActive(true);
            }
            else if (smoothie0[1] == 1)
            {
                sDrink.SetActive(true);
            }
            else if (smoothie0[2] == 1)
            {
                bDrink.SetActive(true);
            }
            else {
                eDrink.SetActive(true);
            }
        }

        if (convoNum == 1)
        {
            if (smoothie1[1] == 1 && smoothie1[2] == 1)
            {
                sbDrink.SetActive(true);
            }
            else if (smoothie1[1] == 1)
            {
                sDrink.SetActive(true);
            }
            else if (smoothie1[2] == 1)
            {
                bDrink.SetActive(true);
            }
            else
            {
                eDrink.SetActive(true);
            }
        }

        if (convoNum == 2)
        {
            if (smoothie2[1] == 1 && smoothie2[2] == 1)
            {
                sbDrink.SetActive(true);
            }
            else if (smoothie2[1] == 1)
            {
                sDrink.SetActive(true);
            }
            else if (smoothie2[2] == 1)
            {
                bDrink.SetActive(true);
            }
            else
            {
                eDrink.SetActive(true);
            }
        }
    }
}
