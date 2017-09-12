using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using protobuf;
using Net;
using System.IO;

public class GameMain : MonoBehaviour {
	public enum State
	{
		Idle,//待机状态,游戏未开始
		Normal,//普通出鱼
		Sweeping,//过场,(一定没子弹)
		Preluding,//开场阵列 
		BeforeSweeping_WaitBulletClear//扫场前等待子弹消失
	}

	//事件
	public static Event_Generic EvtStopGenerateFish;//停止出鱼
	public static Event_Generic EvtMainProcess_StartGame;//开始游戏
	public static Event_Generic EvtMainProcess_PrepareChangeScene;//准备过场.波浪过先
	public static Event_Generic EvtMainProcess_FinishChangeScene;//过场完毕.波浪过先
	public static Event_Generic EvtMainProcess_FinishPrelude;//鱼阵结束
	public static Event_Generic EvtMainProcess_FirstEnterScene;//第一次进入场景

	public delegate void Event_FishClear(Fish f);
	public static Event_FishClear EvtFishClear;
	public static Event_FishClear EvtFishInstance;
	public delegate void Event_LeaderInstance(Swimmer s);
	public static Event_LeaderInstance EvtLeaderInstance;//领队初始化

	public delegate void Event_BulletDestroy(Bullet b);
	public static Event_BulletDestroy EvtBulletDestroy;

	public Canvas mCanvas;

	public FishGenerator FishGenerator;//鱼群生成
	[System.NonSerialized]
	public Rect WorldDimension;
	[System.NonSerialized]
	public int NumFishAlive = 0;//活鱼数目

	public float TimeNormalScene = 240F;//普通场景持续时间 
	public float GameSpeed = 1F;//游戏速度

	public Gun GunInst;
	public Gun[] Prefab_Guns;//待换枪,对应GunType
	public Bullet[] Prefab_BulletNormal;
	public Bullet[] Prefab_BulletLizi;
	public int[] GunStylesScore;//根据该分数改变枪类型,如:= new int[]{49,999,10000}代表: 1-49,二管炮;50~999,三管炮; 1000~10000代表四管炮
	public Transform GunTrans;
	public Transform BulletLayer;
	//静态变量
	public static State State_;//GameMain状态


	//私有变量
    private static GameMain mSingleton;
	private int mCurFireScore;
	private int mOldFireScore;

	//属性
    public static GameMain Singleton
	{
		get
		{
			if (mSingleton == null)
			{
				mSingleton = GameObject.FindObjectOfType(typeof(GameMain)) as GameMain;
			}
			return mSingleton;
		}
	}

	public int CurFireScore
	{
		get
		{
			return mCurFireScore;
		}
	}

	void Awake()
	{
		mSingleton = Singleton;//初始化数据

		State_ = State.Idle;

		//初始化 2D世界范围
		WorldDimension = mCanvas.GetComponent<RectTransform>().rect;

		//鱼序号赋值
        for (int i = 0; i != FishGenerator.Prefab_FishAll.Length; ++i)
		{
			FishGenerator.Prefab_FishAll[i].TypeIndex = i;
		}

		//设置是否锁帧
        //Application.targetFrameRate = 60;

		Time.timeScale = GameSpeed;
	}

	void InitGame()
	{
        UserInfo.Instance.Init();

		mCurFireScore = 1;
		mOldFireScore = 1;

		GunLevelType glt = Gun.GunNeedScoreToLevelType(mCurFireScore);
		GunType gt = Gun.CombineGunLevelAndPower(glt, GunPowerType.Normal);

		if (GunInst != null)
			Destroy(GunInst.gameObject);

		GunInst = Instantiate(Prefab_Guns[(int)gt]) as Gun;
		GunInst.GunType_ = gt;

		GunInst.transform.parent = GunTrans;
		GunInst.transform.localPosition = Vector3.zero;
		GunInst.transform.localRotation = Quaternion.identity;
		GunInst.transform.localScale = Vector3.one;
	}
	// Use this for initialization
	void Start () {
        //NetManager.Instance.SendConnect();

        InitGame();
		FishGenerator.StartFishGenerate();
	}

    void OnEnable()
    {
        MessageCenter.Instance.addObsever(eProtocalCommand.login, Login);
    }

    void OnDisable()
    {
        MessageCenter.Instance.removeObserver(eProtocalCommand.login, Login);
    }

    void OnApplicationQuit()
    {
        //NetManager.Instance.OnRemove();
    }

    public void ClickDisConnect()
    {
        NetManager.Instance.OnRemove();
    }

    public void ClickSendConnect()
    {
        NetManager.Instance.SendConnect();
    }
    
    public void ClickLogin()
    {
        LoginRequest loginReq = new LoginRequest();
        loginReq.userName = "zhangsan";
        loginReq.password = "123";
        NetManager.Instance.SendMessage(eProtocalCommand.login, loginReq);
    }

    public void Login(byte[] data)
    {
        LoginResponse loginResp = ProtoBuf.Serializer.Deserialize(typeof(LoginResponse), new MemoryStream(data)) as LoginResponse;
        Debug.Log("Login data : " + loginResp.resultDescription);
    }

    // Update is called once per frame
    void Update () {
	
	}

	public void ClickUpFire()
	{
		mOldFireScore = mCurFireScore;
		if (mCurFireScore < 10)
			mCurFireScore += 1;
		else if (mCurFireScore < 100)
			mCurFireScore += 10;
		else if (mCurFireScore <= 1000)
			mCurFireScore += 100;

		if (mCurFireScore > 1000)
			mCurFireScore = 1;

		SetFireScore();
	}

	public void ClickDownFire()
	{
		mOldFireScore = mCurFireScore;
		if (mCurFireScore > 100)
			mCurFireScore -= 100;
		else if (mCurFireScore > 10)
			mCurFireScore -= 10;
		else if (mCurFireScore >= 0)
			mCurFireScore -= 1;

		if (mCurFireScore <= 0)
			mCurFireScore = 1000;
		
		SetFireScore();
	}

	void SetFireScore()
	{
		if (GunInst != null)
		{
			GunInst.SetNeedScore(mCurFireScore);
			GunLevelType preType = Gun.GunNeedScoreToLevelType(mOldFireScore);
			GunLevelType curType = Gun.GunNeedScoreToLevelType(mCurFireScore);
			if (curType != preType)
				On_GunLevelTypeChanged(curType);
		}
	}

	/// <summary>
    /// 押分改变(自身触发)
    /// </summary>
    /// <param name="curScore"></param>
    void On_GunLevelTypeChanged(GunLevelType curType)
	{
		ChangeGun(curType, GunInst.GetPowerType());
	}

	public bool ChangeGun(GunLevelType lvType, GunPowerType pwrType)
	{
		return ChangeGun(Gun.CombineGunLevelAndPower(lvType, pwrType));
	}

	/// <summary>
    /// 换枪
    /// </summary>
    /// <param name="gt"></param>
    /// <returns></returns>
    public bool ChangeGun(GunType gt)
	{
		Gun g = Instantiate(Prefab_Guns[(int)gt]) as Gun;
		g.GunType_ = gt;

		g.transform.parent = GunInst.transform.parent;
		g.transform.localPosition = GunInst.transform.localPosition;
		g.transform.localRotation = GunInst.transform.localRotation;
		g.transform.localScale = GunInst.transform.localScale;

		//方位
		g.TsGun.transform.localPosition = GunInst.TsGun.transform.localPosition;
		g.TsGun.transform.localRotation = GunInst.TsGun.transform.localRotation;


		GunInst.CopyDataTo(g);
		Destroy(GunInst.gameObject);
		GunInst = g;

		return true;
	}

	public void StartFire()
	{
		GunInst.StartFire();
	}

	public void StopFire()
	{
		GunInst.StopFire();
	}

	public void PlayGunAnim(Vector3 worldPos)
	{
		Transform tsGun = GunInst.gameObject.transform;
		worldPos.z = tsGun.position.z;
		Vector3 lookDirect = worldPos - tsGun.position;
		tsGun.rotation = Quaternion.LookRotation(Vector3.forward, lookDirect);
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawCube(new Vector3((WorldDimension.x + WorldDimension.width*0.5f)*0.01f, (WorldDimension.y + WorldDimension.height*0.5f)*0.01f,0f),new Vector3(WorldDimension.width*0.01f,WorldDimension.height*0.01f,0f));
	}
#endif
}
