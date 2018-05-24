using HelperPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraRaycaster;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorController : MonoBehaviour
{
    /// <summary>
    /// Based on : https://docs.unity3d.com/ScriptReference/Cursor.SetCursor.html
    /// </summary>

    ///<sumary>
    /// Cursor textures
    /// </sumary>
    [SerializeField] Texture2D normal_cursor,
                               attack_cursor,
                               talk_cursor;

    /// <summary>
    /// Cursor mode native from Unity
    /// </summary>
    public CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// Hotspot position native from Unity (for example, left click and it just works on the hotspot)
    /// </summary>
    public Vector2 hotSpot = Vector2.zero;

    /// <summary>
    /// CameraRaycaster object
    /// </summary>
    public CameraRaycaster cameraRaycaster;

    [SerializeField] const int walkableLayerID = 8;
    [SerializeField] const int enemyLayerID = 9;
    [SerializeField] const int stiffLayerID = 10;
    [SerializeField] const int raystopLayerID = -1;
    [SerializeField] const int npcLayerID = 11;

    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += CursorHandler;
    }


    /// <summary>
    /// Method linked to onLayerChange event at CameraRayCaster (onLayerChange is based on the delegated OnLayerChange)
    /// </summary>
    /// <param name="newLayer"></param>
    void CursorHandler(int newLayer)
    {
        switch (newLayer)
        {
            case walkableLayerID:
                Cursor.SetCursor(normal_cursor, hotSpot, cursorMode); //default one
                break;
            case enemyLayerID:
                Cursor.SetCursor(attack_cursor, hotSpot, cursorMode); //default one
                break;
            case raystopLayerID:
                Cursor.SetCursor(normal_cursor, hotSpot, cursorMode); //default too
                break;
            case npcLayerID:
                Cursor.SetCursor(talk_cursor, hotSpot, cursorMode);
                break;
            case stiffLayerID:
                ILog.toUnity("Dead bodies here");
                break;
            default:
                Cursor.SetCursor(normal_cursor, hotSpot, cursorMode); //default one
                return;
        }
    }

}