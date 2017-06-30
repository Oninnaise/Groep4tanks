using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;

        protected bool isDisplayed = true;
        protected Image panelImage;

        void Start()
        {
            panelImage = GetComponent<Image>();
        }


        void Update()
        {
            if (!isInGame) // Als de speler niet in de spel is
                return; // Niets doen

            if (Input.GetKeyDown(KeyCode.Escape)) // Als er op escape gedrukt is
            {
                ToggleVisibility(!isDisplayed); // Toggle het zien van de top panel
            }

        }

        public void ToggleVisibility(bool visible) // Toggelen van de top panel
        {
            isDisplayed = visible;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(isDisplayed);
            }

            if (panelImage != null)
            {
                panelImage.enabled = isDisplayed;
            }
        }
    }
}