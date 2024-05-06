using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementBehavior : MonoBehaviour
{
    public float jumpForce; // Puissance du saut
    public float moveSpeed; // Vitesse de déplacement vers la droite
    public bool test;

    private Rigidbody2D _rb;
    private bool _isGrounded;

    // Rotation du personnage
    public GameObject playerSprite; // Sprite du personnage
    public float rotationAmount; // Angle de rotation en degrés
    public float rotationSpeed; // Vitesse de rotation en degrés par seconde
    private bool _isRotating = false; // Indique si le personnage est en cours de rotation
    // Rotation du personnage

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Saut
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            // On tourne le personnage de 90° vers la gauche (avec le tag PlayerSprite)
            if (!_isRotating) 
            {
                StartCoroutine(RotatePlayerSprite());
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
        }

        // Debug la speed du joueur
        // Debug.Log("Speed : " + _rb.velocity.x);
        // if (_rb.velocity.x <= 0f)
        // {
        //     _rb.velocity = new Vector2(-1, _rb.velocity.y);
        // }

    }

    private void FixedUpdate()
    {
        // Déplacement automatique vers la droite
        if (!test)
            _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Vérifie si la collision est avec un objet portant le tag "Ground"
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         _isGrounded = true; // Le cube est au sol
    //     }
    //     Debug.Log("Collision with " + collision.gameObject.name);
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D ct = collision.GetContact(0);
        if (ct.normal.y == 0)
        {
            Debug.Log("Collision with " + collision.gameObject.name);
            
            if (collision.gameObject.CompareTag("Platform"))
            {
                GameManager.Instance.PlayerDie();
            }
        }
        if (ct.normal.x == 0 && ct.normal.y > 0)
            _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Vérifie si le cube quitte le sol
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false; // Le cube n'est plus au sol
        }
    }
    
    IEnumerator RotatePlayerSprite()
    {
        _isRotating = true;

        Quaternion startRotation = playerSprite.transform.rotation; // Rotation initiale
        Quaternion endRotation = playerSprite.transform.rotation * Quaternion.Euler(0, 0, rotationAmount); // Rotation finale

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (rotationSpeed / 90f); // Ajuster la vitesse de rotation en fonction de la rotation souhaitée
            playerSprite.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        // Si la rotation du personnage n'est pas parmis la liste suivante, alors la remettre à la valeur la plus proche.
        List<float> rotationList = new List<float> { 0, 90, -90, 180, -180, 270, -270, 360, -360 };
        float currentAngle = playerSprite.transform.rotation.eulerAngles.z;
        // Si l'angle n'est pas dans le tableau rotationList, alors on fait rien. Sinon on tourne le personnage.
        if (!rotationList.Contains(currentAngle))
        {
            if (currentAngle > 0 && currentAngle < 90)
                currentAngle = 90;
            else if (currentAngle < 0 && currentAngle > -90)
                currentAngle = -90;
            else if (currentAngle > 90 && currentAngle < 180)
                currentAngle = 180;
            else if (currentAngle < -90 && currentAngle > -180)
                currentAngle = -180;
            else if (currentAngle > 180 && currentAngle < 270)
                currentAngle = 270;
            else if (currentAngle < -180 && currentAngle > -270)
                currentAngle = -270;
            else if (currentAngle > 270 && currentAngle < 360)
                currentAngle = 360;
            else if (currentAngle < -270 && currentAngle > -360)
                currentAngle = -360;
        }

        playerSprite.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        _isRotating = false;
    }

    public void DoubleJump ()
    {
        StartCoroutine(RotatePlayerSprite());
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    public void setIsGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }
}
