using System;

namespace CSLA.Core 
{
  /// <summary>
  /// This base class declares the IsDirtyChanged event
  /// to be NonSerialized so serialization will work.
  /// </summary>
  [Serializable()]
  public abstract class BindableBase
  {
    [NonSerialized]
    EventHandler _nonSerializableHandlers;
    EventHandler _serializableHandlers;

    /// <summary>
    /// Implements a serialization-safe IsDirtyChanged event.
    /// </summary>
    public event EventHandler IsDirtyChanged
    {
      add
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          _serializableHandlers = (EventHandler)Delegate.Combine(_serializableHandlers, value);
        else
          _nonSerializableHandlers = (EventHandler)Delegate.Combine(_nonSerializableHandlers, value);
      }
      remove
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          _serializableHandlers = (EventHandler)Delegate.Remove(_serializableHandlers, value);
        else
          _nonSerializableHandlers = (EventHandler)Delegate.Remove(_nonSerializableHandlers, value);
      }
    }

    /// <summary>
    /// Call this method to raise the IsDirtyChanged event.
    /// </summary>
    virtual protected void OnIsDirtyChanged()
    {
      if (_nonSerializableHandlers != null)
        _nonSerializableHandlers(this, EventArgs.Empty);
      if (_serializableHandlers != null)
        _serializableHandlers(this, EventArgs.Empty);
    }
  }
}