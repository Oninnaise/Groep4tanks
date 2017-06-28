using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    GUIStyle healthStyle;
    GUIStyle backStyle;
    Combat combat; //combat is een variable die de script "Combat" van een gameObject selecteerd
    public bool showHP;


    void Awake()
    {
        combat = GetComponent<Combat>();  // combat is een variable met de script "Combat" van de gameObject
    }

    void OnGUI()
    { if (combat.health <= 0)
            showHP = false; 
        if (showHP)
        {
            InitStyles();
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position); // Set locatie van de healthbars

            // Achtergrond tekenen
            GUI.color = Color.grey;
            GUI.backgroundColor = Color.grey;
            GUI.Box(new Rect(pos.x - 26, Screen.height - pos.y + 20, Combat.maxHealth / 2, 7), ".", backStyle);

            // Healthbar tekenen
            GUI.color = Color.green;
            GUI.backgroundColor = Color.green;
            GUI.Box(new Rect(pos.x - 25, Screen.height - pos.y + 21, combat.health / 2, 5), ".", healthStyle);
        }


    }

    void InitStyles()
    {
        if (healthStyle == null)
        {
            healthStyle = new GUIStyle(GUI.skin.box); // Draw een box
            healthStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1.0f)); // Geef het een kleur
        }

        if (backStyle == null)
        {
            backStyle = new GUIStyle(GUI.skin.box); // Draw een box
            backStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1.0f)); // Geef het een kleur
        }
    }

    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}