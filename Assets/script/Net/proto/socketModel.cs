//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: socketModel.proto
namespace protobuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SocketModelRequest")]
  public partial class SocketModelRequest : global::ProtoBuf.IExtensible
  {
    public SocketModelRequest() {}
    
    private string _messageId;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"messageId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string messageId
    {
      get { return _messageId; }
      set { _messageId = value; }
    }
    private int _cmd;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"cmd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int cmd
    {
      get { return _cmd; }
      set { _cmd = value; }
    }
    private string _deviceId;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"deviceId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string deviceId
    {
      get { return _deviceId; }
      set { _deviceId = value; }
    }
    private byte[] _data;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SocketModelResponse")]
  public partial class SocketModelResponse : global::ProtoBuf.IExtensible
  {
    public SocketModelResponse() {}
    
    private string _messageId;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"messageId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string messageId
    {
      get { return _messageId; }
      set { _messageId = value; }
    }
    private int _cmd;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"cmd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int cmd
    {
      get { return _cmd; }
      set { _cmd = value; }
    }
    private int _resultStatus;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"resultStatus", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int resultStatus
    {
      get { return _resultStatus; }
      set { _resultStatus = value; }
    }
    private string _resultDescription;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"resultDescription", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string resultDescription
    {
      get { return _resultDescription; }
      set { _resultDescription = value; }
    }
    private byte[] _data;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}