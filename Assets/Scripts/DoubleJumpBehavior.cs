using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpBehavior : MonoBehaviour
{

    private bool _canDoubleJump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerMovementBehavior _pmb = GameObject.Find("Player").GetComponent<PlayerMovementBehavior>();

        if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
        {
            _pmb.DoubleJump();
            _canDoubleJump = false;
            // Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _canDoubleJump = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _canDoubleJump = false;
        }
    }
}
