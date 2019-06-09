#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class AkSpatialAudioInitSettings : global::System.IDisposable {
  private global::System.IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkSpatialAudioInitSettings(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static global::System.IntPtr getCPtr(AkSpatialAudioInitSettings obj) {
    return (obj == null) ? global::System.IntPtr.Zero : obj.swigCPtr;
  }

  internal virtual void setCPtr(global::System.IntPtr cPtr) {
    Dispose();
    swigCPtr = cPtr;
  }

  ~AkSpatialAudioInitSettings() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkSpatialAudioInitSettings(swigCPtr);
        }
        swigCPtr = global::System.IntPtr.Zero;
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public AkSpatialAudioInitSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkSpatialAudioInitSettings(), true) {
  }

  public int uPoolID { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolID_get(swigCPtr); } 
  }

  public uint uPoolSize { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uPoolSize_get(swigCPtr); } 
  }

  public uint uMaxSoundPropagationDepth { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_get(swigCPtr); } 
  }

  public uint uDiffractionFlags { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_get(swigCPtr); } 
  }

  public float fDiffractionShadowAttenFactor { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_get(swigCPtr); } 
  }

  public float fDiffractionShadowDegrees { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_get(swigCPtr); } 
  }

  public float fMovementThreshold { set { AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fMovementThreshold_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkSpatialAudioInitSettings_fMovementThreshold_get(swigCPtr); } 
  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.