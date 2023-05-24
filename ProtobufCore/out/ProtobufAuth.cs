// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protobuf_Auth.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace AxibugProtobuf {

  /// <summary>Holder for reflection information generated from protobuf_Auth.proto</summary>
  public static partial class ProtobufAuthReflection {

    #region Descriptor
    /// <summary>File descriptor for protobuf_Auth.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtobufAuthReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNwcm90b2J1Zl9BdXRoLnByb3RvEg5BeGlidWdQcm90b2J1ZiKRAQoOUHJv",
            "dG9idWZfTG9naW4SLAoJbG9naW5UeXBlGAEgASgOMhkuQXhpYnVnUHJvdG9i",
            "dWYuTG9naW5UeXBlEi4KCmRldmljZVR5cGUYAiABKA4yGi5BeGlidWdQcm90",
            "b2J1Zi5EZXZpY2VUeXBlEg8KB0FjY291bnQYAyABKAkSEAoIUGFzc3dvcmQY",
            "BCABKAkifwoTUHJvdG9idWZfTG9naW5fUkVTUBINCgVUb2tlbhgBIAEoCRIV",
            "Cg1MYXN0TG9naW5EYXRlGAIgASgJEg8KB1JlZ0RhdGUYAyABKAkSMQoGU3Rh",
            "dHVzGAQgASgOMiEuQXhpYnVnUHJvdG9idWYuTG9naW5SZXN1bHRTdGF0dXMq",
            "KwoJQ29tbWFuZElEEg4KCkNNRF9ERUZBVUwQABIOCglDTURfTE9HSU4Q0Q8q",
            "KwoJRXJyb3JDb2RlEhAKDEVSUk9SX0RFRkFVTBAAEgwKCEVSUk9SX09LEAEq",
            "PgoJTG9naW5UeXBlEg8KC0Jhc2VEZWZhdWx0EAASDgoKSGFvWXVlQXV0aBAB",
            "EgcKA0JGMxADEgcKA0JGNBAEKksKCkRldmljZVR5cGUSFgoSRGV2aWNlVHlw",
            "ZV9EZWZhdWx0EAASBgoCUEMQARILCgdBbmRyb2lkEAISBwoDSU9TEAMSBwoD",
            "UFNWEAQqTgoRTG9naW5SZXN1bHRTdGF0dXMSIQodTG9naW5SZXN1bHRTdGF0",
            "dXNfQmFzZURlZmF1bHQQABIGCgJPSxABEg4KCkFjY291bnRFcnIQAkICSAFi",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::AxibugProtobuf.CommandID), typeof(global::AxibugProtobuf.ErrorCode), typeof(global::AxibugProtobuf.LoginType), typeof(global::AxibugProtobuf.DeviceType), typeof(global::AxibugProtobuf.LoginResultStatus), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::AxibugProtobuf.Protobuf_Login), global::AxibugProtobuf.Protobuf_Login.Parser, new[]{ "LoginType", "DeviceType", "Account", "Password" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::AxibugProtobuf.Protobuf_Login_RESP), global::AxibugProtobuf.Protobuf_Login_RESP.Parser, new[]{ "Token", "LastLoginDate", "RegDate", "Status" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum CommandID {
    /// <summary>
    ///缺省不使用
    /// </summary>
    [pbr::OriginalName("CMD_DEFAUL")] CmdDefaul = 0,
    /// <summary>
    ///登录上行 | 下行 对应 Protobuf_Login | Protobuf_Login_RESP
    /// </summary>
    [pbr::OriginalName("CMD_LOGIN")] CmdLogin = 2001,
  }

  public enum ErrorCode {
    /// <summary>
    ///缺省不使用
    /// </summary>
    [pbr::OriginalName("ERROR_DEFAUL")] ErrorDefaul = 0,
    /// <summary>
    ///成功
    /// </summary>
    [pbr::OriginalName("ERROR_OK")] ErrorOk = 1,
  }

  public enum LoginType {
    /// <summary>
    ///缺省不使用
    /// </summary>
    [pbr::OriginalName("BaseDefault")] BaseDefault = 0,
    [pbr::OriginalName("HaoYueAuth")] HaoYueAuth = 1,
    [pbr::OriginalName("BF3")] Bf3 = 3,
    [pbr::OriginalName("BF4")] Bf4 = 4,
  }

  public enum DeviceType {
    /// <summary>
    ///缺省不使用
    /// </summary>
    [pbr::OriginalName("DeviceType_Default")] Default = 0,
    [pbr::OriginalName("PC")] Pc = 1,
    [pbr::OriginalName("Android")] Android = 2,
    [pbr::OriginalName("IOS")] Ios = 3,
    [pbr::OriginalName("PSV")] Psv = 4,
  }

  public enum LoginResultStatus {
    /// <summary>
    ///缺省不使用
    /// </summary>
    [pbr::OriginalName("LoginResultStatus_BaseDefault")] BaseDefault = 0,
    [pbr::OriginalName("OK")] Ok = 1,
    [pbr::OriginalName("AccountErr")] AccountErr = 2,
  }

  #endregion

  #region Messages
  /// <summary>
  ///登录数据上行
  /// </summary>
  public sealed partial class Protobuf_Login : pb::IMessage<Protobuf_Login>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Protobuf_Login> _parser = new pb::MessageParser<Protobuf_Login>(() => new Protobuf_Login());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Protobuf_Login> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxibugProtobuf.ProtobufAuthReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login(Protobuf_Login other) : this() {
      loginType_ = other.loginType_;
      deviceType_ = other.deviceType_;
      account_ = other.account_;
      password_ = other.password_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login Clone() {
      return new Protobuf_Login(this);
    }

    /// <summary>Field number for the "loginType" field.</summary>
    public const int LoginTypeFieldNumber = 1;
    private global::AxibugProtobuf.LoginType loginType_ = global::AxibugProtobuf.LoginType.BaseDefault;
    /// <summary>
    ///登录操作类型 [0]皓月通行证 [3] 皓月BF3 [4] 皓月BF4
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::AxibugProtobuf.LoginType LoginType {
      get { return loginType_; }
      set {
        loginType_ = value;
      }
    }

    /// <summary>Field number for the "deviceType" field.</summary>
    public const int DeviceTypeFieldNumber = 2;
    private global::AxibugProtobuf.DeviceType deviceType_ = global::AxibugProtobuf.DeviceType.Default;
    /// <summary>
    ///设备类型 [0]PC [1]AndroidPad预留 [3]IPad预留
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::AxibugProtobuf.DeviceType DeviceType {
      get { return deviceType_; }
      set {
        deviceType_ = value;
      }
    }

    /// <summary>Field number for the "Account" field.</summary>
    public const int AccountFieldNumber = 3;
    private string account_ = "";
    /// <summary>
    ///用户名
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Account {
      get { return account_; }
      set {
        account_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Password" field.</summary>
    public const int PasswordFieldNumber = 4;
    private string password_ = "";
    /// <summary>
    ///密码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Password {
      get { return password_; }
      set {
        password_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Protobuf_Login);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Protobuf_Login other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (LoginType != other.LoginType) return false;
      if (DeviceType != other.DeviceType) return false;
      if (Account != other.Account) return false;
      if (Password != other.Password) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (LoginType != global::AxibugProtobuf.LoginType.BaseDefault) hash ^= LoginType.GetHashCode();
      if (DeviceType != global::AxibugProtobuf.DeviceType.Default) hash ^= DeviceType.GetHashCode();
      if (Account.Length != 0) hash ^= Account.GetHashCode();
      if (Password.Length != 0) hash ^= Password.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (LoginType != global::AxibugProtobuf.LoginType.BaseDefault) {
        output.WriteRawTag(8);
        output.WriteEnum((int) LoginType);
      }
      if (DeviceType != global::AxibugProtobuf.DeviceType.Default) {
        output.WriteRawTag(16);
        output.WriteEnum((int) DeviceType);
      }
      if (Account.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Account);
      }
      if (Password.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Password);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (LoginType != global::AxibugProtobuf.LoginType.BaseDefault) {
        output.WriteRawTag(8);
        output.WriteEnum((int) LoginType);
      }
      if (DeviceType != global::AxibugProtobuf.DeviceType.Default) {
        output.WriteRawTag(16);
        output.WriteEnum((int) DeviceType);
      }
      if (Account.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Account);
      }
      if (Password.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Password);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (LoginType != global::AxibugProtobuf.LoginType.BaseDefault) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) LoginType);
      }
      if (DeviceType != global::AxibugProtobuf.DeviceType.Default) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) DeviceType);
      }
      if (Account.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Account);
      }
      if (Password.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Password);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Protobuf_Login other) {
      if (other == null) {
        return;
      }
      if (other.LoginType != global::AxibugProtobuf.LoginType.BaseDefault) {
        LoginType = other.LoginType;
      }
      if (other.DeviceType != global::AxibugProtobuf.DeviceType.Default) {
        DeviceType = other.DeviceType;
      }
      if (other.Account.Length != 0) {
        Account = other.Account;
      }
      if (other.Password.Length != 0) {
        Password = other.Password;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            LoginType = (global::AxibugProtobuf.LoginType) input.ReadEnum();
            break;
          }
          case 16: {
            DeviceType = (global::AxibugProtobuf.DeviceType) input.ReadEnum();
            break;
          }
          case 26: {
            Account = input.ReadString();
            break;
          }
          case 34: {
            Password = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            LoginType = (global::AxibugProtobuf.LoginType) input.ReadEnum();
            break;
          }
          case 16: {
            DeviceType = (global::AxibugProtobuf.DeviceType) input.ReadEnum();
            break;
          }
          case 26: {
            Account = input.ReadString();
            break;
          }
          case 34: {
            Password = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  ///登录数据下行
  /// </summary>
  public sealed partial class Protobuf_Login_RESP : pb::IMessage<Protobuf_Login_RESP>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Protobuf_Login_RESP> _parser = new pb::MessageParser<Protobuf_Login_RESP>(() => new Protobuf_Login_RESP());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Protobuf_Login_RESP> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxibugProtobuf.ProtobufAuthReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login_RESP() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login_RESP(Protobuf_Login_RESP other) : this() {
      token_ = other.token_;
      lastLoginDate_ = other.lastLoginDate_;
      regDate_ = other.regDate_;
      status_ = other.status_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_Login_RESP Clone() {
      return new Protobuf_Login_RESP(this);
    }

    /// <summary>Field number for the "Token" field.</summary>
    public const int TokenFieldNumber = 1;
    private string token_ = "";
    /// <summary>
    ///登录凭据 （本次登录之后，所有业务请求凭据，需要存储在内存中）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Token {
      get { return token_; }
      set {
        token_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "LastLoginDate" field.</summary>
    public const int LastLoginDateFieldNumber = 2;
    private string lastLoginDate_ = "";
    /// <summary>
    ///上次登录时间（只用于呈现的字符串，若界面需求需要）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string LastLoginDate {
      get { return lastLoginDate_; }
      set {
        lastLoginDate_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "RegDate" field.</summary>
    public const int RegDateFieldNumber = 3;
    private string regDate_ = "";
    /// <summary>
    ///注册时间（只用于呈现的字符串，若界面需求需要）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RegDate {
      get { return regDate_; }
      set {
        regDate_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Status" field.</summary>
    public const int StatusFieldNumber = 4;
    private global::AxibugProtobuf.LoginResultStatus status_ = global::AxibugProtobuf.LoginResultStatus.BaseDefault;
    /// <summary>
    ///账号状态 （预留） [1]正常[0]被禁封
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::AxibugProtobuf.LoginResultStatus Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Protobuf_Login_RESP);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Protobuf_Login_RESP other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Token != other.Token) return false;
      if (LastLoginDate != other.LastLoginDate) return false;
      if (RegDate != other.RegDate) return false;
      if (Status != other.Status) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Token.Length != 0) hash ^= Token.GetHashCode();
      if (LastLoginDate.Length != 0) hash ^= LastLoginDate.GetHashCode();
      if (RegDate.Length != 0) hash ^= RegDate.GetHashCode();
      if (Status != global::AxibugProtobuf.LoginResultStatus.BaseDefault) hash ^= Status.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Token.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Token);
      }
      if (LastLoginDate.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(LastLoginDate);
      }
      if (RegDate.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(RegDate);
      }
      if (Status != global::AxibugProtobuf.LoginResultStatus.BaseDefault) {
        output.WriteRawTag(32);
        output.WriteEnum((int) Status);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Token.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Token);
      }
      if (LastLoginDate.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(LastLoginDate);
      }
      if (RegDate.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(RegDate);
      }
      if (Status != global::AxibugProtobuf.LoginResultStatus.BaseDefault) {
        output.WriteRawTag(32);
        output.WriteEnum((int) Status);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Token.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Token);
      }
      if (LastLoginDate.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(LastLoginDate);
      }
      if (RegDate.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RegDate);
      }
      if (Status != global::AxibugProtobuf.LoginResultStatus.BaseDefault) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Protobuf_Login_RESP other) {
      if (other == null) {
        return;
      }
      if (other.Token.Length != 0) {
        Token = other.Token;
      }
      if (other.LastLoginDate.Length != 0) {
        LastLoginDate = other.LastLoginDate;
      }
      if (other.RegDate.Length != 0) {
        RegDate = other.RegDate;
      }
      if (other.Status != global::AxibugProtobuf.LoginResultStatus.BaseDefault) {
        Status = other.Status;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Token = input.ReadString();
            break;
          }
          case 18: {
            LastLoginDate = input.ReadString();
            break;
          }
          case 26: {
            RegDate = input.ReadString();
            break;
          }
          case 32: {
            Status = (global::AxibugProtobuf.LoginResultStatus) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Token = input.ReadString();
            break;
          }
          case 18: {
            LastLoginDate = input.ReadString();
            break;
          }
          case 26: {
            RegDate = input.ReadString();
            break;
          }
          case 32: {
            Status = (global::AxibugProtobuf.LoginResultStatus) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
