using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class customer2 : MonoBehaviour
{
    public GameObject managerObject;
    public Animator anim;
    public Image blackBox;
    private CanvasGroup blackBoxCG;

    void Start()
    {
        anim = GetComponent<Animator>();
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 0;
    }

    void Update()
    {
        
        if (manager.convoNum == 2)
        {
            if (anim.GetBool("Entering"))
            {
                if (transform.position.z > 4)
                {
                    transform.position -= new Vector3(0, 0, 2 * Time.deltaTime);
                }
                else
                {
                    anim.SetBool("Entering", false);
                    managerObject.GetComponent<manager>().LoadNewStory("Convo2");
                    anim.SetBool("Talking", true);
                }
            }
            else
            {
                manager.dialogueOpen = anim.GetBool("Talking");
            }

            if (anim.GetBool("GotDrink"))
            {
                StartCoroutine(CharacterFinishesDrinking());
            }
            if (anim.GetBool("Leaving"))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);
                if (transform.rotation.y < 0.2 && transform.position.z < 20)
                {
                    transform.position += new Vector3(0, 0, 2 * Time.deltaTime);
                }
                if (transform.position.z >= 20)
                {
                    manager.convoNum = -1;
                }
                StartCoroutine(WaitEnding());
            }
        }

        if (blackBoxCG.alpha >= 1) {
            SceneManager.LoadScene("Ending");
        }
    }

    IEnumerator CharacterFinishesDrinking()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("GotDrink", false);
        anim.SetBool("Talking", true);
    }

    IEnumerator WaitEnding()
    {
        yield return new WaitForSeconds(6);
        blackBoxCG.alpha += 0.3f * Time.deltaTime;
    }
}
