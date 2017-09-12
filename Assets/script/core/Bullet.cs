using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {
	public float Speed = 1F;
	public int Score = 1;
	public Text Text_CoinNum;
	public float RadiusStandardBoom = 0.175F;//爆炸最大半径,标准值: 普通子弹:0.175F, 粒子炮:0.35
	public float ScaleCollider = 0.5F;//网碰撞默认大小,当IsScaleWeb为true是无效,跟gamemain设置有关

	[System.NonSerialized]
	public Rect MoveArea;//移动区域

	private Fish mTargetFish;//锁定鱼
	[System.NonSerialized]
	public bool IsLockingFish = false;//是否锁定鱼子弹
	private Vector3 mPosSaved;
	private bool mIsDestroyed = false;
	private Transform mTs;
	private GameObject mBulletGO;

	void Awake()
	{
		mTs = transform;
	}

	void CopyDataTo(Bullet tar)
	{
		tar.Speed = Speed;
		tar.Score = Score;
		tar.RadiusStandardBoom = RadiusStandardBoom;
		tar.ScaleCollider = ScaleCollider;
		tar.mIsDestroyed = mIsDestroyed;
		tar.MoveArea = MoveArea;
	}

	public void SelfDestroy()
	{
		if (mIsDestroyed)
			return;
		
		if (GameMain.EvtBulletDestroy != null)
			GameMain.EvtBulletDestroy(this);

		Destroy(gameObject);
		--GameMain.Singleton.GunInst.NumBulletInWorld;

		mIsDestroyed = true;//Destroy不会立即使得OnTrigger失效,防止多次物理碰撞,
	}

	public void Fire(Gun from, Fish tar, Quaternion dir)
	{
		GameMain gm = GameMain.Singleton;
		if (tar != null)
		{
			mTargetFish = tar;
			IsLockingFish = true;
			Vector3 targetDir = tar.transform.position - mTs.position;
			targetDir.z = 0F;
			targetDir.Normalize();
			targetDir = Quaternion.Euler(0F, 0F, Random.Range(-90F, 90F)) * targetDir + from.transform.up;//得出子弹偏向  Random.Range(0,2)==0?-90F:90F
			mTs.rotation = Quaternion.FromToRotation(Vector3.up, targetDir);
		}
		else
		{
			IsLockingFish = false;
			mTs.rotation = dir;
		}

		mTs.parent = gm.BulletLayer;
		Text_CoinNum.text = Score.ToString();
		++from.NumBulletInWorld;

		MoveArea.x = gm.WorldDimension.x * 0.01f;
		MoveArea.y = gm.WorldDimension.y * 0.01f;
		MoveArea.width = gm.WorldDimension.width * 0.01f;
		MoveArea.height = gm.WorldDimension.height * 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
		mPosSaved = mTs.position;

		if (IsLockingFish && mTargetFish != null)
		{
			Vector3 toward = mTargetFish.transform.position - mTs.position;
			toward.z = 0F;

			Quaternion quatToTarget = Quaternion.FromToRotation(Vector3.up, toward);
			float limitDistance = 1F / toward.sqrMagnitude;
			if (limitDistance < 1F)
				limitDistance = 1F;
			mTs.rotation = Quaternion.Slerp(mTs.rotation, quatToTarget, Time.deltaTime * Speed * 0.00651F * limitDistance);
		}

		mTs.position += Speed * Time.deltaTime * mTs.up * 0.01f;

		Vector3 curPos = mTs.position;

		if (curPos.y < MoveArea.yMin || curPos.y > MoveArea.yMax)
		{
			//curPos.y = -curPos.y;
			Vector3 dir = mTs.up;
			dir.y = -dir.y;
			mTs.up = dir;
			mTs.position = mPosSaved;
			Vector3 euler = mTs.localEulerAngles;
			euler.y = 180F;
			mTs.localEulerAngles = euler;
			IsLockingFish = false;
		}
		if (curPos.x < MoveArea.xMin || curPos.x > MoveArea.xMax)
		{
			Vector3 dir = mTs.up;
			dir.x = -dir.x;
			mTs.up = dir;
			mTs.position = mPosSaved;
			Vector3 euler = mTs.localEulerAngles;
			euler.y = 180F;
			mTs.localEulerAngles = euler;
			IsLockingFish = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (mIsDestroyed)
			return;


		Fish fishFirst = other.GetComponent<Fish>();//被子弹击中的鱼
		if (fishFirst == null)
		{
			Debug.LogError("Fish在这里不可能是null!");
			return;
		}

		if (IsLockingFish && mTargetFish != null && mTargetFish.ID != fishFirst.ID)//锁目标
		{
			return;
		}

		HitProcessor.ProcessHit(this, fishFirst);

		SelfDestroy();
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 13.5f * 0.01f);
	}
#endif
}
