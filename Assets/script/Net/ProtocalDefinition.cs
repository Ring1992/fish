/// <summary>
/// 网络事件协议ID
/// </summary>
public enum eProtocalCommand
{
    login = 1001,
}

/// <summary>
/// 网络事件外层返回结果ID
/// </summary>
public enum eResultOutStatus
{
    MSG_SUCCESS = 9999,
    SYSTEM_UNKNOWN_ERROR = 9000,
    NO_HAVE_INTERFACE = 9001,
    TOKEN_ERROR = 9002,
    DECODING_ERROR = 9003,
    VERIFY_ERROR = 9004,
    PARAMS_ERROR = 9005,
    DEVICE_ID_NULL = 9006,
    DEVICE_ID_NO_HAVE = 9007,
}

/// <summary>
/// 网络事件外层返回结果ID
/// </summary>
public enum eResultInStatus
{
    bodySuccess = 999,
}