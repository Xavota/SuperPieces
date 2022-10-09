using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DerivateCharacterTest_Mara : CS_BaseCharacter_Mara
{
  new void FixedUpdate() 
  {
    base.FixedUpdate();
    
    if (mAnimator)
    {
      mAnimator.SetFloat("Speed", Mathf.Abs(mCharacterMove));
      mAnimator.SetBool("Jumping", mIsJumping);

      if (mController)
      {
        mAnimator.SetBool("OnFloor", mController.isOnGround);
      }
    }

    Debug.Log("Derivate");
  }
}
