using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
  private Rigidbody2D rig;

  [Header("Movement")]
  public float MoveAccel;
  public float maxSpeed;

  private void Start()
  {
    rig = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    Vector2 velocityVector = rig.velocity;
    velocityVector.x = Mathf.Clamp(velocityVector.x + MoveAccel * Time.deltaTime, 0f, maxSpeed);

    rig.velocity = velocityVector;
  }

}
