using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace JustManageMarkers.CommonGUI.Interfaces;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IcolorableElement
{
    protected Vector4? _textColor { get; set; }
}
