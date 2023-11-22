using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace JustManageMarkers.CommonGUI.Interfaces;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface IguiElementCollection
{
    protected List<IguiElement> _elements { get; set; }

    void draw();

    void draw(ref int refInt1);

    void draw(ref int refInt1, ref int refInt2);
}
