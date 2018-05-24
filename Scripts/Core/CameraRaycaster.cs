using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(RaycastHit))]
public class CameraRaycaster : MonoBehaviour
{
    /// <summary>
    /// Layer priorities Array by saving the layer id
    /// </summary>
    [SerializeField] int[] layerPriorities;

    /// <summary>
    /// Max raycast depth
    /// </summary>
    float maxRaycastDepth = 100f;

    /// <summary>
    /// Saves the last top priority layer id
    /// </summary>
    int topPriorityLayerLastFrame = -1;

    /// <summary>
    /// Delegates a new function to receive the new layer on cursor layer change
    /// </summary>
    /// <param name="newLayer"></param>
    public delegate void OnCursorLayerChange(int newLayer);

    /// <summary>
    /// Delegates a event for Layer Change function to notify the observers
    /// </summary>
    public event OnCursorLayerChange notifyLayerChangeObservers;

    /// <summary>
    /// Delegates a new function when the click is on a top priority layer
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="layer"></param>
    public delegate void OnClickPriorityLayer(RaycastHit hit, int layer);

    /// <summary>
    /// Delegates a event for PriorityClick to notify the mouse click observers
    /// </summary>
    public event OnClickPriorityLayer notifyMouseClickObservers;


    // Update is called once per frame
    private void Update ()
    {	
        //Check if the pointer is over a GUI element (gui gameobject)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            NotifyObserversIfLayerChanged(5);
            return;
        }

        // Raycast to max depth, every frame as things can move under mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

        RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
        if (!priorityHit.HasValue) // if hit no priority object
        {
            NotifyObserversIfLayerChanged(0); // broadcast default layer
            return;
        }

        // Notify delegates of layer change
        var layerHit = priorityHit.Value.collider.gameObject.layer;
        NotifyObserversIfLayerChanged(layerHit);

        // Notify delegates of highest priority game object under mouse when clicked
        if (Input.GetMouseButton(0))
        {
            notifyMouseClickObservers(priorityHit.Value, layerHit);
        }
    }

    /// <summary>
    /// Delegated function to notify if the layer change on new click
    /// </summary>
    /// <param name="newLayer"></param>
    protected void NotifyObserversIfLayerChanged(int newLayer)
    {
        if (newLayer != topPriorityLayerLastFrame)
        {
            topPriorityLayerLastFrame = newLayer;
            notifyLayerChangeObservers(newLayer);
        }
    }

    /// <summary>
    /// Function used to find if the click is on a top priority layer and if yes we return this hit
    /// RaycastHit? returns RaycastHit or allows null with ?
    /// </summary>
    /// <param name="raycastHits"></param>
    /// <returns></returns>
    protected RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits)
    {
        // Form list of layer numbers hit
        List<int> layersOfHitColliders = new List<int>();
        foreach (RaycastHit hit in raycastHits)
        {
            layersOfHitColliders.Add(hit.collider.gameObject.layer);
        }

        // Step through layers in order of priority looking for a gameobject with that layer
        foreach (int layer in layerPriorities)
        {
            foreach (RaycastHit hit in raycastHits)
            {
                if (hit.collider.gameObject.layer == layer)
                {
                    return hit; // stop looking
                }
            }
        }
        return null; // because cannot use GameObject? nullable
    }
}
