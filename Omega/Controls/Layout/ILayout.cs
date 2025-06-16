using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// Base interface for all layout controls in Omega
/// </summary>
public interface ILayout
{
    /// <summary>
    /// Gets or sets the padding around the layout
    /// </summary>
    Thickness Padding { get; set; }

    /// <summary>
    /// Gets or sets the background color of the layout
    /// </summary>
    Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets whether the layout is enabled
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the layout
    /// </summary>
    double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the layout
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Gets the collection of child elements in the layout
    /// </summary>
    IList<IView> Children { get; }

    /// <summary>
    /// Invalidates the layout and forces a new layout pass
    /// </summary>
    void Invalidate();
}