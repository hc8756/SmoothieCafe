using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class friend : MonoBehaviour
{
    public GameObject managerObject;
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (manager.convoNum == 0)
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
                    anim.SetBool("Talking", true);
                }
            }

            else { manager.dialogueOpen = anim.GetBool("Talking"); }

            if (anim.GetBool("GotDrink"))
            {
                StartCoroutine(CharacterFinishesDrinking());
            }

            if (anim.GetBool("Leaving")) {  
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);
                if (transform.rotation.y <0.2 && transform.position.z < 20)
                {
                    transform.position += new Vector3(0, 0, 2* Time.deltaTime);
                }
                if (transform.position.z >= 20) {
                    manager.convoNum = 1;
                }
            }
        }
    }

    IEnumerator CharacterFinishesDrinking()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("GotDrink", false);
        anim.SetBool("Talking", true);
    }
}
