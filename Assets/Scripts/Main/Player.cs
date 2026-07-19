using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool canBeKnocked;
    private bool isDead;
    private bool isLanding;

    [Header("VFX Info: ")]
    [SerializeField] private ParticleSystem dustVFX;
    [SerializeField] private ParticleSystem bloodVFX;

    [Header("Knockback Info:")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isKnocked;
    [HideInInspector] public bool hasExtraLife;

    [Header("Speed Info:")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [Space]
    [SerializeField] private float speedMilestone;
    [SerializeField] private float milestoneIncreaser;
    [SerializeField] private float defaultMilestoneIncrease;

    [Header("Move Info:")]
    [SerializeField] private float moveSpeed;
    [HideInInspector] public bool isMoving;

    [Header("Jump Info:")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForceFactor;
    private bool canDoubleJump;

    [Header("Slide Info:")]
    [SerializeField] private float defaultSlideSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideSpeedMultiplier;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCoolDownTime;
    [SerializeField] private float slideCoolDownTimer;
    private bool isSliding;
    [SerializeField] public bool canSlide;

    [Header("Collision Info:")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private Vector2 wallCheckTempSize;

    [Header("Ledge Info:")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbStartPosition;
    private Vector2 climbEndPosition;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool ceilingCheck;
    [SerializeField] private bool wallDetected;
    [SerializeField]private bool canGrabLedge;
    [SerializeField]private bool canClimbLedge;

    private bool canRoll;

    [SerializeField] public bool ledgeDetected;

    void Start()
    {
        hasExtraLife = true;
        canBeKnocked = true;
        canGrabLedge = true;
        canDoubleJump = true;
        canSlide = true;
        speedMilestone = 50;
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncrease;
        slideSpeed = defaultSlideSpeed;
        wallCheckTempSize = wallCheckSize;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckAnimation();
        CheckInput();

        if (isDead || isKnocked) return;

        if (isMoving)
        {
            Move();
        }
        else if (!isMoving && isGrounded)
        {
            Stop();
        }

        if (isGrounded) canDoubleJump = true;

        CheckForSlidingWallCheck();
        CheckForSpeed();
        CheckForLedge();
        CheckCollision();
        CheckForSliding();
        CheckForSlidingCoolDown();
        CheckForLanding();
    }

    private void CheckForSlidingWallCheck()
    {
        if (isSliding)
        {
            wallCheckTempSize = wallCheckSize;
            wallCheckSize.y /= 2;
        }
        else
        {
            wallCheckSize = wallCheckTempSize;
        }
    }
    private void CheckForLanding()
    {
        if (rb.linearVelocityY < -5 && !isGrounded)
            isLanding = true;
        if (isLanding && isGrounded)
        {
            dustVFX.Play();
            isLanding = false;
        }
    }
    private void RollOver() => canRoll = false;
    private void ResetSpeed()
    {
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncrease;
        slideSpeed = defaultSlideSpeed;
    }
    private void CheckForSpeed()
    {
        if (moveSpeed == maxSpeed)
        {
            hasExtraLife = true;
            return;
        }

        if(transform.position.x > speedMilestone)
        {
            moveSpeed *= speedMultiplier;
            slideSpeed *= slideSpeedMultiplier;
            speedMilestone += milestoneIncreaser;
            milestoneIncreaser *= speedMultiplier;
            if(moveSpeed > maxSpeed) moveSpeed = maxSpeed;
        }
    }
    private void CheckForSlidingCoolDown()
    {
        if (!isSliding)
        {
            slideCoolDownTimer -= Time.deltaTime;
            if (slideCoolDownTimer < 0)
            {
                canSlide = true;
                slideCoolDownTimer = 0;
            }
        }
    }
    private void CheckForSliding()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if(slideTimer < 0 && !ceilingCheck || wallDetected)
            {
                isSliding = false;
                slideTimer = 0;
                slideCoolDownTimer = slideCoolDownTime;
            }
        } 
    }
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbStartPosition = ledgePosition + offset1;
            climbEndPosition = ledgePosition + offset2;
            canClimbLedge = true;
        }

        if (canClimbLedge)
            transform.position = climbStartPosition;
    }
    public void CheckForJumping()
    {
        if (isSliding || isDead) return;

        if (isGrounded)
            Jump(jumpForce);
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump(jumpForce * doubleJumpForceFactor);
        }
    }
    private void LedgeClimbOver()
    {
        canClimbLedge = false;
        wallDetected = false;
        rb.gravityScale = 6;
        transform.position = climbEndPosition;
        Invoke("DelayLedgeClimbOver", .1f);
    }
    private void DelayLedgeClimbOver() => canGrabLedge = true;
    private void KnockbackOver() => isKnocked = false;
    public void Damage()
    {
        bloodVFX.Play();
        if (hasExtraLife)
        {
            Knockback();
            ResetSpeed();
        }
        else
            StartCoroutine(Die());
    }
    public IEnumerator Die()
    {
        GameManager.instance.GameEnded();
        isDead = true;
        canBeKnocked = false;
        rb.linearVelocityX = knockbackDirection.x;
        yield return new WaitForSeconds(1f);
        rb.linearVelocity = new Vector2(0, 0);
    }
    private IEnumerator Invincibility()
    {
        Color originalColor = sr.color;
        Color darkerColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        float waitTime = 0.1f;
        canBeKnocked = false;
        while(waitTime <= 0.3f)
        {
            sr.color = originalColor;
            yield return new WaitForSeconds(waitTime);
            sr.color = darkerColor;
            yield return new WaitForSeconds(waitTime);
            waitTime += 0.05f;
        }
        sr.color = originalColor;
        canBeKnocked = true;
    }
    private void Knockback()
    {
        if (!canBeKnocked) return;
        StartCoroutine(Invincibility());
        hasExtraLife = false;
        isKnocked = true;
        if (isSliding) isSliding = false;
        rb.linearVelocityX = knockbackDirection.x;
    }
    private void Move()
    {
        if (wallDetected)
        {
            Stop();
            return;
        }

        if(isSliding)
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
        else
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
    }
    private void Jump(float force)
    {
        dustVFX.Play();
        AudioManager.instance.PlaySFX(Random.Range(1, 2));
        rb.linearVelocity = new Vector2(rb.linearVelocityX, force);
    }
    public void Slide()
    {
        if (isDead) return;

        if(rb.linearVelocityX != 0 && canSlide)
        {
            dustVFX.Play();
            isSliding = true;
            canSlide = false;
            slideTimer = slideTime;
        }
    }
    private void Stop() => rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForJumping();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            Slide();
        }
    }
    private void CheckAnimation()
    {
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimbLedge);
        anim.SetBool("canRoll", canRoll);
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isDead", isDead);

        if (rb.linearVelocityY < -30 || !canDoubleJump) canRoll = true;
    }
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround) || !canGrabLedge;
        ceilingCheck = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
    }
}
