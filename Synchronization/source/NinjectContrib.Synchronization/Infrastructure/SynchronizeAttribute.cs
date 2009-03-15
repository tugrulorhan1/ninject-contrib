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
using System.Threading;
using Ninject.Core.Infrastructure;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
    public sealed class SynchronizeAttribute : Attribute
    {
        public SynchronizeAttribute()
            : this( SynchronizationProxyType.Undefined )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeAttribute"/> class.
        /// </summary>
        public SynchronizeAttribute( SynchronizationProxyType synchronizationProxyType )
        {
            SynchronizationProxyType = synchronizationProxyType;
            Order = 0;
            //SynchronizationContext = ContextScope.CurrentContext;
            ContextType = null;
        }

        public SynchronizeAttribute( Type contextType )
            : this( SynchronizationProxyType.SynchronizationContext )
        {
            Ensure.ArgumentNotNull( contextType, "contextType" );

            if ( !typeof (SynchronizationContext).IsAssignableFrom( contextType ) )
            {
                throw new ArgumentOutOfRangeException( "contextType", contextType,
                                                       "The synchronization type must be assignable to SynchronizationContext." );
            }

            ContextType = contextType;
        }

        public SynchronizationProxyType SynchronizationProxyType { get; internal set; }

        /// <summary>
        /// Gets or sets the interceptor's order number. Interceptors are invoked in ascending order.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Gets or sets the synchronization context.
        /// </summary>
        /// <value>The synchronization context.</value>
        public SynchronizationContext SynchronizationContext { get; internal set; }

        public Type ContextType { get; private set; }
    }
}