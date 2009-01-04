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
    [SuppressMessage( "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly" )]
    public class BehaviorContextDecorator : IBehavior
    {
        private readonly IBehavior _behavior;
        private readonly SynchronizeAttribute _syncAttribute;

        public BehaviorContextDecorator( IBehavior behavior, SynchronizeAttribute synchronizeAttribute )
        {
            Ensure.ArgumentNotNull( behavior, "behavior" );
            Ensure.ArgumentNotNull( behavior, "synchronizeAttribute" );
            _behavior = behavior;
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
            _behavior.Dispose();
        }

        #endregion

        #region Implementation of IBehavior

        /// <summary>
        /// Gets or sets the kernel related to the behavior.
        /// </summary>
        public IKernel Kernel
        {
            get { return _behavior.Kernel; }
            set { _behavior.Kernel = value; }
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
            get { return _behavior.SupportsEagerActivation; }
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
            get { return _behavior.ShouldTrackInstances; }
        }

        /// <summary>
        /// Resolves an instance of the type based on the rules of the behavior.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <returns>An instance of the type associated with the behavior.</returns>
        public object Resolve( IContext context )
        {
            using ( new ContextScope( _syncAttribute.SynchronizationContext, Kernel ) )
            {
                return _behavior.Resolve( context );
            }
        }

        /// <summary>
        /// Releases the instance of the type contained in the context based on the rules of the behavior.
        /// </summary>
        /// <param name="context">The activation context.</param>
        public void Release( IContext context )
        {
            using ( new ContextScope( _syncAttribute.SynchronizationContext, Kernel ) )
            {
                _behavior.Release( context );
            }
        }

        #endregion
    }
}