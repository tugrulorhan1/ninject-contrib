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
using System.Threading;
using System.Windows.Forms;
using Ninject.Core.Interception;
using NinjectContrib.Synchronization.Properties;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Interception
{
    internal class StandardSynchronizationInterceptor : SynchronizationInterceptorBase
    {
        public StandardSynchronizationInterceptor( SynchronizeAttribute attribute )
        {
            SynchronizationAttribute = attribute;
        }

        #region Overrides of SynchronizationInterceptorBase

        protected override void MarshalInvoke( IInvocation invocation )
        {
            if ( !InvokeUsingSynchronizationContext( invocation ) &&
                 !InvokeUsingImplicitSynchronization( invocation ) )
            {
                invocation.Proceed();
            }
        }

        #endregion

        public SynchronizeAttribute SynchronizationAttribute { get; set; }

        private bool InvokeUsingImplicitSynchronization( IInvocation invocation )
        {
            if ( SynchronizationAttribute.SynchronizationProxyType != SynchronizationProxyType.ISynchronizeInvoke )
            {
                return false;
            }

            if ( !( invocation.Request.Target is ISynchronizeInvoke ) )
            {
                throw new InvalidOperationException( Resources.ISyncInvokeMismatchError );
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

            return true;
        }

        private bool InvokeUsingSynchronizationContext( IInvocation invocation )
        {
            if ( SynchronizationAttribute.SynchronizationProxyType != SynchronizationProxyType.SynchronizationContext )
            {
                return false;
            }

            // If we are already in the target context, the context will not be switched.
            // We don't need to do and check for current context and call invocation.Proceed separately.
            using ( new ContextScope( SynchronizationAttribute.SynchronizationContext, invocation.Request.Kernel ) )
            {
                SynchronizationAttribute.SynchronizationContext.Send(state => invocation.Proceed(), null);
            }

            return true;
        }
    }
}