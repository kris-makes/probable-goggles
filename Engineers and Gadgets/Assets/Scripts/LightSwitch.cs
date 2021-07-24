using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public bool isOn;
    public Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: kell ez?
    }

    // TODO: paraméterekkel?
    public void TurnLights()
    {
        for (int i = 0; i < lights.Length; ++i)
        {
            lights[i].enabled = !isOn;
        }
        isOn = !isOn;
    }

    public void RandomColor()
    {
        for (int i = 0; i < lights.Length; ++i)
        {
            lights[i].color = new Color(Random.Range(0f, 255f), Random.Range(0f, 255f), Random.Range(0f, 255f), 255f);
        }
    }
}
