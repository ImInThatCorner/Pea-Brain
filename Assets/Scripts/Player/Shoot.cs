using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag.Equals("Player"))
                    Fire(hit);
            }
        }
    }

    void Fire(RaycastHit hit)
    {
        RaycastHit playerHit;
        Vector3 playerPosAdjusted = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 hitPosAdjusted = new Vector3(hit.point.x, 1f, hit.point.z);

        //Debug.Log(hit.point);

        Ray ray = new Ray(playerPosAdjusted, hitPosAdjusted-playerPosAdjusted);

        if (Physics.Raycast(ray, out playerHit, (hitPosAdjusted - playerPosAdjusted).magnitude))
        {
            //Debug.Log("hit pos " + playerHit.point);
            Debug.Log(playerHit.transform.tag);
        }
    }
}
