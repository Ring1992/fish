using UnityEngine;

public static class Defines{
    public static int SocketPort = 8004;                           //Socket服务器端口
    public static string SocketAddress = "192.168.2.37";          //Socket服务器地址

    public static float ClearFishRadius = 1.1F;//鱼离开场景距离达到 KillFishRadius*自己的半径 则清除
	public static UnityEngine.Rect WorldDimensionUnit = new UnityEngine.Rect(-672F, -375F, 1344F, 750F);//单位屏幕区域(Unity世界单位)

	public static float CanvasWidthUnit = 1344f;
	public static float CanvasHeightUnit = 750f;
}

public enum Language
{
	Cn,
	En
}

public static class PubFunc
{
	public static Quaternion QuatRotateZ90 = new Quaternion(0F, 0F, 0.7F, 0.7F);//绕Z轴旋转90度

	/// <summary>
	/// 获得旋转right向量到指定向量的四元数
	/// </summary>
	/// <param name="right"></param>
	/// <returns></returns>
	public static Quaternion RightToRotation(Vector3 rightTarget)
	{
		return Quaternion.LookRotation(Vector3.forward, rightTarget) * QuatRotateZ90;
	}
}

public enum HittableType
{
	Normal,         //普通鱼typeIndex:0~49
	AreaBomb,       //范围炸弹typeIndex 100~149
	SameTypeBomb,   //同类炸弹typeIndex:与Normal鱼一致
	FreezeBomb,      //定身炸弹
	SameTypeBombEx  //触发所有同类炸弹的炸弹
}

public delegate void Event_Generic();

[System.Serializable]
public class WebScoreScaleRatio
{
	public WebScoreScaleRatio()
	{
		Scale = PositionScale = /*ScaleCollider =*/ BubbleScale = 1F;

	}
	public int StartScore;//开始分值,不可超过下一开始分值
						  //public float Size;//网大小unity中单位
	public float Scale;//标准缩放值-网缩放值
	public float PositionScale;//位置缩放值
							   //public float ScaleCollider;//碰撞缩放值
	public float BubbleScale;//泡泡缩放
	public string NameSprite;//网sprite id
	public GameObject PrefabWeb;//网sprite prefab
	public GameObject PrefabWebBoom;//网效果,格局prefab
}

/// <summary>
/// 飞币种类
/// </summary>
public enum FlyingCoinType
{
	Sliver, Glod
}

/// <summary>
/// 枪总类型
/// </summary>
public enum GunType
{
	Normal,
	NormalTri,
	NormalQuad,
	Lizi,
	LiziTri,
	LiziQuad
}

/// <summary>
/// 枪分级类型,二管.三管.四管
/// </summary>
public enum GunLevelType
{
	Dbl,
	Tri,
	Quad
}

/// <summary>
/// 功能分类,普通,离子
/// </summary>
public enum GunPowerType
{
	Normal,
	Lizi
}

/// <summary>
/// 游戏难度
/// </summary>
public enum GameDifficult
{
	VeryEasy,
	Easy,
	Hard,
	VeryHard,
	DieHard
}

/// <summary>
/// 出奖励类型
/// </summary>
public enum OutBountyType
{
	OutCoinButtom,
	OutTicketButtom,
	OutCoinImmediate,
	OutTicketImmediate
}

/// <summary>
/// 场地类型
/// </summary>
public enum ArenaType
{
	Small, Medium, Larger
}
