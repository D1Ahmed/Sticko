using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float sprintSpeed = 12f;
    public float jumpForce = 30f;
    public float gravityDown = 10f;
    public float gravityUP = 10f;
    [Header("Gun System")]
    public bool isArmed = false;
    public GameObject gunPivot;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        bool shifting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = shifting ? sprintSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);

        anim.SetFloat("Speed", Mathf.Abs(moveX));
        anim.SetBool("isSprinting", shifting && Mathf.Abs(moveX) > 0.1f);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("hasGun", isArmed);

        if (gunPivot != null && gunPivot.activeSelf != isArmed)
        {
            gunPivot.SetActive(isArmed);
        }

        if (isArmed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else if (mousePos.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);

            bool isBackpedaling = (transform.localScale.x > 0 && moveX < 0) ||
                                  (transform.localScale.x < 0 && moveX > 0);

            if (Mathf.Abs(moveX) > 0.1f)
            {
                anim.SetFloat("AnimDirection", isBackpedaling ? -1f : 1f);
            }
            else
            {
                anim.SetFloat("AnimDirection", 1f);
            }
        }
        else
        {
            if (moveX > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (moveX < 0) transform.localScale = new Vector3(-1, 1, 1);

            anim.SetFloat("AnimDirection", 1f);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        rb.gravityScale = (rb.linearVelocity.y < 0) ? gravityUP : gravityDown;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }
}
