using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 4f;
    public float rotationSpeed = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Forward
        transform.Translate(speed * Vector3.forward * Time.deltaTime);

        //Turning
        float rotation;
        if(this.gameObject.name == "P2"){
            rotation = Input.GetAxis("Horizontal2") * rotationSpeed;
        }else{
            rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        }
        rotation *= Time.deltaTime;

        transform.Rotate(0,rotation,0);
    }

    void OnCollisionEnter(Collision collision){
        Destroy(this.gameObject);
    }
}
