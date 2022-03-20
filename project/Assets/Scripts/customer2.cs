using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class customer2 : MonoBehaviour
{
    public GameObject managerObject;
    public Animator anim;
    public string result;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
                StartCoroutine(LoadEnding());
            }
        }
    }

    IEnumerator CharacterFinishesDrinking()
    {
        yield return new WaitForSeconds(4);
        managerObject.GetComponent<manager>().LoadNewStory(result);
        anim.SetBool("GotDrink", false);
        anim.SetBool("Talking", true);
    }

    IEnumerator LoadEnding()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Ending");
    }
}
