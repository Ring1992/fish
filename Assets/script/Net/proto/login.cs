//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: login.proto
namespace protobuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginRequest")]
  public partial class LoginRequest : global::ProtoBuf.IExtensible
  {
    public LoginRequest() {}
    
    private string _userName;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    private string _password;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginResponse")]
  public partial class LoginResponse : global::ProtoBuf.IExtensible
  {
    public LoginResponse() {}
    
    private string _userName;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    private string _token;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"token", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string token
    {
      get { return _token; }
      set { _token = value; }
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
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}