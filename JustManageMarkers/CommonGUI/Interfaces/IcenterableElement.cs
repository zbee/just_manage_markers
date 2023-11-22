using System.Diagnostics.CodeAnalysis;

namespace JustManageMarkers.CommonGUI.Interfaces;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IcenterableElement
{
    protected bool _centered { get; set; }
}
