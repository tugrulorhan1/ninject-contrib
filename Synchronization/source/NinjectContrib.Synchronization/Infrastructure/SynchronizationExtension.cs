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
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning;
using NinjectContrib.Synchronization.Infrastructure.Activation;
using NinjectContrib.Synchronization.Infrastructure.Planning;
using NinjectContrib.Synchronization.Properties;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure
{
    internal class SynchronizationExtension : KernelComponentBase
    {
        private static readonly MarshalingControl _marshallingControl = new MarshalingControl();

        public SynchronizationExtension()
        {
            if ( _marshallingControl.Handle == IntPtr.Zero )
            {
                // handle was not created.
                throw new ActivationException( Resources.ModuleMarshalingControlActivationError );
            }
        }

        /// <summary>
        /// Called when the component is connected to its environment.           
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnConnected( EventArgs args )
        {
            Kernel.Components.Get<IPlanner>().Strategies.Append( new MethodSynchronizationRegistrationStrategy() );
            Kernel.Components.Get<IActivator>().Strategies.Append( new SynchronizationActivator() );

            base.OnConnected( args );
        }

        /// <summary>
        /// Called when the component is disconnected from its environment.        
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnDisconnected( EventArgs args )
        {
            Kernel.Components.Get<IPlanner>().Strategies.RemoveAll<MethodSynchronizationRegistrationStrategy>();
            Kernel.Components.Get<IActivator>().Strategies.RemoveAll<SynchronizationActivator>();

            base.OnDisconnected( args );
        }
    }
}