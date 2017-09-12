using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Callback_NetMessage_Handle(byte[] _data);
public class MessageCenter : SingletonMonoBehaviour<MessageCenter>
{
    private Dictionary<eProtocalCommand, Callback_NetMessage_Handle> _netMessage_EventList = new Dictionary<eProtocalCommand, Callback_NetMessage_Handle>();
    public Queue<KeyValuePair<eProtocalCommand, byte[]>> _netMessageDataQueue = new Queue<KeyValuePair<eProtocalCommand, byte[]>>();

    //添加网络事件观察者
    public void addObsever(eProtocalCommand _protocalType, Callback_NetMessage_Handle _callback)
    {
        if (_netMessage_EventList.ContainsKey(_protocalType))
        {
            _netMessage_EventList[_protocalType] += _callback;
        }
        else
        {
            _netMessage_EventList.Add(_protocalType, _callback);
        }
    }
    //删除网络事件观察者
    public void removeObserver(eProtocalCommand _protocalType, Callback_NetMessage_Handle _callback)
    {
        if (_netMessage_EventList.ContainsKey(_protocalType))
        {
            _netMessage_EventList[_protocalType] -= _callback;
            if (_netMessage_EventList[_protocalType] == null)
            {
                _netMessage_EventList.Remove(_protocalType);
            }
        }
    }

    void Update()
    {
        while (_netMessageDataQueue.Count > 0)
        {
            lock (_netMessageDataQueue)
            {
                KeyValuePair<eProtocalCommand, byte[]> tmpNetMessageData = _netMessageDataQueue.Dequeue();
                if (_netMessage_EventList.ContainsKey(tmpNetMessageData.Key))
                {
                    _netMessage_EventList[tmpNetMessageData.Key](tmpNetMessageData.Value);
                }
            }
        }
    }
}
