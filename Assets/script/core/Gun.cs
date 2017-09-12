using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gun : MonoBehaviour {
	public Transform TsGun;
	public Text Text_NeedCoin;
	public Transform local_GunFire;
	public Transform local_EffectGunFire;
	public GameObject Prefab_GunFire;
	//public Bullet Prefab_BulletNormal;
	public float Cooldown = 0.5F;//发射冷却
	public float CooldownMultiFix = 1F;//发射冷却调整
	public int NumBulletLimit = 15;//子弹限制

	[System.NonSerialized]
	public GunType GunType_;
	[System.NonSerialized]
	public int NumBulletInWorld = 0;//在场面上的子弹数量
	[System.NonSerialized]
	public bool Fireable = true;//是否可发射
	[System.NonSerialized]
	public bool Rotatable = true;//是否可转动
	public int NumDivide = 1;//子弹分裂数目

	public AnimationCurve Curve_GunShakeOffset;

	private float mCooldownRemain = 0F;//冷却剩余时间
	private float mLastFireTime;//最后一次发炮的时间
	private bool mFiring = false;

	public bool FastFirable = false;//是否可以快速开火
	public float FastFireSpeedUp = 1.5f;//快速开火状态下 加速百分比
	private static readonly float FastFireAddPrepareNumTime = 0.3F;//快速开火增加PrepareNum数目的时间间隔
	private static readonly int FastFireEnablePrepareNum = 2;//开启快速开火准备次数
	private static readonly float FastFireDuration = 1.5F;//快速开火持续时间

	private int mFastFire_PrepareNum = 0;//快速射击需要储蓄的按键数(在一定时间间隔内按下)
	private float mPreStartFireTime = 0F;//上次触发开火时间
	private bool mIsFastFire = false;//快速开火状态

	private bool mEffect_ShakeCoroLock = false;//效果是否在播放中
	// Use this for initialization

	public void CopyDataTo(Gun tar)
	{
		tar.Cooldown = Cooldown;
		tar.NumBulletLimit = NumBulletLimit;
		tar.NumBulletInWorld = NumBulletInWorld;
		tar.Fireable = Fireable;


		tar.mCooldownRemain = mCooldownRemain;
		tar.mLastFireTime = mLastFireTime;
		tar.mFiring = mFiring;

		//tar.mControlMode = mControlMode;
		tar.Rotatable = Rotatable;
		//tar.mLockedTarget = mLockedTarget;

		tar.mFastFire_PrepareNum = mFastFire_PrepareNum;
		tar.mIsFastFire = mIsFastFire;
		tar.CooldownMultiFix = CooldownMultiFix;
		tar.FastFireSpeedUp = FastFireSpeedUp;
		tar.FastFirable = FastFirable;
	}
	void Start () {
		Text_NeedCoin.text = GameMain.Singleton.CurFireScore.ToString();
	}

	public void StartFire()
	{
		mFiring = true;

		if (FastFirable)
		{
			if (Time.time - mPreStartFireTime < FastFireAddPrepareNumTime)
			{
				++mFastFire_PrepareNum;

				if (mFastFire_PrepareNum >= FastFireEnablePrepareNum)
				{
					mIsFastFire = true;
					CooldownMultiFix = 1F / FastFireSpeedUp;
					StopCoroutine("_Coro_StopFastFireDelay");
					StartCoroutine("_Coro_StopFastFireDelay");
				}
			}
			else
			{
				mFastFire_PrepareNum = 0;
			}

			mPreStartFireTime = Time.time;
		}
	}

	/// <summary>
    /// 停止普通开火
    /// </summary>
    public void StopNormalFire()
	{
		mFiring = false;
	}

	/// <summary>
    /// 停止所有开火
    /// </summary>
    public void StopFire()
	{
		mFiring = false;

		mFastFire_PrepareNum = 0;
		mIsFastFire = false;
		CooldownMultiFix = 1F;
		StopCoroutine("_Coro_StopFastFireDelay");
	}

	IEnumerator _Coro_StopFastFireDelay()
	{
		yield return new WaitForSeconds(FastFireDuration);
		mIsFastFire = false;
		CooldownMultiFix = 1F;
	}

	void Update()
	{
		//开火流程
		if (mIsFastFire)
			goto TAG_JUMP_FIRING_JUDGE_UPDATE;

		if (!mFiring)
		{
			return;
		}

	TAG_JUMP_FIRING_JUDGE_UPDATE://跳过mFiring判断


		if (NumBulletInWorld > NumBulletLimit)
		{
			return;
		}

		if (!Fireable)
		{
			return;
		}

		if (Time.time - mLastFireTime < mCooldownRemain)
		{
			return;
		}

		//动画
		//AniSpr_GunPot.Play(AniSpr_GunPot.clipId, 0F);
		//AniSpr_GunPot.PlayFrom(AniSpr_GunPot.DefaultClip, 0F);
		if (!mEffect_ShakeCoroLock)
			StartCoroutine(_Coro_Effect_GunShake());

		Bullet b = Instantiate(GameMain.Singleton.Prefab_BulletNormal[(int)GunNeedScoreToLevelType(GameMain.Singleton.CurFireScore)]);
		b.transform.position = local_GunFire.position;
		b.Score = GameMain.Singleton.CurFireScore;
		b.Fire(this, null, transform.rotation);

		//效果
		if (Prefab_GunFire != null)
		{
			GameObject gunfire = Instantiate(Prefab_GunFire) as GameObject;

			gunfire.transform.localScale = local_EffectGunFire.localScale;
			gunfire.transform.position = local_EffectGunFire.position;
			gunfire.transform.rotation = local_EffectGunFire.rotation;
		}
		mCooldownRemain = Cooldown * CooldownMultiFix;
		mLastFireTime = Time.time;
	}

	IEnumerator _Coro_Fire()
	{

		while (true)
		{
			if (mIsFastFire)
				goto TAG_JUMP_FIRING_JUDGE;

			if (!mFiring)
			{
				yield return 0;
				continue;
			}

		TAG_JUMP_FIRING_JUDGE://跳过mFiring判断


			if (NumBulletInWorld > NumBulletLimit)
			{
				yield return 0;
				continue;
			}

			if (!Fireable)
			{
				yield return 0;
				continue;
			}

			//换枪cd
			if (mCooldownRemain != 0F)
			{
				while (mCooldownRemain >= 0F)
				{
					mCooldownRemain -= Time.deltaTime;
					yield return 0;
				}
				mCooldownRemain = 0F;

			}

			//动画
			//AniSpr_GunPot.Play(AniSpr_GunPot.clipId,0F);
			//AniSpr_GunPot.PlayFrom(AniSpr_GunPot.DefaultClip,0F);
			StartCoroutine(_Coro_Effect_GunShake());
			//效果
			if (Prefab_GunFire != null)
			{
				GameObject gunfire = Instantiate(Prefab_GunFire) as GameObject;

				gunfire.transform.localScale = local_EffectGunFire.localScale;
				gunfire.transform.position = local_EffectGunFire.position;
				gunfire.transform.rotation = local_EffectGunFire.rotation;
			}
			mCooldownRemain = Cooldown * CooldownMultiFix;
			while (mCooldownRemain >= 0F)
			{
				mCooldownRemain -= Time.deltaTime;
				yield return 0;
			}
			mCooldownRemain = 0F;
		}


	}


	/// <summary>
	/// 直接设置押分
	/// </summary>
	public void SetNeedScore(int newScore)
	{
		Text_NeedCoin.text = newScore.ToString();
	}

	public IEnumerator _Coro_Effect_GunShake()
	{
		mEffect_ShakeCoroLock = true;
		float time = 0.1F;
		float elapse = 0F;
		Transform tsAniGun = TsGun;
		Vector3 oriPos = tsAniGun.localPosition;
		while (elapse < time)
		{
			tsAniGun.localPosition = oriPos + (Curve_GunShakeOffset.Evaluate(elapse / time)) * (tsAniGun.localRotation * Vector3.up);

			elapse += Time.deltaTime;
			yield return 0;
		}
		tsAniGun.localPosition = oriPos;
		mEffect_ShakeCoroLock = false;
	}

	/// <summary>
	/// 获得枪的功能类型
	/// </summary>
	/// <returns></returns>
	public GunPowerType GetPowerType()
	{
		return GunType_ <= GunType.NormalQuad ? GunPowerType.Normal : GunPowerType.Lizi;
	}

	/// <summary>
	/// 从分数转换到枪的等级类型
	/// </summary>
	/// <returns></returns>
	public static GunLevelType GunNeedScoreToLevelType(int curScore)
	{
		int idx = 0;
		for (idx = 0; idx != GameMain.Singleton.GunStylesScore.Length; ++idx)
		{
			if (curScore <= GameMain.Singleton.GunStylesScore[idx])
			{
				break;
			}
		}

		//Debug.LogError("枪超出ChangeGunNeedScore范围");
		return (GunLevelType)idx;
	}

	public static GunType CombineGunLevelAndPower(GunLevelType glt, GunPowerType gpt)
	{
		switch (glt)
		{
			case GunLevelType.Dbl:
				switch (gpt)
				{
					case GunPowerType.Normal:
						return GunType.Normal;
					case GunPowerType.Lizi:
						return GunType.Lizi;
				}
				break;
			case GunLevelType.Tri:
				switch (gpt)
				{
					case GunPowerType.Normal:
						return GunType.NormalTri;
					case GunPowerType.Lizi:
						return GunType.LiziTri;
				}
				break;
			case GunLevelType.Quad:
				switch (gpt)
				{
					case GunPowerType.Normal:
						return GunType.NormalQuad;
					case GunPowerType.Lizi:
						return GunType.LiziQuad;
				}
				break;

		}
		return GunType.Normal;
	}
}
