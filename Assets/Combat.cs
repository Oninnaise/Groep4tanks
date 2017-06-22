using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour
{
    public const int maxHealth = 100; // Maak een int met constante waarde (moet aangepast worden als je een maxHealth powerup wilt) van 100
    private NetworkStartPosition[] spawnPoints; // Maak een array NetworkStartPositions (spawn points)
    void Start() // Start wordt aangeroepen zodra de script aangezet wordt
    {
        if (isLocalPlayer) // isLocalPlayer is een Unity netwerk boolean waarmee je kunt aangeven of iets van een speler is of niet. De Player prefab heeft een Network Identity script met Local Player Authority aanstaan, dat zit met elkaar verbonden
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>(); // Unity doorzoekt alle gameObjects met de NetworkStartPosition script
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
            health = maxHealth;

            RpcRespawn();
            from.GetComponent<PlayerMove>().AddScore(); // Geef score aan de winnaar
        }
        if (health > 100) // Zorgt ervoor dat health niet boven 100 kan komen d.m.v powerups
        {
            health = maxHealth; 
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