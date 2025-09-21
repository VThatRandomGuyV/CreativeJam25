using UnityEngine;

// hello :D

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigid { get; private set; } // the rigid body attached to the player

    public CircleCollider2D col; // the collider attached to the player

    public bool isFlipped; // whether the player is flipped or not

    PlayerStats stats; // reference to the player stats script

    Vector3 velocityVector; // the direction the plr is moving in. This is a unit vector so it only gives direction, not speed
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // gets the rigid body component
        stats = GetComponent<PlayerStats>(); // gets the player stats component
        isFlipped = false; // sets isFlipped to false
    }
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // position of mouse
        var plrPos = transform.position; // position of plr
        mousePos.z = plrPos.z; // make the z's equal because for some fucking reason we need to worry about z in a 2D environment
        velocityVector = (mousePos - plrPos).normalized; // turns it into a unit vector so movement doesnt change depending on how far mouse is from plr
    }

    void FixedUpdate()
    {
        rigid.linearVelocity = velocityVector * stats.speed; // sets the velocity, tweak speed in the properties of the script

        if (velocityVector.x < 0 && !isFlipped) // if moving left and not flipped
        {
            Flip(); // flip the sprite
        }
        else if (velocityVector.x > 0 && isFlipped) // if moving right and flipped
        {
            Flip(); // flip the sprite
        }
    }

    private void Flip()
    {
        isFlipped = !isFlipped; // toggle isFlipped
        Vector3 scale = transform.localScale; // get the local scale of the plr
        scale.x *= -1; // flip the x scale
        transform.localScale = scale; // set the local scale to the new flipped scale
    }
}

