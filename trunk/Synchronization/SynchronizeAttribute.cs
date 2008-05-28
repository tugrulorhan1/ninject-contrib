#region License

//
// Author: Ian Davis <ian.f.davis@gmail.com>
// Copyright (c) 2007-2008, Ian Davis
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
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Extensions.Synchronization.Infrastructure;

#endregion

namespace Ninject.Extensions.Synchronization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SynchronizeAttribute : InterceptAttribute
    {
        private SynchronizationContext _synchronizationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeAttribute"/> class.
        /// </summary>
        public SynchronizeAttribute()
        {
            SynchronizationContext = CustomSyncContext.Instance;
        }

        /// <summary>
        /// Gets or sets the synchronization context.
        /// </summary>
        /// <value>The synchronization context.</value>
        public SynchronizationContext SynchronizationContext
        {
            get { return _synchronizationContext; }
            set { _synchronizationContext = value; }
        }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor(IRequest request)
        {
            return request.Kernel.Get<SynchronizationInterceptor>();
        }

        #region Nested type: CustomSyncContext

        /// <summary>
        /// Dummy context for early testing.
        /// </summary>
        private class CustomSyncContext : SynchronizationContext
        {
            private static readonly SynchronizationContext _instance = new CustomSyncContext();

            private CustomSyncContext()
            {
            }

            /// <summary>
            /// Gets the instance.
            /// </summary>
            /// <value>The instance.</value>
            public static SynchronizationContext Instance
            {
                get { return _instance; }
            }
        }

        #endregion
    }
}