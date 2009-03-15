#region License

//
// Author: Ian Davis <ian.f.davis@gmail.com>
// Copyright (c) 2007-2009, Ian Davis
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

#region Using Directives

using System.Diagnostics.CodeAnalysis;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Infrastructure;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Behavior
{
    /// <summary>
    /// Wraps the default behavior using in object activation. To all callers it is the same object,
    /// but the Resolve and Release members change the creation context befor sending the base behavior
    /// on its way.
    /// </summary>
    [SuppressMessage( "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly" )]
    internal class BehaviorContextDecorator : IBehavior
    {
        private readonly IBehavior _baseBehavior;
        private readonly SynchronizeAttribute _syncAttribute;

        /// <summary>
        /// Creates a new <see cref="BehaviorContextDecorator"/> instance.
        /// </summary>
        /// <param name="behavior">The base behavior to decorate.</param>
        /// <param name="synchronizeAttribute">The creation context of the base behavior.</param>
        public BehaviorContextDecorator( IBehavior behavior, SynchronizeAttribute synchronizeAttribute )
        {
            Ensure.ArgumentNotNull( behavior, "behavior" );
            Ensure.ArgumentNotNull(synchronizeAttribute, "synchronizeAttribute");
            _baseBehavior = behavior;
            _syncAttribute = synchronizeAttribute;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        [SuppressMessage( "Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly" )]
        [SuppressMessage( "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly" )]
        public void Dispose()
        {
            _baseBehavior.Dispose();
        }

        #endregion

        #region Implementation of IBehavior

        /// <summary>
        /// Gets or sets the kernel related to the behavior.
        /// </summary>
        public IKernel Kernel
        {
            get { return _baseBehavior.Kernel; }
            set { _baseBehavior.Kernel = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the behavior supports eager activation.
        /// </summary>
        /// <remarks>
        /// If <see langword="true"/>, instances of the associated type will be automatically
        /// activated if the <c>UseEagerActivation</c> option is set for the kernel. If
        /// <see langword="false"/>, all instances of the type will be lazily activated.
        /// </remarks>
        public bool SupportsEagerActivation
        {
            get { return _baseBehavior.SupportsEagerActivation; }
        }

        /// <summary>
        /// Gets a value indicating whether the kernel should track instances created by the
        /// behavior for deterministic disposal.
        /// </summary>
        /// <remarks>
        /// If <see langword="true"/>, the kernel will keep a reference to each instance of
        /// the associated type that is activated. When the kernel is disposed, the instances
        /// will be released.
        /// </remarks>
        public bool ShouldTrackInstances
        {
            get { return _baseBehavior.ShouldTrackInstances; }
        }

        /// <summary>
        /// Resolves an instance of the type based on the rules of the behavior.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <returns>An instance of the type associated with the behavior.</returns>
        public object Resolve( IContext context )
        {
            // We want to make sure that objects are created in the contex which they have stated. 
            // For example, all controls, forms, etc will be created in the WindowsformsSychronizationContext.
            // This ensures that all calls are made on the same context in which the object was created.
            using (new ContextScope(_syncAttribute.SynchronizationContext, context.Kernel))
            {
                return _baseBehavior.Resolve( context );
            }
        }

        /// <summary>
        /// Releases the instance of the type contained in the context based on the rules of the behavior.
        /// </summary>
        /// <param name="context">The activation context.</param>
        public void Release( IContext context )
        {
            // We want to restore the context during teardown of the object. This is mainly needed for controls.
            using ( new ContextScope( _syncAttribute.SynchronizationContext, context.Kernel ) )
            {
                _baseBehavior.Release( context );
            }
        }

        #endregion
    }
}