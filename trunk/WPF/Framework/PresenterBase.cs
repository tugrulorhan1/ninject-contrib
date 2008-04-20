using System;
using Ninject.Framework.PresentationFoundation;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Core.Logging;

namespace Ninject.Framework.PresentationFoundation
{
    /// <summary>
    /// The baseline definition of a presenter.
    /// </summary>
    /// <typeparam name="TView">The type of the view that the presenter manages.</typeparam>
    public abstract class PresenterBase<TView> : DisposableObject, IPresenter where TView : class, IView
    {
        private TView _view;

        /// <summary>
        /// Gets or sets the view that the presenter should manage.
        /// </summary>
        public virtual TView View
        {
             get { return _view; }
             set
            {
                if (_view != null)
                    OnViewDisconnected(_view);

                _view = value;

                if(value != null)
                 OnViewConnected(value);
            }
        }

        /// <summary>
        /// Gets or sets the logger associated with the object.
        /// </summary>
        [Inject]
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// Releases all resources currently held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
                View = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when a view is connected to the presenter.
        /// </summary>
        /// <param name="view">The view that was connected.</param>
        protected virtual void OnViewConnected(TView view)
        {
           
        }
        /*----------------------------------------------------------------------------------------*/
        /// <summary>
        /// Called when a view is disconnected from the presenter.
        /// </summary>
        /// <param name="view">The view that was disconnected.</param>
        protected virtual void OnViewDisconnected(TView view)
        {
            
        }


        /// <summary>
        /// Injects the view into the presenter.
        /// </summary>
        /// <param name="view">The view to associate with the presenter.</param>
        void IPresenter.SetView(IView view)
        {
            var strongView = view as TView;

            if (strongView == null)
                throw new ArgumentException("Invalid view", "view");

            View = strongView;
        }
    }
}