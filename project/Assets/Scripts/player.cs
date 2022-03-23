using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), 5 * Time.deltaTime);
            anim.SetBool("Moving", true);
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x<5)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), 5 * Time.deltaTime);
            transform.Translate(Vector3.right * Time.deltaTime * 4, Space.World);
            anim.SetBool("Moving", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);
            anim.SetBool("Moving", true);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x>-5)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), 5 * Time.deltaTime);
            transform.Translate(Vector3.left * Time.deltaTime * 4, Space.World);
            anim.SetBool("Moving", true);
        }
        if (!Input.anyKey)
        {
            anim.SetBool("Moving", false);
        }
    }
}
