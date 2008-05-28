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
using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Extensions.Synchronization.Infrastructure.Directives;

#endregion

namespace Ninject.Extensions.Synchronization.Infrastructure
{
    [Transient]
    internal class SynchronizationInterceptor : IInterceptor
    {
        public SynchronizationInterceptor()
        {
            Console.WriteLine("SynchronizationInterceptor::.ctor");
        }

        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            BeforeInvoke(invocation);
            MarshalInvoke(invocation);
            AfterInvoke(invocation);
        }

        #endregion

        /// <summary>
        /// Marshals the invocation onto the proper synchronization context.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        private void MarshalInvoke(IInvocation invocation)
        {
            SynchronizationDirective directive =
                invocation.Request.Context.Plan.Directives.GetOne<SynchronizationDirective>();
            directive.SynchronizationContext.Post(delegate { invocation.Proceed(); }, null);
        }

        /// <summary>
        /// Takes some action before the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void BeforeInvoke(IInvocation invocation)
        {
            Console.WriteLine("Intercepting call for: {0}::{1}",
                              invocation.Request.Target.GetType(),
                              invocation.Request.Method);
        }

        /// <summary>
        /// Takes some action after the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void AfterInvoke(IInvocation invocation)
        {
            Console.WriteLine("Intercepted call for: {0}::{1}",
                              invocation.Request.Target.GetType(),
                              invocation.Request.Method);
        }
    }
}