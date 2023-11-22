using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace JustManageMarkers.CommonGUI.Interfaces;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IbackgroundableElement
{
    protected Vector4? _backgroundColor { get; set; }
}
