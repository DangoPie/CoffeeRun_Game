using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private float dirX = 0f;
    [SerializeField] public float jumpForce = 7;
    [SerializeField] public float moveSpeed = 8;
    [SerializeField] private LayerMask jumpableGround;
    Animator anim;
    SpriteRenderer sprite;
    IInteractable currentInteractable;
    private enum MovementState {idle,running,jumping,falling };
    [SerializeField] private AudioSource jumpSoundEffect;
  
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent <Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForHoles();
        }
    }

    public void CheckForHoles()
    {
        GameObject[] holes = GameObject.FindGameObjectsWithTag("hole");

        foreach (GameObject hole in holes)
        {
            float dist = Vector3.Distance(transform.position, hole.transform.position);
            if (dist < 4f)
            {
                hole holecomp = hole.GetComponent<hole>();
                holecomp.ToggleVisibility(true);
            }
        }
    }

    private void FixedUpdate()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(horizontalMove, 0);
        UpdateAnimatorState(horizontalMove);
    }

    void UpdateAnimatorState(float horizontalMove)
    {
        MovementState state;
        if (horizontalMove > 0.1f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (horizontalMove < -0.1f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else 
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
       return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentInteractable = collision.GetComponent<IInteractable>();
    }
}
