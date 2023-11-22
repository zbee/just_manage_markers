using System;

namespace JustManageMarkers.CommonGUI;

public class WantsReferenceException : Exception
{
    public WantsReferenceException(string message = "") : base(message)
    {
    }
}
