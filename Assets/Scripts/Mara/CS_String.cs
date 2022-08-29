using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_String : MonoBehaviour
{
    public float mMaxLenght = 5.0f;
    public float mCurrentLenght = 0.0f;
    public float mRealCurrentLenght = 0.0f;
    public Vector3 mStart;
    public Vector3 mEnd;

    public GameObject mMesh;

    List<GameObject> mStringParts = new List<GameObject>();
    public List<Vector3> mStringPoints = new List<Vector3>();

  [SerializeField] private LayerMask mStringCollisionLayerMask;

    void Start()
    {
        mStringParts.Add(Instantiate(mMesh, transform));
        mStringParts[0].name = "String Part 0";

        mStringPoints.Add(mStart);
        mStringPoints.Add(mEnd);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int StringPointsCount = mStringParts.Count;

        mStringPoints[0] = mStart;
        mStringPoints[StringPointsCount] = mEnd;

        mCurrentLenght = 0.0f;
        mRealCurrentLenght = 0.0f;

        List<int> eraseIndices = new List<int>();
        for (int i = 0; i < StringPointsCount; ++i)
        {
            Vector3 currentDir = mStringPoints[i + 1] - mStringPoints[i];
            float currentLength = currentDir.magnitude;
            mRealCurrentLenght += currentLength;
            currentLength = mCurrentLenght + currentLength > mMaxLenght ? mMaxLenght - mCurrentLenght : currentLength;
            mCurrentLenght += currentLength;
            mStringParts[i].transform.localScale = new Vector3(currentLength, 0.1f, 1.0f);

            currentDir.Normalize();
            mStringParts[i].transform.position = mStringPoints[i] + currentDir * currentLength * 0.5f;

            float stringAngle = Mathf.Atan2(currentDir.y, currentDir.x);
            mStringParts[i].transform.eulerAngles = new Vector3(0.0f, 0.0f, stringAngle * Mathf.Rad2Deg);


            if (i == StringPointsCount - 1 || i == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(mStringPoints[i], currentDir, currentLength, mStringCollisionLayerMask);

                if (hit)
                {
                    if (i == 0)
                    {
                        mStringPoints.Add(mEnd);
                        for (int j = StringPointsCount - 1; j >= 1; --j)
                        {
                            mStringPoints[j + 1] = mStringPoints[j];
                        }
                        mStringPoints[1] = new Vector3(hit.point.x, hit.point.y, 0.0f) + (currentDir + new Vector3(hit.normal.x, hit.normal.y, 0.0f)).normalized * 0.1f;
                    }
                    else
                    {
                        mStringPoints.Add(mEnd);
                        mStringPoints[i + 1] = new Vector3(hit.point.x, hit.point.y, 0.0f) + (currentDir + new Vector3(hit.normal.x, hit.normal.y, 0.0f)).normalized * 0.1f;
                    }
                    int StringPartName = mStringParts.Count;
                    mStringParts.Add(Instantiate(mMesh, transform));
                    mStringParts[StringPartName].name = "String Part" + StringPartName.ToString();
                }
            }
            if (i != StringPointsCount - 1)
            {
                currentDir = mStringPoints[i + 2] - mStringPoints[i];
                currentLength = currentDir.magnitude;
                currentDir.Normalize();
                RaycastHit2D hit = Physics2D.Raycast(mStringPoints[i], currentDir, currentLength, mStringCollisionLayerMask);

                if (!hit)
                {
                    int checksCount = 15;
                    bool interHit = false;
                    for (int j = 0; j < checksCount; ++j)
                    {
                        Vector3 interPoint = mStringPoints[i + 1] + (mStringPoints[i + 2] - mStringPoints[i + 1]) * ((float)j / checksCount);
                        currentDir = interPoint - mStringPoints[i];
                        currentLength = currentDir.magnitude;
                        currentDir.Normalize();
                        hit = Physics2D.Raycast(mStringPoints[i], currentDir, currentLength - 0.1f, mStringCollisionLayerMask);
                        if (hit)
                        {
                            interHit = true;
                            break;
                        }
                    }
                    if (!interHit) eraseIndices.Add(i + 1);
                }
            }
        }

        foreach (int ei in eraseIndices)
        {
            if (mStringParts[ei]) Destroy(mStringParts[ei]);
            mStringParts.RemoveAt(ei);
            mStringPoints.RemoveAt(ei);
        }
    }

    public void Reset()
    {
        foreach (GameObject o in mStringParts)
        {
            if (o) Destroy(o);
        }
        mStringParts.Clear();
        mStringParts.Add(Instantiate(mMesh, transform));
        mStringParts[0].name = "String Part 0";

        mStringPoints.Clear();
        mStringPoints.Add(mStart);
        mStringPoints.Add(mEnd);
    }

    public void Show(bool show)
    {
        foreach (GameObject o in mStringParts)
        {
            if (o) o.GetComponent<SpriteRenderer>().enabled = show;
        }
    }

    public float GetRealLength()
    {
        int StringPointsCount = mStringParts.Count;
        float realL = 0.0f;
        for (int i = 0; i < StringPointsCount; ++i)
        {
            Vector3 currentDir = mStringPoints[i + 1] - mStringPoints[i];
            float currentLength = currentDir.magnitude;
            realL += currentLength;
        }

        return realL;
    }
}
