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

using Ninject.Core;
using Ninject.Core.Interception;
using Ninject.Extensions.Synchronization.Infrastructure;

#endregion

namespace Ninject.Extensions.Synchronization
{
    /// <summary>
    /// Adds functionality to the kernel to support method synchronization accross threads.
    /// </summary>
    public class SynchronizationModule : StandardModule
    {
        #region Public Methods

        /// <summary>
        /// Prepares the module for being loaded. Can be used to connect component dependencies.
        /// </summary>
        public override void BeforeLoad()
        {
            if (!Kernel.Components.Has<IProxyFactory>())
            {
                throw new ActivationException(
                    string.Format(
                        "The {0} requires that an implementation of {1} be registered as a kernel component prior to the {1} being loaded.",
                        GetType().Name, typeof (IProxyFactory)));
            }
            Kernel.Components.Connect<SynchronizationExtension>(new SynchronizationExtension());
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
        }

        #endregion
    }
}