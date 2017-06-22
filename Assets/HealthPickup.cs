using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tijdelijke Health Pickup, moet omgezet worden naar een Strategy Pattern
public class HealthPickup : MonoBehaviour
{
    float speed = 25.2f; //Roteer snelheid. F moet aan het eind van een float gezet worden bij een decimaal (20.2F) anders wordt het als een double gezien
    void OnTriggerEnter(Collider other) //Als je de triggerzone in gaat, wat het raakt is Collider, wordt doorgestuurd naar other
    {
        if (other.gameObject.tag == "Player") //Als het object wat de trigger geraakt heeft een tag "Player" heeft
        {
            var combat = other.GetComponent<Combat>(); // combat is een variable met de script "Combat" van de gameObject
            combat.TakeDamage(-40, null); // Roep de TakeDamage functie uit met -40 damage van de "Combat" script
            Destroy(this.gameObject); // Verwijder de gameObject ---- TODO respawn functie
        }
    }

    void Update() // Wordt elke frame geroepen
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime); // Het object laten roteren
    }
}