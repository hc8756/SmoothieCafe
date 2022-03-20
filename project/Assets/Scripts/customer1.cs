using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customer1 : MonoBehaviour
{
    public GameObject managerObject;
    public Animator anim;
    public string result;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.convoNum == 1)
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
                    managerObject.GetComponent<manager>().LoadNewStory("Convo1");
                    anim.SetBool("Talking", true);
                }
            }
            else {
                manager.dialogueOpen = anim.GetBool("Talking"); }

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
                    manager.convoNum = 2;
                }
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

}
