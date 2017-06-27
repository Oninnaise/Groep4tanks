using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Zoekt een Transform en stopt het in playerTransform
    
    void Update() //Update wordt elke frame geroepen
    {
        if (playerTransform != null) // Als de playerTransform niet leeg is
        {
            transform.position = playerTransform.position + new Vector3(0, 10, 1); // Zet de position van de camera bij de PlayerTransform + Vector3(x,y,z) aanpassingen
        }
    }

    public void setTarget(Transform target) // Set Target
    {
        playerTransform = target;
    }
}