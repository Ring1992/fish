using UnityEngine;
using System.Collections;

public class Swimmer : MonoBehaviour {
	public enum State
	{
		Stop, Swim
	}

	//事件
    public delegate void Event_RotateStart(float angle);
	public Event_RotateStart EvtRotateStart;//开始旋转

	public Event_Generic EvtSwimOutLiveArea;//游开了生存区域

	//变量
    public float BoundCircleRadius = 1F;//绑定园半径
	private Rect mLiveDimension;//可生存区域,在初始化时计算

	public float Speed = 1F;
	public float RotateSpd = 180F;//角度/秒 

	public State CurrentState
	{
		get { return mState; }
		set { mState = value; }
	}

	private Transform mTs;
	private State mState;
	private static readonly float CheckLiveInterval = 0.2F;//检查是否超出生存范围时间
	private float mCheckLiveRemainTime = 0F;

	private bool mStateRotating = false;//是否在旋转状态
	struct RotateData
	{
		public float rotateDir;
		public float angleAbs;
		public float stopRotateRadian;
		public float rotatedAngle;
		public float rotateDelta;
	}
	private RotateData mCurRotateData;
	public void CopyDataTo(Swimmer tar)
	{
		tar.BoundCircleRadius = BoundCircleRadius;
		tar.Speed = Speed;
		tar.RotateSpd = RotateSpd;
		tar.mLiveDimension = mLiveDimension;
		//Prefab复制没有计算mLiveDimenesion
		//tar.SetLiveDimension(BoundCircleRadius);
	}

	void Awake()
	{
		mTs = transform;
		mCheckLiveRemainTime = CheckLiveInterval;
		SetLiveDimension(Defines.ClearFishRadius);
	}

	/// <summary>
    /// 是否在生存区域
    /// </summary>
    /// <returns></returns>
    public bool IsInLiveArea()
	{
		return mLiveDimension.Contains(mTs.position);
	}

	/// <summary>
    /// 漏一半视为进入世界
    /// </summary>
    /// <returns></returns>
    public bool IsInWorld()
	{
		return GameMain.Singleton.WorldDimension.Contains(mTs.position);
	}

	public void SetLiveDimension(float radiusMulti)
	{
		mLiveDimension.x = (GameMain.Singleton.WorldDimension.x - BoundCircleRadius * radiusMulti)*0.01f;
		mLiveDimension.y = (GameMain.Singleton.WorldDimension.y - BoundCircleRadius * radiusMulti)*0.01f;
		mLiveDimension.width = (GameMain.Singleton.WorldDimension.width + 2F * BoundCircleRadius * radiusMulti)*0.01f;
		mLiveDimension.height = (GameMain.Singleton.WorldDimension.height + 2F * BoundCircleRadius * radiusMulti)*0.01f;
	}

	public void Go()
	{
		mState = State.Swim;
	}

	public void StopImm()
	{
		mState = State.Stop;
	}

	public void Rotate(float angle)
	{
		if (mTs == null)
			mTs = transform;
		StartRotate(angle);
		//StopCoroutine("_Coro_Rotate");
		//StartCoroutine("_Coro_Rotate", angle);
	}
	//IEnumerator _Coro_Rotate(float angle)
    //{
    //    if (EvtRotateStart != null)
    //        EvtRotateStart(angle);
    //    float rotateDir = Mathf.Sign(angle);
    //    float angleAbs = Mathf.Abs(angle) /**Mathf.Deg2Rad*/;
    //    float stopRotateRadian = angleAbs * 0.95F;
    //    float rotatedAngle = 0F;
    //    float rotateDelta = 0F;
    //    while (rotatedAngle < stopRotateRadian)
    //    {
    //        if (mState == State.Swim)
    //        {
    //            rotateDelta = RotateSpd * Time.deltaTime * (1F - rotatedAngle / angleAbs);
    //            mTs.rotation *= Quaternion.Euler(0F, 0F, rotateDir * rotateDelta);
    //            rotatedAngle += rotateDelta;
    //        }
    //        yield return 0;
    //    }
    //}

	void StartRotate(float angle)
	{
		mStateRotating = true;

		if (EvtRotateStart != null)
			EvtRotateStart(angle);

		mCurRotateData.rotateDir = Mathf.Sign(angle);
		mCurRotateData.angleAbs = Mathf.Abs(angle) /**Mathf.Deg2Rad*/;
		mCurRotateData.stopRotateRadian = mCurRotateData.angleAbs * 0.95F;
		mCurRotateData.rotatedAngle = 0F;
		mCurRotateData.rotateDelta = 0F;


	}
	
	// Update is called once per frame
	void Update () {
		if (mState == State.Swim)
		{
			mTs.localPosition += Speed * Time.deltaTime * mTs.right;

			if (mStateRotating)
			{
				mCurRotateData.rotateDelta = RotateSpd * Time.deltaTime * (1F - mCurRotateData.rotatedAngle / mCurRotateData.angleAbs);
				mTs.rotation *= Quaternion.Euler(0F, 0F, mCurRotateData.rotateDir * mCurRotateData.rotateDelta);
				mCurRotateData.rotatedAngle += mCurRotateData.rotateDelta;
				if (mCurRotateData.rotatedAngle > mCurRotateData.stopRotateRadian)
					mStateRotating = false;
			}
		}

		if (mCheckLiveRemainTime < 0F)
		{
			if (!IsInLiveArea() && EvtSwimOutLiveArea != null)
				EvtSwimOutLiveArea();

			mCheckLiveRemainTime = CheckLiveInterval;
		}
		mCheckLiveRemainTime -= Time.deltaTime;
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, BoundCircleRadius*0.01f);
		//Gizmos.DrawLine(new Vector3(mLiveDimension.x,mLiveDimension.y,0),new Vector3(mLiveDimension.x+mLiveDimension.width,mLiveDimension.y,0));
		//Gizmos.DrawLine(new Vector3(mLiveDimension.x, mLiveDimension.y, 0), new Vector3(mLiveDimension.x, mLiveDimension.y + mLiveDimension.height, 0));
		//Gizmos.DrawLine(new Vector3(mLiveDimension.x, mLiveDimension.y + mLiveDimension.height, 0), new Vector3(mLiveDimension.x + mLiveDimension.width, mLiveDimension.y + mLiveDimension.height, 0));
		//Gizmos.DrawLine(new Vector3(mLiveDimension.x + mLiveDimension.width, mLiveDimension.y, 0), new Vector3(mLiveDimension.x + mLiveDimension.width, mLiveDimension.y + mLiveDimension.height, 0));
	}
#endif
}
