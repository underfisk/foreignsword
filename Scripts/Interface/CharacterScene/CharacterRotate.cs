using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotate : MonoBehaviour
{
    /// <summary>
    /// Script used to rotate the character when we drag
    /// </summary>
	protected void OnMouseDrag()
    {
        float rotY = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, rotY,Space.World);
    }

}
