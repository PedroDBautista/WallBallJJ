using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalHandler : MonoBehaviour
{

    [SerializeField] protected List<SignalHandler> listeners;

    public virtual void ReceiveSignal(string signal)
    {

    }
    protected void SendSignal(string signal)
    {
        for(int i = 0; i < listeners.Count; i++)
        {
            listeners[i].ReceiveSignal(signal);
        }
    }

    public void AddSignalListener(SignalHandler listener)
    {
        listeners.Add(listener);
    }

    public void RemoveSignalListener(SignalHandler listener)
    {
        if(listeners.Contains(listener)) listeners.Remove(listener);
    }
}
