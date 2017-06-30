using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Combat : NetworkBehaviour
{

    public const int maxHealth = 100; // Maak een int met constante waarde (moet aangepast worden als je een maxHealth powerup wilt) van 100
    public float explosionRadius = 100.0f;
    public float explosionPower = 100.0f;
    public bool destroyOnDeath;
    public float timer;
    public int waitingTime = 3;
    public Vector3[] positions;
    [SyncVar]
    public bool waitingrespawn = false;
    private NetworkStartPosition[] spawnPoints; // Maak een array NetworkStartPositions (spawn points)
    void Start() // Start wordt aangeroepen zodra de script aangezet wordt
    {
        if (isLocalPlayer) // isLocalPlayer is een Unity netwerk boolean waarmee je kunt aangeven of iets van een speler is of niet. De Player prefab heeft een Network Identity script met Local Player Authority aanstaan, dat zit met elkaar verbonden
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>(); // Unity doorzoekt alle gameObjects met de NetworkStartPosition script
            positions = new Vector3[transform.childCount];
            var count = 0;

            foreach (Transform child in transform)
            {
                positions[count] = child.position;
                count++;
            }
        }
    }

    void Update()
    {
        if (waitingrespawn == true)
        {
            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                waitingrespawn = false;



                timer = 0;
            }
        }
    }
    [SyncVar] // Dit wordt met iedereen gesynct, zorgt ervoor dat iedereen dezelfde waarde ziet
    public int health = maxHealth;

    public void TakeDamage(int amount, Transform from) 
    {
        if (!isServer) // Zorgt ervoor dat de script alleen op de server geroepen wordt (wordt dus 1x gedaan en kan niet met een packet tracer extra malen doorgestuurd worden)
            return;

        health -= amount;
        if (health <= 0)
        {

            if (destroyOnDeath)
            {
                CmdEnvironmentDestroy();
            }
            else
            {

                from.GetComponent<PlayerMove>().AddScore(); // Geef score aan de winnaar
               // waitingrespawn = true;
                //RpcExplode();
                //RpcReset();
                RpcRespawn();
                health = maxHealth;

            }
        }
        if (health > 100) // Zorgt ervoor dat health niet boven 100 kan komen d.m.v powerups
        {
            health = maxHealth; 
        }
    }

    [Command]
    void CmdEnvironmentDestroy()
    {
        RpcEnvironmentExplode();
    }

    [Command]
    void CmdOnRespawn()
    {
        RpcExplode();
    }

    [Command]
    void CmdOnReset()
    {
        RpcReset();
    }

    [ClientRpc]
    void RpcEnvironmentExplode()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Rigidbody>().isKinematic = false;
            child.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, explosionRadius);
            GetComponent<Collider>().isTrigger = true;
        }
    }

    [ClientRpc]
    void RpcReset()
    {
        if (isLocalPlayer)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshCollider>().enabled = false;
                child.GetComponent<Rigidbody>().isKinematic = true;
            }
            var count = 0;
            foreach (Transform child in transform)
            {
                child.position = positions[count];
                count++;
            }
        }
    }
    [ClientRpc]
    void RpcExplode()
    {
        if (isLocalPlayer)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshCollider>().enabled = true;
                child.GetComponent<Rigidbody>().isKinematic = false;
                child.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, explosionRadius);

            }
        }
    }

    [ClientRpc] // Zorgt ervoor dat RpcRespawn() op de server gecallt wordt maar op alle client uitgevoerd wordt
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero; // Speler spawnpoint is Vector3(0,0,0)

            if (spawnPoints != null && spawnPoints.Length > 0) // Als er spawnpoints zijn
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position; // Zet de spawnpoint op de positie van een van de spawnpoints (Pos1, Pos2, Pos3, Pos4 in Unity)
            }
            
            transform.position = spawnPoint; // Speler position is de position van de spawnpoint
        }
    }

}

