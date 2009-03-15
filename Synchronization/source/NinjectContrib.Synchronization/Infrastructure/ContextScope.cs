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

using System;
using System.Diagnostics;
using System.Threading;
using Ninject.Core;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure
{
    internal class ContextScope : IDisposable
    {
        private IKernel _kernel;

        public ContextScope( SynchronizationContext target, IKernel kernel )
        {
            _kernel = kernel;
            OriginalContext = SetCreationContext( target );
        }

        private SynchronizationContext OriginalContext { get; set; }

        /// <summary>
        /// Gets the current <see cref="SynchronizationContext"/>. If there is no current context, a new context
        /// will be created and set.
        /// </summary>
        /// <returns>The current <see cref="SynchronizationContext"/>.</returns>
        public static SynchronizationContext GetCurrentContext(IKernel kernel)
        {
            
                if ( SynchronizationContext.Current == null )
                {
                    SynchronizationContext.SetSynchronizationContext( kernel.Get<SynchronizationContext>() );
                }
                Debug.Assert( SynchronizationContext.Current != null );
                return SynchronizationContext.Current;
            
        }

        /// <summary>
        /// Restores the current context to that of the target if the current context is different than that of the target.
        /// </summary>
        /// <param name="target">The context desired to be current.</param>
        public void RestoreCreationContext( SynchronizationContext target )
        {
            if ( GetCurrentContext(_kernel) != target)
            {
                SynchronizationContext.SetSynchronizationContext( target );
            }
        }

        /// <summary>
        /// Sets the current <see cref="SynchronizationContext"/> to the target <see cref="SynchronizationContext"/> 
        /// and returns the <see cref="SynchronizationContext"/> that was active prior to the switch.
        /// </summary>
        /// <param name="target">The context desired to be current.</param>
        /// <returns>The <see cref="SynchronizationContext"/> that was active prior to the target being set.</returns>
        public SynchronizationContext SetCreationContext( SynchronizationContext target )
        {
            // Get the current context, creating it if needed.
            SynchronizationContext original = GetCurrentContext(_kernel);

            bool hashesEqual = target.GetHashCode() == original.GetHashCode();
            Console.WriteLine("Target: {0}\tOriginal: {1}", target.GetHashCode(), original.GetHashCode());

            if ( target != original )
            {
                SynchronizationContext.SetSynchronizationContext( target );
            }

            return original;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        void IDisposable.Dispose()
        {
            RestoreCreationContext( OriginalContext );
            _kernel = null;
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}