using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour
{

    public GameObject upButton, leftButton, rightButton, downButton, fireButton, Score; //Aangeven om welke gameObjects het gaat. In deze geval de Android controls

    public static GuiController singleton = null;

    void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // Als de game op een desktop opgestart wordt, laat de android controls niet zien
        {
            GameObject tempObject = GameObject.Find("Controls"); // Vind de gameObject "Controls" (is een child van Canvas)
            tempObject.SetActive(false); // SetActive(false), dus deactivate.

        }
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }
}