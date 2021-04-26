using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator playerAnimator;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI sizeText;

    private float fixedSpeed; // Speed determined by size
    public float flexibleSpeed = 6f;
    public float jumpSpeed = 1200f;
    private float currentSize;
    public float speedConstant = 12f;
    private float startTime;
    // For checking if character has already touched ground
    public Transform groundCheck;
    public LayerMask GroundLayer;
    public bool isGround, isJump;
    private bool jumpPressed;
    void Start()
    {
        startTime = Time.time;
        // Initial size = 0.8
        currentSize = 0.8f;
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Randomly increase size by a bit
        switch (collision.gameObject.tag){
            case "Collective":
                currentSize += Random.Range(0.1f, 0.3f);
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                Destroy(gameObject);
                GameOver();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // On collision decrease size
        if (collision.gameObject.tag == "Enemy")
        {
            //TODO
            currentSize -= 0.15f;
        }
    }


    private void UpdateCanvas()
    {
        timeText.text = (Time.time - startTime).ToString("F1") + "s";
        sizeText.text = ((int)(currentSize * 100)).ToString() + "%";
    }
    private void GameOver()
    {
        SceneManager.LoadScene(2);
    }
    private void UpdateSize(float add)
    {
        
        if (add == 0)
        {
            // Check the moving of player, and change size
            currentSize -= body.velocity.x * 0.0001f;
        } else
        {
            currentSize += add;
        }
        float size = Mathf.Clamp(currentSize * 2, 0.5f, 2f);
        // Change size of player and collider size
        transform.localScale = new Vector3(size * 1.1f, size * 1.1f, 1);
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
        fixedSpeed = Mathf.Clamp(currentSize * speedConstant, 10, 50);
        // Check if is on ground by checking overlap with a layer
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, GroundLayer);
        isJump = !isGround;

        Move();
        Jump();
        
        UpdateSize(0);
        
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
        if (hInput > 0)
        {
            playerAnimator.SetBool("run", true);
        } else
        {
            playerAnimator.SetBool("run", false);
        }
        body.AddForce(new Vector2(0, -100 * currentSize));
    }

    void Jump()
    {
        if (isGround)
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
        }

        if (jumpPressed && isGround)
        {
            // Make jump verticle to the pipe by using tangent
            float verticleJump = Mathf.Tan(Mathf.Abs(transform.localRotation.eulerAngles.z - 360));
            if (currentSize >= 0.6f)
            {
                body.AddForce(new Vector2(verticleJump * 1.4f * jumpSpeed, 1.4f * jumpSpeed));
            }
            else
            {
                body.AddForce(new Vector2(verticleJump * jumpSpeed, jumpSpeed));
            }
            jumpPressed = false;
        }
    }

}
