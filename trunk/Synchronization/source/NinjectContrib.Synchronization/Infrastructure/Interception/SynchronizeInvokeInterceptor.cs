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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Ninject.Core.Interception;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Interception
{
    internal class SynchronizeInvokeInterceptor : SynchronizationInterceptorBase
    {
        /// <summary>
        /// Marshals the invocation onto the proper synchronization context.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        protected override void MarshalInvoke( IInvocation invocation )
        {
            if ( !( invocation.Request.Target is ISynchronizeInvoke ) )
            {
                throw new InvalidOperationException( "Target must implement ISynchronizeInvoke." );
            }

            var target = invocation.Request.Target as ISynchronizeInvoke;
            Debug.Assert( target != null );
            if ( target.InvokeRequired )
            {
                target.Invoke( (MethodInvoker) ( () => invocation.Proceed() ), null );
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}