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

using System.Diagnostics;
using System.Threading;
using Ninject.Core;
using Ninject.Core.Interception;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Interception
{
    [Transient]
    internal class SynchronizationContextInterceptor : SynchronizationInterceptorBase
    {
        public SynchronizationContextInterceptor( SynchronizeAttribute attribute )
        {
            SyncAttribute = attribute;
        }

        public SynchronizeAttribute SyncAttribute { get; set; }

        /// <summary>
        /// Marshals the invocation onto the proper synchronization context.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        protected override void MarshalInvoke( IInvocation invocation )
        {
            SynchronizationContext activeSyncContext = SynchronizationContext.Current;
            if ( activeSyncContext == null )
            {
                SynchronizationContext.SetSynchronizationContext( new SynchronizationContext() );
                Debug.Assert( SynchronizationContext.Current != null );
                activeSyncContext = SynchronizationContext.Current;
            }

            if ( SyncAttribute.SynchronizationContext != activeSyncContext )
            {
                SynchronizationContext prevSyncContext = activeSyncContext;

                try
                {
                    SynchronizationContext.SetSynchronizationContext( SyncAttribute.SynchronizationContext );

                    SyncAttribute.SynchronizationContext.Send( state => invocation.Proceed(), null );
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext( prevSyncContext );
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}