using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
  private Rigidbody2D rig;
  private Animator anim;
  private CharacterSoundController sound;

  [Header("Movement")]
  public float MoveAccel;
  public float maxSpeed;

  [Header("Jump")]
  public float jumpAccel;
  private bool isJumping;

  [Header("Ground Raycast")]
  public float groundRaycastDistance;
  public LayerMask groundLayerMask;
  private bool isOnGround = true;


  [Header("Scoring")]
  public ScoreController score;
  public float scoringRatio;
  private float lastPositionX;


  [Header("GameOver")]
  public GameObject gameOverScreen;
  public float fallPositionY;

  [Header("Camera")]
  public CameraMoveController gameCamera;

  private void Start()
  {
    rig = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    sound = GetComponent<CharacterSoundController>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (isOnGround)
      {
        isJumping = true;
        sound.PlayJump();
      }
    }

    // change animation
    anim.SetBool("isOnGround", isOnGround);

    // calculate score
    int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
    int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

    if (scoreIncrement > 0)
    {
      score.IncreaseCurrentScore(scoreIncrement);
      lastPositionX += distancePassed;
    }

    // game over
    if (transform.position.y < fallPositionY)
    {
      GameOver();
    }
  }

  private void FixedUpdate()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);

    if (hit)
    {
      if (rig.velocity.y <= 1.5f)
      {
        isOnGround = true;
      }
      else
      {
        isOnGround = false;
      }
    }

    // calculate velocity vector
    Vector2 velocityVector = rig.velocity;

    if (isJumping)
    {
      velocityVector.y += jumpAccel;
      isJumping = false;
    }

    velocityVector.x = Mathf.Clamp(velocityVector.x + MoveAccel * Time.deltaTime, 0f, maxSpeed);

    rig.velocity = velocityVector;
  }

  private void GameOver()
  {
    // set high score
    score.FinishScoring();

    // stop camera movement
    gameCamera.enabled = false;

    // show gameover
    gameOverScreen.SetActive(true);

    // disable this too
    this.enabled = false;
  }

  private void OnDrawGizmos()
  {
    Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
  }

}