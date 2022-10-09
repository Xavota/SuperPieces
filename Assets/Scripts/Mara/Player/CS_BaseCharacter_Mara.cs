using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_BaseCharacter_Mara : MonoBehaviour
{
  public CharacterController2D mController = null;
  public Animator mAnimator = null;

  protected Rigidbody2D mCharRigidbody = null;

  protected float mCharacterMove = 0.0f;
  public float Movement
  {
    get
    {
      return mCharacterMove;
    }
  }
  public float mCharacterSpeed = 40.0f;

  protected float mCharacterHorizontalDirection = 1.0f;
  public float HorizontalDirection
  {
    get
    {
      return mCharacterHorizontalDirection;
    }
  }

  protected bool mIsJumping = false;


  void Start()
  {
    mCharRigidbody = gameObject.GetComponent<Rigidbody2D>();
    if (!mCharRigidbody)
    {
      Debug.Log("No rigidbody 2D");
    }
  }


  void Update()
  {
    mCharacterMove = Input.GetAxisRaw("Horizontal") * mCharacterSpeed;
    if (mCharacterMove != 0.0f)
    {
      mCharacterHorizontalDirection = Mathf.Sign(mCharacterMove);
    }
    else
    {
      mCharacterHorizontalDirection = 0.0f;
    }

    if (Input.GetButtonDown("Jump"))
    {
      mIsJumping = true;
    }
  }


  protected void FixedUpdate()
  {
    if (mController)
    {
      mController.Move(mCharacterMove * Time.fixedDeltaTime, false, mIsJumping);
    }
    mIsJumping = false;
  }
}
