/**
 * Autogenerated by Thrift Compiler (1.0.0-dev)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace FusionMiner.Thrift
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class NetworkConfig : TBase
  {
    private bool _Enabled;
    private bool _DHCP;
    private string _IP;
    private string _SubnetMask;
    private string _Router;
    private string _DNS1;
    private string _DNS2;
    private string _SSID;
    private string _Security;
    private string _Password;

    public bool Enabled
    {
      get
      {
        return _Enabled;
      }
      set
      {
        __isset.Enabled = true;
        this._Enabled = value;
      }
    }

    public bool DHCP
    {
      get
      {
        return _DHCP;
      }
      set
      {
        __isset.DHCP = true;
        this._DHCP = value;
      }
    }

    public string IP
    {
      get
      {
        return _IP;
      }
      set
      {
        __isset.IP = true;
        this._IP = value;
      }
    }

    public string SubnetMask
    {
      get
      {
        return _SubnetMask;
      }
      set
      {
        __isset.SubnetMask = true;
        this._SubnetMask = value;
      }
    }

    public string Router
    {
      get
      {
        return _Router;
      }
      set
      {
        __isset.Router = true;
        this._Router = value;
      }
    }

    public string DNS1
    {
      get
      {
        return _DNS1;
      }
      set
      {
        __isset.DNS1 = true;
        this._DNS1 = value;
      }
    }

    public string DNS2
    {
      get
      {
        return _DNS2;
      }
      set
      {
        __isset.DNS2 = true;
        this._DNS2 = value;
      }
    }

    public string SSID
    {
      get
      {
        return _SSID;
      }
      set
      {
        __isset.SSID = true;
        this._SSID = value;
      }
    }

    public string Security
    {
      get
      {
        return _Security;
      }
      set
      {
        __isset.Security = true;
        this._Security = value;
      }
    }

    public string Password
    {
      get
      {
        return _Password;
      }
      set
      {
        __isset.Password = true;
        this._Password = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Enabled;
      public bool DHCP;
      public bool IP;
      public bool SubnetMask;
      public bool Router;
      public bool DNS1;
      public bool DNS2;
      public bool SSID;
      public bool Security;
      public bool Password;
    }

    public NetworkConfig() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.Bool) {
              Enabled = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Bool) {
              DHCP = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              IP = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              SubnetMask = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              Router = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              DNS1 = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              DNS2 = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.String) {
              SSID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.String) {
              Security = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              Password = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("NetworkConfig");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.Enabled) {
        field.Name = "Enabled";
        field.Type = TType.Bool;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Enabled);
        oprot.WriteFieldEnd();
      }
      if (__isset.DHCP) {
        field.Name = "DHCP";
        field.Type = TType.Bool;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(DHCP);
        oprot.WriteFieldEnd();
      }
      if (IP != null && __isset.IP) {
        field.Name = "IP";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(IP);
        oprot.WriteFieldEnd();
      }
      if (SubnetMask != null && __isset.SubnetMask) {
        field.Name = "SubnetMask";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SubnetMask);
        oprot.WriteFieldEnd();
      }
      if (Router != null && __isset.Router) {
        field.Name = "Router";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Router);
        oprot.WriteFieldEnd();
      }
      if (DNS1 != null && __isset.DNS1) {
        field.Name = "DNS1";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(DNS1);
        oprot.WriteFieldEnd();
      }
      if (DNS2 != null && __isset.DNS2) {
        field.Name = "DNS2";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(DNS2);
        oprot.WriteFieldEnd();
      }
      if (SSID != null && __isset.SSID) {
        field.Name = "SSID";
        field.Type = TType.String;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SSID);
        oprot.WriteFieldEnd();
      }
      if (Security != null && __isset.Security) {
        field.Name = "Security";
        field.Type = TType.String;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Security);
        oprot.WriteFieldEnd();
      }
      if (Password != null && __isset.Password) {
        field.Name = "Password";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Password);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("NetworkConfig(");
      bool __first = true;
      if (__isset.Enabled) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Enabled: ");
        __sb.Append(Enabled);
      }
      if (__isset.DHCP) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DHCP: ");
        __sb.Append(DHCP);
      }
      if (IP != null && __isset.IP) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IP: ");
        __sb.Append(IP);
      }
      if (SubnetMask != null && __isset.SubnetMask) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SubnetMask: ");
        __sb.Append(SubnetMask);
      }
      if (Router != null && __isset.Router) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Router: ");
        __sb.Append(Router);
      }
      if (DNS1 != null && __isset.DNS1) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DNS1: ");
        __sb.Append(DNS1);
      }
      if (DNS2 != null && __isset.DNS2) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DNS2: ");
        __sb.Append(DNS2);
      }
      if (SSID != null && __isset.SSID) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SSID: ");
        __sb.Append(SSID);
      }
      if (Security != null && __isset.Security) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Security: ");
        __sb.Append(Security);
      }
      if (Password != null && __isset.Password) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Password: ");
        __sb.Append(Password);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
