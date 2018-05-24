using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamage : MonoBehaviour
{

    [SerializeField] public GameObject PopupPrefab;
    /// <summary>
    /// Generates a popup damage above the gameobject sent
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="location"></param>
    /// <param name="canvas"></param>
    /// <param name="secondsToFade"></param>
    public IEnumerator Display(float dmg, GameObject target)
    {
        if (PopupPrefab == null) Debug.LogWarning("Please add a popup prefab in sword collider script");

        GameObject prefab = Instantiate(PopupPrefab);
        Debug.Log("Prefab text => " + prefab.GetComponent<Text>().text);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        //Object properties
        prefab.transform.SetParent(transform);
        prefab.transform.position = screenPosition;
        prefab.GetComponent<Text>().text = dmg.ToString();
        prefab.GetComponent<Text>().fontSize = 40;
        prefab.GetComponent<Text>().raycastTarget = false;

        float timeToDestroy = prefab.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;

        yield return new WaitForSeconds(timeToDestroy);

        Destroy(prefab);

    }
}
