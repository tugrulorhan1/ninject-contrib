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

using Ninject.Core.Activation;
using Ninject.Core.Activation.Strategies;
using NinjectContrib.Synchronization.Infrastructure.Planning.Directives;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Activation
{
    /// <summary>
    /// An activation strategy that wraps the instance that is being activated in a proxy which
    /// will synchronize method calls and property access.
    /// </summary>
    internal class SynchronizationActivationStrategy : ProxyStrategy
    {
        /// <summary>
        /// Returns a value indicating whether the instance in the specified context should be proxied.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <returns><see langword="True"/> if the instance should be proxied, otherwise <see langword="false"/>.</returns>
        protected override bool ShouldProxy( IContext context )
        {
            // If dynamic interceptors have been defined, all types will be proxied, regardless
            // of whether or not they request interceptors.
            if ( context.Binding.Components.AdviceRegistry.HasDynamicAdvice )
            {
                return true;
            }

            // Otherwise, check the type's activation plan.
            return context.Plan.Directives.HasOneOrMore<SynchronizationDirective>();
        }
    }
}