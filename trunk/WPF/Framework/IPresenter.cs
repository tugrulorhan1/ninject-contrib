namespace Ninject.Framework.PresentationFoundation
{
    /// <summary>
    /// An abstract definition of a presenter.
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Injects the view into the presenter.
        /// </summary>
        /// <param name="view">The view to associate with the presenter.</param>
        void SetView(IView view);
    }
}