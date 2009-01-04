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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Ninject.Core;
using Ninject.Core.Behavior;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Strategies;
using NinjectContrib.Synchronization.Infrastructure.Behavior;
using NinjectContrib.Synchronization.Infrastructure.Interception;
using NinjectContrib.Synchronization.Infrastructure.Planning.Directives;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Planning
{
    public class MethodSynchronizationRegistrationStrategy : InterceptorRegistrationStrategy
    {
        private readonly SynchronizationMetaDataStore _synchronizationStore;

        public MethodSynchronizationRegistrationStrategy()
        {
            _synchronizationStore = new SynchronizationMetaDataStore();
        }

        /// <summary>
        /// Executed to build the activation plan.         
        /// </summary>
        /// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <returns>
        /// A value indicating whether to proceed or interrupt the strategy chain.
        /// </returns>
        public override StrategyResult Build( IBinding binding, Type type, IActivationPlan plan )
        {
            SynchronizationMetaData meta = _synchronizationStore.CreateMetaData( type );

            // We need to do this for the creation context.
            AttachActivationBehaviorDecorator( binding, type, plan, meta.DefaultAttribute );

            // From here below is the same as the base, but with a different attribute type.
            IEnumerable<KeyValuePair<MethodInfo, SynchronizeAttribute>> candidates = GetCandidateMethodData( type );

            //RegisterClassInterceptors(binding, type, plan, candidates);

            foreach ( KeyValuePair<MethodInfo, SynchronizeAttribute> candidate in candidates )
            {
                RegisterMethodInterceptors( binding, type, plan, candidate.Key, new[] {candidate.Value} );

                // Indicate that instances of the type should be proxied.
                if ( !plan.Directives.HasOneOrMore<SynchronizationDirective>() )
                {
                    plan.Directives.Add( new SynchronizationDirective() );
                }
            }

            return StrategyResult.Proceed;
        }

        private IEnumerable<KeyValuePair<MethodInfo, SynchronizeAttribute>> GetCandidateMethodData( Type type )
        {
            foreach (
                KeyValuePair<MethodInfo, SynchronizeAttribute> candidate in
                    _synchronizationStore[type].MethodSynchronizationData )
            {
                if ( candidate.Value != null )
                {
                    yield return candidate;
                }
            }
        }

        /// <summary>
        /// Registers static interceptors defined by attributes on the specified method.
        /// </summary>
        /// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <param name="method">The method that may be intercepted.</param>
        /// <param name="attributes">The interception attributes that apply.</param>
        protected virtual void RegisterMethodInterceptors( IBinding binding, Type type, IActivationPlan plan,
                                                           MethodInfo method,
                                                           ICollection<SynchronizeAttribute> attributes )
        {
            IAdviceFactory factory = binding.Components.AdviceFactory;
            IAdviceRegistry registry = binding.Components.AdviceRegistry;

            foreach ( SynchronizeAttribute attribute in attributes )
            {
                IAdvice advice = factory.Create( method );

                advice.Callback = CreateInterceptor; //attribute.CreateInterceptor;
                advice.Order = attribute.Order;

                registry.Register( advice );
            }
        }

        private IInterceptor CreateInterceptor( IRequest request )
        {
            Ensure.ArgumentNotNull( request, "request" );

            SynchronizeAttribute attribute =
                _synchronizationStore[request.Target.GetType()].GetSynchronizationAttributeForMethod( request.Method );

            switch ( attribute.SynchronizationProxyType )
            {
                case SynchronizationProxyType.ISynchronizeInvoke:
                {
                    if ( typeof (ISynchronizeInvoke).IsAssignableFrom( request.Context.Implementation ) )
                    {
                        return new SynchronizeInvokeInterceptor();
                    }

                    throw new InvalidOperationException(
                        "ISynchronizeInvoke interception can only be used on types that implement ISynchronizeInvoke." );
                }
                case SynchronizationProxyType.SynchronizationContext:
                    attribute.SynchronizationContext =
                        request.Kernel.Get( attribute.ContextType ) as SynchronizationContext;
                    return new SynchronizationContextInterceptor( attribute );
                default:
                    throw new InvalidOperationException(
                        "Attribute SynchronizationProxyType but be set prior to interceptor creation." );
            }
        }

        #region Bingind Behavior Manipulation

        /// <summary>
        /// Wraps the binding behavior in a decorator that will adjust the creation context
        /// of the object to match it synchronization configuration.
        /// </summary>
        /// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="synchronizationAttribute">The interception attributes that apply.</param>
        private void AttachActivationBehaviorDecorator( IBinding binding, ICustomAttributeProvider type,
                                                        IActivationPlan plan,
                                                        SynchronizeAttribute synchronizationAttribute )
        {
            if ( synchronizationAttribute == null )
            {
                return;
            }

            if ( binding.Behavior != null )
            {
                // If the binding declares a behavior, it overrides any behavior that would be read
                // via reflection.
                plan.Behavior = new BehaviorContextDecorator( binding.Behavior, synchronizationAttribute );
            }
            else
            {
                IBehavior behavior;
                var behaviorAttribute = type.GetOneAttribute<BehaviorAttribute>();

                if ( behaviorAttribute != null )
                {
                    // If a behavior attribute was defined on the implementation type, ask it to create
                    // the appropriate behavior.
                    behavior = behaviorAttribute.CreateBehavior();
                }
                else
                {
                    // If no behavior attribute was defined, create a behavior as defined by the kernel's options.
                    behavior = Activator.CreateInstance( Kernel.Options.DefaultBehaviorType ) as IBehavior;
                }
                Debug.Assert( behavior != null );

                behavior.Kernel = Kernel;

                plan.Behavior = new BehaviorContextDecorator( behavior, synchronizationAttribute );
            }
        }

        #endregion
    }
}