using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMovement : MonoBehaviour
{
  public CharacterController2D mController;
  public Animator mAnimator;

  public GameObject mVariablePanel;

  TMP_InputField mPlayerSpeedField;
  TMP_InputField mJumpForceField;

  public float mCharacterMove = 0.0f;
  public float mCharacterSpeed = 40.0f;

  bool mIsJumping = false;

  public float mCharacterHorizontalDirection = 1.0f;

  Rigidbody2D mCharRigidbody = null;

  void Start()
  {
    mCharRigidbody = gameObject.GetComponent<Rigidbody2D>();
    if (!mCharRigidbody)
    {
      Debug.Log("No rigidbody 2D");
    }
    mVariablePanel = GameObject.FindGameObjectsWithTag("VariableControl")[0];
    mPlayerSpeedField = mVariablePanel.transform.Find("PlayerSpeed").GetComponentsInChildren<TMP_InputField>()[0];
    mJumpForceField = mVariablePanel.transform.Find("JumpForce").GetComponentsInChildren<TMP_InputField>()[0];
  }


  void Update()
  {
    mCharacterMove = Input.GetAxisRaw("Horizontal") * mCharacterSpeed;
    if (mCharacterMove != 0.0f)
    {
      mCharacterHorizontalDirection = Mathf.Sign(mCharacterMove);
    }

    if (Input.GetButtonDown("Jump"))
    {
      mIsJumping = true;
    }

    mCharacterSpeed = float.Parse(mPlayerSpeedField.text);
    mController.m_JumpForce = float.Parse(mJumpForceField.text);

  }


  void FixedUpdate()
  {
    mAnimator.SetFloat("Speed", Mathf.Abs(mCharacterMove));
    mAnimator.SetBool("Jumping", mIsJumping);

    if (mController)
    {
      mAnimator.SetBool("OnFloor", mController.isOnGround);
      mController.Move(mCharacterMove * Time.fixedDeltaTime, false, mIsJumping);
    }
    mIsJumping = false;
  }
}
