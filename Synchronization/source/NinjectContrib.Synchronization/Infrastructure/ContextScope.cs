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
        public ContextScope( SynchronizationContext target, IKernel kernel )
        {
            OriginalContext = SetCreationContext( target, kernel );
        }

        private SynchronizationContext OriginalContext { get; set; }

        public static SynchronizationContext GetCurrentContext()
        {
            if ( SynchronizationContext.Current == null )
            {
                SynchronizationContext.SetSynchronizationContext( new SynchronizationContext() );
            }
            Debug.Assert( SynchronizationContext.Current != null );
            return SynchronizationContext.Current;
        }

        public static void RestoreCreationContext( SynchronizationContext original )
        {
            if ( SynchronizationContext.Current != original )
            {
                SynchronizationContext.SetSynchronizationContext( original );
            }
        }

        public static SynchronizationContext SetCreationContext( SynchronizationContext target, IKernel kernel )
        {
            // First, if there is no context, create one.
            SynchronizationContext original = SynchronizationContext.Current;
            if ( original == null )
            {
                SynchronizationContext.SetSynchronizationContext( kernel.Get<SynchronizationContext>() );
                original = SynchronizationContext.Current;
            }

            Debug.Assert( original != null );

            // Now that there is a context, see if we need to change contexts.
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
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}