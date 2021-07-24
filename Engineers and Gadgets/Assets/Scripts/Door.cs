using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public int kickForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: lehet, hogy nem kell
    }

    public void ChangeState()
    {
        // TODO: valami jobb kéne
        gameObject.transform.rotation = isOpen ? Quaternion.Euler(0f, -40f, 0f) : Quaternion.Euler(0f, 80f, 0f);
        isOpen = !isOpen;
    }

    public void Kick()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * kickForce, ForceMode.Impulse);
    }
}
