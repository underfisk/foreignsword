using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //player or the target itself
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 15;
    // the height we want the camera to be above the target
    public float height = 5;
    // How much we 
    public float heightDamping = 3;
    public float rotationDamping = 3;

    protected virtual void Awake()
    {
        if (target == null)
            target = GameObject.FindWithTag("Player").gameObject.transform;

        ChasePlayer();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ChasePlayer();
    }

    protected virtual void ChasePlayer()
    {
        if (target) //we got something to target
        {

            float wantedHeight = target.position.y + height;
            float currentHeight = transform.position.y;


            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target

            Vector3 pos = target.position;
            pos -= Vector3.forward * distance;
            pos.y = currentHeight;
            transform.position = pos;
            // Always look at the target
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// Shakes the camera during a X time with a X magnitude
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude)
    {
        Debug.Log($"Shaking the camera for {duration} seconds with magnitude {magnitude}");
        Vector3 originalPos = transform.parent.gameObject.transform.position;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f ,1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.parent.gameObject.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; //wait end of the frame
        }

        //reset pos now
        transform.parent.gameObject.transform.localPosition = transform.position;
    }
}
