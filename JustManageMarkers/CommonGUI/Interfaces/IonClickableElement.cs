using System;
using System.Diagnostics.CodeAnalysis;

namespace JustManageMarkers.CommonGUI.Interfaces;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IonClickableElement
{
    protected Action? _callback { get; set; }
}
