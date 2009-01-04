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
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Creation;
using Ninject.Core.Interception;
using NinjectContrib.Synchronization.Infrastructure;
using NinjectContrib.Synchronization.Properties;

#endregion

namespace NinjectContrib.Synchronization
{
    public class SynchronizationModule : StandardModule
    {
        #region Overrides of ModuleBase<IBindingTargetSyntax,IAdviceTargetSyntax>

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            if ( !Kernel.Components.Has<IProxyFactory>() )
            {
                throw new ActivationException(
                    string.Format( CultureInfo.CurrentCulture, Resources.ModuleActivationErrorMessage01, GetType().Name,
                                   typeof (IProxyFactory) ) );
            }
            Kernel.Components.Connect( new SynchronizationExtension() );
            // The WindowsFormsSynchronizationContext should have been created by the auto install of the marshalling control.
            SynchronizationContext current = SynchronizationContext.Current;
            Trace.Assert( current != null );
            Bind<WindowsFormsSynchronizationContext>().ToProvider( new WinFormsContextProvider() );
            Bind<SynchronizationContext>().ToSelf().Using( new OnePerThreadBehavior() );
        }

        #endregion

        #region Nested type: WinFormsContextProvider

        private class WinFormsContextProvider : IProvider
        {
            #region Implementation of IProvider

            /// <summary>
            /// Determines whether the provider is compatible with the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns>
            /// <see langword="True" /> if the provider is compatible, otherwise <see langword="false" />.
            /// </returns>
            public bool IsCompatibleWith( IContext context )
            {
                return context.Service == typeof (WindowsFormsSynchronizationContext);
            }

            /// <summary>
            /// Gets the actual concrete type that will be instantiated for the provided context.
            /// </summary>
            /// <param name="context">The context in which the activation is occurring.</param>
            /// <returns>
            /// The concrete type that will be instantiated.
            /// </returns>
            public Type GetImplementationType( IContext context )
            {
                return typeof (WindowsFormsSynchronizationContext);
            }

            /// <summary>
            /// Creates a new instance of the type.
            /// </summary>
            /// <param name="context">The context in which the activation is occurring.</param>
            /// <returns>
            /// The instance of the type.
            /// </returns>
            public object Create( IContext context )
            {
                return SynchronizationContext.Current;
            }

            /// <summary>
            /// Gets the prototype of the provider. This is almost always the actual type that is returned,
            /// except in certain cases, such as generic argument inference.
            /// </summary>
            public Type Prototype
            {
                get { return typeof (WindowsFormsSynchronizationContext); }
            }

            #endregion
        }

        #endregion
    }
}