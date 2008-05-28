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
using Ninject.Core.Infrastructure;
using Ninject.Core.Planning;
using Ninject.Extensions.Synchronization.Infrastructure.Strategies;

#endregion

namespace Ninject.Extensions.Synchronization.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public class SynchronizationExtension : KernelComponentBase
    {
        /// <summary>
        /// Called when the component is connected to its environment.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnConnected(EventArgs args)
        {
            Kernel.Components.Get<IPlanner>().Strategies.Append(new MethodSynchronizationStrategy());

            base.OnConnected(args);
        }

        /// <summary>
        /// Called when the component is disconnected from its environment.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnDisconnected(EventArgs args)
        {
            Kernel.Components.Get<IPlanner>().Strategies.RemoveAll<MethodSynchronizationStrategy>();

            base.OnDisconnected(args);
        }
    }
}