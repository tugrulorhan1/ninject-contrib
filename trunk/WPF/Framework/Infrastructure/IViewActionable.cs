/// <summary>
/// Defines the property Action on a WPF view/presenter publisher/subscriber attribute.
/// </summary>
public interface IViewActionable
{
    /// <summary>
    /// Gets the name of the channel to subscribe to.
    /// </summary>
    /// <value>The action.</value>
    string Action { get; }
}