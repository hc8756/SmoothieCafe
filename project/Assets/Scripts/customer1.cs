using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customer1 : MonoBehaviour
{
    public GameObject managerObject;
    private GameObject model;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        model = this.gameObject.transform.GetChild(0).gameObject;
        anim = model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //this makes the customer come in
        if (anim.GetBool("entering"))
        {
            if (transform.position.z > 0)
            {
                transform.position -= new Vector3(0, 0, Time.deltaTime);
            }
            else{
                anim.SetBool("entering", false);
                anim.SetBool("standing", true);
            }
        }
        //this makes the friend leave 
        if (anim.GetBool("standing"))
        {
            manager.convoNum = 1;
            manager.dialogueOpen = true;
        }
        //this makes the customer leave 
        else if (anim.GetBool("leaving"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), 5 * Time.deltaTime);
            if (transform.rotation.y < -0.98 && transform.position.z < 7)
            {
                transform.position += new Vector3(0, 0, Time.deltaTime);
            }
        }
    }


    
}
