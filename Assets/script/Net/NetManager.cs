using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using protobuf;

namespace Net
{
    public class NetManager : SingletonMonoBehaviour<NetManager>
    {
        private SocketClient _socketClient;
        SocketClient socketClient
        {
            get
            {
                if (_socketClient == null)
                {
                    _socketClient = new SocketClient();
                }
                return _socketClient;
            }
        }

        void Start()
        {
            InitNet();
        }

        public void InitNet()
        {
            socketClient.OnRegister();
        }

        /// <summary>
        /// 发送链接请求
        /// </summary>
        public void SendConnect()
        {
            Debug.Log("-----发送链接请求-----");
            socketClient.SendConnect();
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void OnRemove()
        {
            socketClient.OnRemove();
        }

        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(ByteBuffer buffer)
        {
            socketClient.SendMessage(buffer);
        }

        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(eProtocalCommand _protocalType, object obj)
        {
            ByteBuffer buff = new ByteBuffer();
            SocketModelRequest smq = new SocketModelRequest();
            smq.messageId = System.DateTime.Now.ToString();
            smq.cmd = (int)_protocalType;
            smq.deviceId = SystemInfo.deviceName;
            
            MemoryStream ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, obj);
            byte[] data = ms.ToArray();
            smq.data = data;

            MemoryStream reqMS = new MemoryStream();
            ProtoBuf.Serializer.Serialize(reqMS, smq);
            byte[] result = reqMS.ToArray();
            buff.WriteBytes(result);
            SendMessage(buff);
        }

        /// <summary>
        /// 连接 
        /// </summary>
        public void OnConnect()
        {
            Debug.Log("======连接========");
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void OnDisConnect()
        {
            Debug.Log("======断开连接========");
        }

        /// <summary>
        /// 派发协议
        /// </summary>
        /// <param name="protoId"></param>
        /// <param name="buff"></param>
        public void DispatchProto(int protoId, byte[] data)
        {
            lock (MessageCenter.Instance._netMessageDataQueue)
            {
                MessageCenter.Instance._netMessageDataQueue.Enqueue(new KeyValuePair<eProtocalCommand, byte[]>((eProtocalCommand)protoId, data));
            }
        }
        public void DispatchProto(MemoryStream ms)
        {
            BinaryReader r = new BinaryReader(ms);
            byte[] message = r.ReadBytes((int)(ms.Length - ms.Position));
            //ByteBuffer buffer = new ByteBuffer(message);
            SocketModelResponse smr = ProtoBuf.Serializer.Deserialize(typeof(SocketModelResponse), new MemoryStream(message)) as SocketModelResponse;
            switch (smr.resultStatus)
            {
                case (int)eResultOutStatus.MSG_SUCCESS:
                    {
                        DispatchProto(smr.cmd, smr.data);
                    }
                    break;
                default:
                    Debug.LogError(smr.resultDescription);
                    break;
            }
        }

        protected override void OnDestroy()
        {
            OnRemove();
        }
    }
}
