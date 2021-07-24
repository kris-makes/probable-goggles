using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    // esetleg ez meg jelenhetne a tárgy neve alatt, hogy
    // lehessen látni, mi fog történni, ha megnyomod az interakciós gombot
    // bár lehet, nem jó, mert elveszi a meglepetést
    public string[] whatToDo;
    public KeyCode[] keyCodes;
    public UnityEvent[] actions;    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
