using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Playercontroller : MonoBehaviour
{
public float speed;
public float groundDist;

public LayerMask terrainLayer;
public Rigidbody rb;
public SpriteRenderer sr;
// Start is called before the first frame update
void Start()
    {

    rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Vector3 castpro = transform.position;
        castpro.y += 1;
        if (Physics.Raycast(castpro,-transform.up, out hit, Mathf.Infinity,terrainLayer))
        {
            if (hit.collider!= null)
            {
                Vector3 movepos = transform.position;
                movepos.y = hit.point.y + groundDist;
                transform.position = movepos;
            }
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movedir = new Vector3(x, 0, y);
        rb.velocity = movedir * speed;
        if (x != 0 && x > 0)
        {
            sr.flipX = true;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
        }
       
    }
}
