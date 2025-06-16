using Microsoft.Maui.Graphics;

namespace Omega.Controls.Layout;

/// <summary>
/// Interface for layout managers that handle measurement and arrangement of child elements
/// </summary>
public interface ILayoutManager
{
    /// <summary>
    /// Measures the layout and its children
    /// </summary>
    /// <param name="widthConstraint">The maximum width constraint</param>
    /// <param name="heightConstraint">The maximum height constraint</param>
    /// <returns>The desired size of the layout</returns>
    Size Measure(double widthConstraint, double heightConstraint);

    /// <summary>
    /// Arranges the children within the given bounds
    /// </summary>
    /// <param name="bounds">The bounds in which to arrange the children</param>
    /// <returns>The actual size used for arrangement</returns>
    Size ArrangeChildren(Rect bounds);
}