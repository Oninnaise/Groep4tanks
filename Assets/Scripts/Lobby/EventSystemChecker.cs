using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype.NetworkLobby
{
    public class EventSystemChecker : MonoBehaviour
    {

        void Awake()
        {
            if (!FindObjectOfType<EventSystem>())
            {
                GameObject obj = new GameObject("EventSystem"); // De GameObject wordt een EventSystem
                obj.AddComponent<EventSystem>(); // Initialiseer een EventSystem
                obj.AddComponent<StandaloneInputModule>().forceModuleActive = true; // Zet EventSystem aan
            }
        }
    }
}