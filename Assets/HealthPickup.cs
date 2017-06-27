using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
//Tijdelijke Health Pickup, moet omgezet worden naar een Strategy Pattern
public class HealthPickup : NetworkBehaviour
{
    public MeshRenderer render;
    public BoxCollider collider;
    [SyncVar]
    bool used = false;
    [SyncVar]
    public float timeleft = 5;
    float speed = 25.2f; //Roteer snelheid. F moet aan het eind van een float gezet worden bij een decimaal (20.2F) anders wordt het als een double gezien
    void OnTriggerEnter(Collider other) //Als je de triggerzone in gaat, wat het raakt is Collider, wordt doorgestuurd naar other
    {
        if (used == false)
        {
            if (other.gameObject.tag == "Player") //Als het object wat de trigger geraakt heeft een tag "Player" heeft
            {
                var combat = other.GetComponent<Combat>(); // combat is een variable met de script "Combat" van de gameObject
                var move = other.GetComponent<PlayerMove>();
                //combat.TakeDamage(-40, null); // Roep de TakeDamage functie uit met -40 damage van de "Combat" script
                move.RapidFire(5);
                CmdOnUsed();
            }
        }
    }
    [Command]
    void CmdOnUsed()
    {
        RpcDelete();
    }

    [Command]
    void CmdOnRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    void RpcDelete()
    {
        used = true;
    }
    void Update() // Wordt elke frame geroepen
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime); // Het object laten roteren
        if (this.used == true)
        {
            collider.enabled = false;
            render.enabled = false;
            timeleft -= Time.deltaTime;
            if (timeleft < 0)
            {
                CmdOnRespawn();
            }
        }
    }
    [ClientRpc]
    void RpcRespawn()
    {
        used = false;
        collider.enabled = true;
        render.enabled = true;
        timeleft = Random.Range(10.0f, 20.0f);

    }
}