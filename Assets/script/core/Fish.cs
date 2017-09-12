using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fish : MonoBehaviour/* ,IPoolObj*/
{
	[System.NonSerialized]
	public int OddBonus = 1;//用于奖励的赔率(有hitprocess赋值,该值应该由kill传入,因为不想复杂化代码,而做的临时方案)
	[System.NonSerialized]
	public float TimeDieAnimation = 1.38F;//死亡动画持续时间(默认:1.35秒)

	/// <summary>
	/// //鱼序列说明
	///  普通鱼:    0~49
	///  范围炸弹:  100~149
	///  同类炸弹:  70~99
	/// </summary>
	public int TypeIndex = 0;//类型索引,顺序递增,不重复,用于放入数组.需要手动赋值.为了效率
							 //public HittableType HittableType_ = HittableType.Normal;
	public bool HitByBulletOnly = false;//只能被子弹击中攻击

	public float AreaBombRadius = 1.6F;//如果是范围炸弹的话.爆炸的范围半径
	public int AreaBombOddLimit = 300;//炸弹倍数限制,超过倍数之后的鱼不参与赔率计算
	public Fish[] Prefab_SameTypeBombAffect;//如果是同类鱼炸弹,是炸那个类型的

	public bool IsLockable = true;//是否可被锁定

	public int FishTypeIdx;
	//[HideInInspector]
	public string HittableTypeS;
	//位移标识,awake处复制
	public uint ID
	{
		get
		{
			if (mID == 0)
			{
				mID = mIDGenerateNow;
				++mIDGenerateNow;
				if (mIDGenerateNow == 0)//保证不存在0的ID
					++mIDGenerateNow;
			}
			return mID;
		}
	}

	public AudioClip[] Snds_Die;


	[System.NonSerialized]
	public bool Attackable = true;//是否可攻击

	private Transform mTs;

	private Renderer mRenderer;
	private uint mID = 0;
	private static uint mIDGenerateNow = 1;// 用于计算当前鱼id
	private Swimmer mSwimmer;
	public Swimmer swimmer
	{
		get
		{
			if (mSwimmer == null)
				mSwimmer = GetComponent<Swimmer>();
			return mSwimmer;
		}
	}

	public bool VisiableFish
	{
		set
		{
			if (mRenderer == null)
			{
				mRenderer = GetComponentInChildren<Renderer>();
			}
			mRenderer.enabled = value;
		}

	}

	public void CopyDataTo(Fish tar)
	{

		tar.Attackable = Attackable;
		tar.mID = mID;

	}

	// 
	//     public void On_Reuse(GameObject prefab)
	//     {
	//         gameObject.SetActive(true);
	//         prefab.GetComponent<Fish>().CopyDataTo(this);
	//         VisiableFish = true;
	//         collider.enabled = true;
	//         ++GameMain.Singleton.NumFishAlive;
	//         mAnimationSprite = AniSprite;//调用一下初始化函数
	//  
	//     }
	// 
	// 
	//     public void On_Recycle()
	//     {
	//         StopAllCoroutines();
	//         gameObject.SetActive(false);
	//  
	//         swimmer.CurrentState = Swimmer.State.Stop;
	//        
	//     }
	void Awake()
	{
		mTs = transform;
		swimmer.EvtSwimOutLiveArea += Handle_SwimOutLiveArea;
		++GameMain.Singleton.NumFishAlive;
	}

	void Handle_SwimOutLiveArea()
	{
		if (Attackable)
		{
			Attackable = false;
			Clear();
		}
	}

	//private bool mIsCleaned = false;
	/// <summary>
	/// 清除,从屏幕上消失
	/// </summary>
	public void Clear()
	{
		//if (mIsCleaned)
		//    return;

		//mIsCleaned = true;

		if (GameMain.Singleton != null)
			--GameMain.Singleton.NumFishAlive;

		Attackable = false;

		Destroy(gameObject);
	}

	public void ClearAI()
	{
		Component[] fishAIs = GetComponents(typeof(IFishAI));
		for (int i = 0; i != fishAIs.Length; ++i)
		{
			Destroy(fishAIs[i]);
		}
	}
}
