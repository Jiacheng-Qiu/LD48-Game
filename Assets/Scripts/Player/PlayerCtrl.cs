using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator playerAnimator;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI sizeText;

    private float fixedSpeed; // Speed determined by size
    public float flexibleSpeed = 6f;
    public float jumpSpeed = 1200f;
    private float currentSize;
    public float speedConstant = 12f;

    // For checking if character has already touched ground
    public Transform groundCheck;
    public LayerMask GroundLayer;
    public bool isGround, isJump;
    private bool jumpPressed;
    void Start()
    {
        // Initial size = 0.8
        currentSize = 0.8f;
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void UpdateCanvas()
    {
        speedText.text = body.velocity.x.ToString("F1");
        sizeText.text = ((int)(currentSize * 100)).ToString() + "%";
    }

    private void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    private void UpdateSize()
    {
        // Check the moving of player, and change size
        currentSize -= body.velocity.x * 0.0001f;
        float size = Mathf.Clamp(currentSize * 2, 0.8f, 2f);
        // Change size of player and collider size
        transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        // Change fixed speed based on size
        fixedSpeed = currentSize * speedConstant;
        // Check if is on ground by checking overlap with a layer
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, GroundLayer);
        isJump = !isGround;

        Move();
        Jump();
        
        UpdateSize();
        
        UpdateCanvas();
        // Check Gameover, TODO record data, and go to gameover scene + add rock collide
        if (currentSize <= 0)
        {
            GameOver();
        }
    }

    void Move()
    {
        float hInput = Input.GetAxis("Horizontal");
        if (hInput != 0)
        {
            // TODO: change vector x comp next
            body.velocity = new Vector2(fixedSpeed + hInput * flexibleSpeed, body.velocity.y);
        } else
        {
            body.velocity = new Vector2(fixedSpeed, body.velocity.y);
        }
        body.AddForce(new Vector2(0, -50));
    }

    void Jump()
    {
        /*if (isGround)
        {
            // This is changing the animation of character into idle
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Falling", false);
        }
        else if (body.velocity.y > 0)
        {
            // This is changing the animation of character into jumping
            playerAnimator.SetBool("Jumping", true);
            playerAnimator.SetBool("Falling", false);
        }
        else
        {
            // This is changing the animation of character into falling
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Falling", true);
        }*/

        if (jumpPressed && isGround)
        {
            // Make jump verticle to the pipe by using tangent
            float verticleJump = Mathf.Tan(Mathf.Abs(transform.rotation.z));
            body.AddForce(new Vector2(verticleJump * jumpSpeed, jumpSpeed));
            // body.velocity = new Vector2(body.velocity.x + verticleJump * Time.deltaTime, jumpSpeed * Time.deltaTime);
            jumpPressed = false;
        }
    }

}
