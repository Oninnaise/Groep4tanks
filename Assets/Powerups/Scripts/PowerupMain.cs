using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PowerupMain : NetworkBehaviour {

    public PickupType type;			//Type pickup
    public MeshRenderer render;
    public BoxCollider collider;
    [SyncVar]
    bool used = false;
    [SyncVar]
    public float timeleft = 5; // Respawn tijd
    float speed = 25.2f; //Roteer snelheid. F moet aan het eind van een float gezet worden bij een decimaal (20.2F) anders wordt het als een double gezien
    public enum PickupType
    {
        HP,
        RapidFire,
    }

    void OnTriggerEnter(Collider other) //Als je de triggerzone in gaat, wat het raakt is Collider, wordt doorgestuurd naar other
    {
        if (used == false)
        {
            if (other.gameObject.tag == "Player") //Als het object wat de trigger geraakt heeft een tag "Player" heeft
            {
                if (type == PickupType.HP)
                {
                    var combat = other.GetComponent<Combat>(); // combat is een variable met de script "Combat" van de gameObject
                    combat.TakeDamage(-40, null); // Roep de TakeDamage functie uit met -40 damage van de "Combat" script
                }
                if (type == PickupType.RapidFire)
                {
                    var move = other.GetComponent<PlayerMove>(); // move is een variablle met de script "PlayerMove" van de gameObject
                    move.RapidFire(5); // Roep de RapidFDire functie uit van de "PlayerMove" script voor 5 seconden
                }
                CmdOnUsed();
            }
        }
    }

    void Update() // Wordt elke frame geroepen
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime); // Het object laten roteren
        if (this.used == true)
        {
            collider.enabled = false; // Onzichtbaar maken
            render.enabled = false; // Onzichtbaar maken
            timeleft -= Time.deltaTime;
            if (timeleft < 0) // Timer voor respawn
            {
                CmdOnRespawn();
            }
        }
    }
    [Command]
    void CmdOnUsed()
    {
        RpcDelete();
    }
    [ClientRpc]
    void RpcDelete()
    {
        used = true;
    }
    [Command]
    void CmdOnRespawn()
    {
        RpcRespawn();
    }
    [ClientRpc]
    void RpcRespawn()
    {
        used = false;
        collider.enabled = true; // Zichtbaar maken
        render.enabled = true; // Zichtbaar maken
        timeleft = Random.Range(10.0f, 20.0f); // Respawn timer resetten

    }


}
