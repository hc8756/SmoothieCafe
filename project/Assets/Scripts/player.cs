using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
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
        
    }
}
