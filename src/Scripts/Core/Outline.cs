// Draws an outline around the entity while the mouse hovers over it. The
// outline strength can be set in the entity's material.
// Note: requires a shader with "_OutlineColor" parameter.
// Note: we use the outline color alpha channel for visibility, which is easier
// than saving the default strenghts and settings strengths to 0.
using UnityEngine;

public class Outline : MonoBehaviour
{
    public Color passive = Color.green;
    public Color enemy = Color.red;

    void SetOutline(Color col)
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            foreach (var mat in r.materials)
                if (mat.HasProperty("_OutlineColor"))
                    mat.SetColor("_OutlineColor", col);

    }

    void Awake()
    {
        // disable outline by default once
        SetOutline(Color.clear);
    }

    /// <summary>
    /// We check for the gameobjects with tag neutral or npc and just outline green there
    /// </summary>
    void OnMouseEnter()
    {
        if (gameObject.tag == "Player")
            SetOutline(passive);
        else if (gameObject.tag == "Enemy")
            SetOutline(enemy);
        else if (gameObject.tag == "NPC")
            SetOutline(passive);
    }

    void OnMouseExit()
    {
        // disable outline
        SetOutline(Color.clear);
    }
}
