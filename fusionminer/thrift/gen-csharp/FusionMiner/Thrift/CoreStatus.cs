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
  public partial class CoreStatus : TBase
  {
    private bool _Available;
    private long _NonceFound;
    private long _NonceNotFound;
    private long _HardwareError;

    public bool Available
    {
      get
      {
        return _Available;
      }
      set
      {
        __isset.Available = true;
        this._Available = value;
      }
    }

    public long NonceFound
    {
      get
      {
        return _NonceFound;
      }
      set
      {
        __isset.NonceFound = true;
        this._NonceFound = value;
      }
    }

    public long NonceNotFound
    {
      get
      {
        return _NonceNotFound;
      }
      set
      {
        __isset.NonceNotFound = true;
        this._NonceNotFound = value;
      }
    }

    public long HardwareError
    {
      get
      {
        return _HardwareError;
      }
      set
      {
        __isset.HardwareError = true;
        this._HardwareError = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Available;
      public bool NonceFound;
      public bool NonceNotFound;
      public bool HardwareError;
    }

    public CoreStatus() {
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
              Available = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              NonceFound = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              NonceNotFound = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              HardwareError = iprot.ReadI64();
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
      TStruct struc = new TStruct("CoreStatus");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.Available) {
        field.Name = "Available";
        field.Type = TType.Bool;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Available);
        oprot.WriteFieldEnd();
      }
      if (__isset.NonceFound) {
        field.Name = "NonceFound";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(NonceFound);
        oprot.WriteFieldEnd();
      }
      if (__isset.NonceNotFound) {
        field.Name = "NonceNotFound";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(NonceNotFound);
        oprot.WriteFieldEnd();
      }
      if (__isset.HardwareError) {
        field.Name = "HardwareError";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(HardwareError);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("CoreStatus(");
      bool __first = true;
      if (__isset.Available) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Available: ");
        __sb.Append(Available);
      }
      if (__isset.NonceFound) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("NonceFound: ");
        __sb.Append(NonceFound);
      }
      if (__isset.NonceNotFound) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("NonceNotFound: ");
        __sb.Append(NonceNotFound);
      }
      if (__isset.HardwareError) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("HardwareError: ");
        __sb.Append(HardwareError);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}