using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_BowShot : MonoBehaviour
{
  public CS_String mString;

  public GameObject mVariablePanel;

  TMP_InputField mShotForceField;
  TMP_InputField mArrowGravityField;
  TMP_InputField mRopeMaxLenghtField;
  TMP_InputField mPendulumGravityField;

  public bool mIsShoting = false;
  Vector2 mShotDirForce;
  public float mShotForce = 15.0f;
  Vector3 mMouseWorldPos;

  public bool mArrowStuck = false;

  public GameObject mArrowMesh;
  public GameObject mBowMesh;

  public float mArrowGravityScale = 3.0f;

  public GameObject mMousePos;

  public bool mWithString = false;
  public bool WithSting
  {
    get; set;
  }

  public float mMaxStringLenght = 15.0f;
  public float mCurrentStringLenght = 0.0f;

  float mChangeStringSizeInput = 0.0f;
  public float mChangeStringSizeSpeed = 1.0f;

  Vector3 mLastPos;

  CharacterController2D mController;
  Rigidbody2D mCharRigidbody = null;

  GameObject mObjectAttachToArrow;
  Vector3 mObjAttArrowDistanceFromCenter;

  bool mFirstFinishStringGround = true;
  Vector3 mCurrentVelocity;

  public float mGravityScale = 1.0f;
  float mAngleMovementSign = 1.0f;
  float mAngularMovementReduction = 5.0f;
  float mFrictionReduction = 0.995f;

  Vector3 mLastPendulePos;

  float mCVtoMRB = 10.0f;




  public AudioSource mArrowAudioSource;
  public AudioSource mBowShotAudioSource;

  void Start()
  {
    RestartBow();
    mLastPos = transform.position;

    mController = gameObject.GetComponent<CharacterController2D>();
    if (!mController)
    {
      CharacterMovement movScript = gameObject.GetComponent<CharacterMovement>();
      if (movScript)
      {
        mController = movScript.mController;
        if (!mController)
        {
          Debug.Log("No Controller");
          movScript.mController = gameObject.AddComponent<CharacterController2D>();
          mController = movScript.mController;
        }
      }
    }
    mCharRigidbody = gameObject.GetComponent<Rigidbody2D>();
    if (!mCharRigidbody)
    {
      Debug.Log("No rigidbody 2D");
      mCharRigidbody = gameObject.AddComponent<Rigidbody2D>();
    }

    mVariablePanel = GameObject.FindGameObjectsWithTag("VariableControl")[0];
    mShotForceField = mVariablePanel.transform.Find("ShotForce").GetComponentsInChildren<TMP_InputField>()[0];
    mArrowGravityField = mVariablePanel.transform.Find("ArrowGravity").GetComponentsInChildren<TMP_InputField>()[0];
    mRopeMaxLenghtField = mVariablePanel.transform.Find("MaxString").GetComponentsInChildren<TMP_InputField>()[0];
    mPendulumGravityField = mVariablePanel.transform.Find("PendulumGravity").GetComponentsInChildren<TMP_InputField>()[0];
  }

  void Update()
  {
    mShotForce = float.Parse(mShotForceField.text);
    mArrowGravityScale = float.Parse(mArrowGravityField.text);
    mMaxStringLenght = float.Parse(mRopeMaxLenghtField.text);
    mGravityScale = float.Parse(mPendulumGravityField.text);

    mMouseWorldPos = Input.mousePosition;
    mMouseWorldPos = Camera.main.ScreenToWorldPoint(mMouseWorldPos);
    mMouseWorldPos.z = 0.0f;
    mMousePos.transform.position = mMouseWorldPos;

    Vector2 shotDir = (Vector2)(mMouseWorldPos - transform.position).normalized;
    mBowMesh.transform.localPosition = new Vector3(shotDir.x, shotDir.y, 0.0f);
    float bowAngle = Mathf.Atan2(shotDir.y, shotDir.x);
    mBowMesh.transform.eulerAngles = new Vector3(0.0f, 0.0f, bowAngle * Mathf.Rad2Deg - 90.0f);
    if (Input.GetButtonDown("Shot"))
    {
      ShotBow(shotDir, mShotForce);
    }
    if (Input.GetButtonDown("Recall"))
    {
      RestartBow();
    }
    //if (!mIsShoting && mArrowStuck)
    //{
    //  mChangeStringSizeInput = Input.GetAxis("ChangeStringSize");
    //  if (mChangeStringSizeInput != 0)
    //  {
    //    mCurrentStringLenght -= mChangeStringSizeSpeed * Mathf.Sign(mChangeStringSizeInput);
    //    mCurrentStringLenght = mCurrentStringLenght < 0 ? 0
    //                         : (mCurrentStringLenght > mMaxStringLenght ? mMaxStringLenght
    //                         : mCurrentStringLenght);
    //    mString.mMaxLenght = mCurrentStringLenght;
    //  }
    //}
    if (Input.GetButtonDown("Restart"))
    {
      RestartLevel();
    }
  }

  void FixedUpdate()
  {
    mString.mStart = transform.position;
    mString.mEnd = mArrowMesh.transform.position;
    if (mIsShoting)
    {
      mArrowMesh.transform.position += (Vector3)mShotDirForce * Time.fixedDeltaTime;

      mShotDirForce.y -= mArrowGravityScale * Time.fixedDeltaTime;

      if (mString.GetRealLength() >= mString.mMaxLenght)
      {
        mIsShoting = false;
        mString.Reset();

        mArrowMesh.transform.position = transform.position;

        mString.mStart = transform.position;
        mString.mEnd = mArrowMesh.transform.position;
      }

      if (mString.mCurrentLenght > mCurrentStringLenght)
      {
        mCurrentStringLenght = mString.mCurrentLenght;
      }

      float dirAngle = Mathf.Atan2(mShotDirForce.y, mShotDirForce.x);
      mArrowMesh.transform.eulerAngles = new Vector3(0.0f, 0.0f, dirAngle * Mathf.Rad2Deg - 90.0f);
    }
    else if (!mArrowStuck)
    {
      mArrowMesh.transform.position = transform.position;
    }
    else
    {
      //mArrowMesh.transform.position = mObjectAttachToArrow.transform.position + mObjAttArrowDistanceFromCenter;
      mString.mEnd = mArrowMesh.transform.position;
      if (mString.GetRealLength() > mString.mMaxLenght - 0.001f)
      {
        FinishedString();
      }
      else if (!mFirstFinishStringGround)
      { 
        mFirstFinishStringGround = true;

        mCharRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        mCharRigidbody.velocity = mCurrentVelocity * mCVtoMRB;
      }
    }

    mLastPos = transform.position;





    if (transform.position.y < -10.0f)
    {
      RestartLevel();
    }
  }

  private void FinishedString()
  {
    if (mController.isOnGround)
    {
      FinishedStringOnGround();
    }
    else
    {
      FinishedStringOnAir();
    }

    float realL = mString.GetRealLength();
    if (realL > mString.mMaxLenght + 1.0f)
    {
      RestartBow();
    }
    else if (realL > mString.mMaxLenght + 0.01f)
    {
      Vector3 pullDir = (mString.mStringPoints[1]
                       - mString.mStringPoints[0]).normalized;
      Vector2 pullDir2 = new Vector2(pullDir.x, 0.0f);

      //transform.position += pullDir;
      mCharRigidbody.AddForce(pullDir2 * 100.0f);
    }
  }
  private void FinishedStringOnGround()
  {
    if (!mFirstFinishStringGround)
    {
      mFirstFinishStringGround = true;

      mCharRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
      mCharRigidbody.velocity = mCurrentVelocity * mCVtoMRB;
    }

    if (mObjectAttachToArrow && mObjectAttachToArrow.GetComponent<CS_DynamicObject>())
    {
      CS_DynamicObject dynScrip = mObjectAttachToArrow.GetComponent<CS_DynamicObject>();

      float pullForce = mString.mMaxLenght - mString.GetRealLength();

      int stringPoinsCount = mString.mStringPoints.Count;
      Vector3 pullDir = (mString.mStringPoints[stringPoinsCount - 1]
                       - mString.mStringPoints[stringPoinsCount - 2]).normalized
                      * pullForce;

      dynScrip.PullObject(pullDir);
    }
    else
    {
      transform.position = mLastPos;
    }
  }
  private void FinishedStringOnAir()
  {
    if (mObjectAttachToArrow && mObjectAttachToArrow.GetComponent<CS_DynamicObject>())
    {
      if (!mFirstFinishStringGround)
      {
        mFirstFinishStringGround = true;

        mCharRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        mCharRigidbody.velocity = mCurrentVelocity * mCVtoMRB;
      }

      CS_DynamicObject dynScrip = mObjectAttachToArrow.GetComponent<CS_DynamicObject>();

      float pullForce = mString.mMaxLenght - mString.GetRealLength();

      int stringPoinsCount = mString.mStringPoints.Count;
      Vector3 pullDir = (mString.mStringPoints[stringPoinsCount - 1]
                       - mString.mStringPoints[stringPoinsCount - 2]).normalized
                      * pullForce;

      dynScrip.PullObject(pullDir);
    }
    else
    {
      mLastPendulePos = transform.position;

      int stringPoinsCount = mString.mStringPoints.Count;
      Vector3 pivotPoint = mString.mStringPoints[1];
      Vector3 finalPoint = transform.position;
      Vector3 dir = finalPoint - pivotPoint;

      if (mFirstFinishStringGround)
      {
        mFirstFinishStringGround = false;

        mCurrentVelocity = mCharRigidbody.velocity * new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), 0.0f).normalized * 0.5f;

        mAngleMovementSign = Mathf.Sign(mCurrentVelocity.x);

        mCharRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        mCharRigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
      }

      float radius = dir.magnitude;
      dir.Normalize();

      float velocityMagnitude = mCurrentVelocity.magnitude * mAngularMovementReduction;
      float angularMovement = Mathf.Abs(velocityMagnitude / radius) * mAngleMovementSign;

      float currentAngle = Mathf.Atan2(dir.y, dir.x) + angularMovement * Time.fixedDeltaTime;

      Vector3 newPosition = pivotPoint + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0.0f).normalized * radius;
      transform.position = newPosition;

      float thetaW = currentAngle + Mathf.PI * 0.5f * mAngleMovementSign;
      angularMovement -= (mGravityScale * Mathf.Sin(thetaW) * Time.fixedDeltaTime * mAngleMovementSign);
      angularMovement *= mFrictionReduction;
      velocityMagnitude = Mathf.Abs(angularMovement * radius * (1.0f / mAngularMovementReduction));
      mAngleMovementSign = Mathf.Sign(angularMovement);


      mCurrentVelocity = new Vector3(Mathf.Cos(thetaW), Mathf.Sin(thetaW), 0.0f).normalized * velocityMagnitude;
    }
  }

  public void RestartBow()
  {
    mArrowStuck = false;
    mIsShoting = false;

    mArrowMesh.transform.position = transform.position;
    mArrowMesh.GetComponent<SpriteRenderer>().enabled = false;

    mArrowMesh.transform.SetParent(null);
    mArrowMesh.GetComponent<Rigidbody2D>().simulated = true;

    mString.Show(false);

    mString.mStart = transform.position;
    mString.mEnd = mArrowMesh.transform.position;

    mCurrentStringLenght = 0.0f;
    mString.mMaxLenght = mMaxStringLenght;

    mString.Reset();


    if (!mFirstFinishStringGround)
    {
      mFirstFinishStringGround = true;

      mCharRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
      mCharRigidbody.velocity = mCurrentVelocity * mCVtoMRB;
    }
  }

  public void ShotBow(Vector2 direction, float force)
  {
    RestartBow();

    mArrowMesh.GetComponent<SpriteRenderer>().enabled = true;
    mArrowMesh.transform.position =
    transform.position + new Vector3(direction.x, direction.y, 0.0f);

    mIsShoting = true;
    mShotDirForce = direction * force;

    mString.Show(true);

    mBowShotAudioSource.Play();
  }

  public void ArrowCollides(GameObject collidingObj)
  {
    if (mIsShoting)
    {
      mCurrentStringLenght += 0.75f;
      mString.mMaxLenght = mCurrentStringLenght;

      mIsShoting = false;
      mArrowStuck = true;

      mObjectAttachToArrow = collidingObj;

      //mObjAttArrowDistanceFromCenter = mArrowMesh.transform.position - mObjectAttachToArrow.transform.position;
      mArrowMesh.transform.SetParent(mObjectAttachToArrow.transform, true);
      mArrowMesh.GetComponent<Rigidbody2D>().simulated = false;

      mArrowAudioSource.Play();
    }
  }

  void OnCollisionEnter2D(Collision2D other)
  {
    if (!mFirstFinishStringGround)
    {
      transform.position = mLastPendulePos;

      mCurrentVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
  }




  void RestartLevel()
  {
    Application.LoadLevel("SC_TestScene");
  }
}
