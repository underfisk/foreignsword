using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Button openMenu;
    [SerializeField] Button zoomInBtn;
    [SerializeField] Button zoomOutBtn;
    [SerializeField] int max_zoom = 10, min_zoom = 5;
    private Camera localData;

    /// <summary>
    /// Initializes the button listeners and get camera component
    /// </summary>
    protected void Start()
    {
        localData = gameObject.GetComponent<Camera>();
        openMenu.onClick.AddListener(() => ShowMenu());
        zoomInBtn.onClick.AddListener(() => ZoomIn());
        zoomOutBtn.onClick.AddListener(() => ZoomOut());
    }

    /// <summary>
    /// After physics update we update the minimap camera to player position
    /// </summary>
    protected void FixedUpdate()
    {
        Vector3 newPos = GameObject.FindWithTag("Player").transform.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }

    /// <summary>
    /// Reduces the zoom on minimap, in camera it goes higher
    /// </summary>
    public void ZoomOut()
    {
        if (localData.orthographicSize < max_zoom)
            localData.orthographicSize += 1;
    }

    /// <summary>
    /// Increase the zoom on minimap, in camera it goes lower
    /// </summary>
    public void ZoomIn()
    {
        if (localData.orthographicSize > min_zoom)
            localData.orthographicSize -= 1;
    }

    /// <summary>
    /// Menu button handler at minimap
    /// </summary>
    public void ShowMenu()
    {
        if (!GUI_Manager.instance.menuWindow.activeSelf)
            GUI_Manager.instance.menuWindow.SetActive(true);
        else
            GUI_Manager.instance.menuWindow.SetActive(false);
    }
}
