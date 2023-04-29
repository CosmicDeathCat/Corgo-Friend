using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorgiPlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    public float jumpForce = 5;
    public float runSpeed = 5;
    public int jumpsAllowed = 2;
    public float checkRadius = 0.5f;
    public LayerMask whatIsGround;
    public Vector2 movement;
    private bool isRunning;
    private PlayerInputActions playerInput;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private int currentJumps;
    private bool isSniffing;
    [SerializeField]
    private List<GameObject>collidedObjects;
    private void Awake()
    {
        playerInput = new PlayerInputActions();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInput.Player.Move.performed += Move_performed;
        playerInput.Player.Move.canceled += Move_canceled;
        playerInput.Player.Run.performed += Run_performed;
        playerInput.Player.Run.canceled += Run_canceled;
        playerInput.Player.Jump.performed += Jump_performed;
        playerInput.Player.Sniff.performed += Sniff_performed;
        playerInput.Player.Sniff.canceled += Sniff_canceled;
        playerInput.Player.Sit.performed += Sit_performed;
        playerInput.Player.Bark.performed += Bark_performed;
        playerInput.Enable();

    }



    private void OnDisable()
    {
        playerInput.Player.Move.performed -= Move_performed;
        playerInput.Player.Move.canceled -= Move_canceled;
        playerInput.Player.Run.performed -= Run_performed;
        playerInput.Player.Run.canceled -= Run_canceled;
        playerInput.Player.Jump.performed -= Jump_performed;
        playerInput.Player.Sniff.performed -= Sniff_performed;
        playerInput.Player.Sniff.canceled -= Sniff_canceled;
        playerInput.Player.Sit.performed -= Sit_performed;
        playerInput.Player.Bark.performed -= Bark_performed;
        playerInput.Disable();
    }

    private void Sit_performed(InputAction.CallbackContext input)
    {
        anim.SetTrigger("Sit");
    }

    private void Sniff_canceled(InputAction.CallbackContext obj)
    {
        isSniffing = false;
        anim.SetBool("isSniffing", isSniffing);

    }
    private void Sniff_performed(InputAction.CallbackContext input)
    {
        isSniffing = true;
        anim.SetBool("isSniffing", isSniffing);
    }

    private void Bark_performed(InputAction.CallbackContext obj)
    {
        anim.SetTrigger("Bark");
        for (int i = 0; i < collidedObjects.Count; i++)
        {
            var animal = collidedObjects[i].GetComponent<AnimalController>();
            if (animal != null)
            {
                animal.Move(new Vector2(animal.transform.position.x + 1, animal.transform.position.y));
            }
        }
    }

    private void Jump_performed(InputAction.CallbackContext input)
    {
        if (isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumps++;


        }
        else if (currentJumps < jumpsAllowed)
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumps++;
        }
    }

    private void Run_performed(InputAction.CallbackContext input)
    {
        isRunning = true;
        anim.SetBool("isRunning", isRunning);
    }
    private void Run_canceled(InputAction.CallbackContext obj)
    {
        isRunning = false;
        anim.SetBool("isRunning", isRunning);
    }

    private void Move_performed(InputAction.CallbackContext input)
    {
        movement = input.ReadValue<Vector2>();
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        anim.SetBool("isWalking", true);
    }
    private void Move_canceled(InputAction.CallbackContext input)
    {
        movement = Vector2.zero;
        anim.SetBool("isWalking", false);
    }

    public void FixedUpdate()
    {
        rb.velocity = isRunning ? new Vector2(movement.x * runSpeed, rb.velocity.y) : new Vector2(movement.x * moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround);
        if (isGrounded)
        {
            currentJumps = 1;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (isSniffing)
        {
            var mushroom = collision.GetComponent<MushroomController>();
            if(mushroom != null)
            {
                mushroom.Activate();
                isSniffing = false;
                anim.SetBool("isSniffing", isSniffing);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collidedObjects.Contains(collision.gameObject))
        {
            collidedObjects.Add(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collidedObjects.Contains(collision.gameObject))
        {
            collidedObjects.Remove(collision.gameObject);
        }
    }
}
