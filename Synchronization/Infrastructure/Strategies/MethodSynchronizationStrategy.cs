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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Planning.Strategies;

#endregion

namespace Ninject.Extensions.Synchronization.Infrastructure.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    internal class MethodSynchronizationStrategy : InterceptorRegistrationStrategy
    {
        private readonly BindingFlags _bindingFlags = BindingFlags.Public |
                                                      BindingFlags.NonPublic |
                                                      BindingFlags.Instance |
                                                      BindingFlags.DeclaredOnly;

        /// <summary>
        /// Gets a collection of methods that may be intercepted on the specified type.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>The candidate methods.</returns>
        protected override ICollection<MethodInfo> GetCandidateMethods(Type type)
        {
            var candidates = new List<MethodInfo>();
            MethodInfo[] methods = type.GetMethods(_bindingFlags);

            // If the class has the SynchronizeAttribute then we want to synchronize all methods of that type
            // that follow the binding flags.
            bool loadAllAvailable = HasAttribute<SynchronizeAttribute>(type);

            foreach (MethodInfo method in methods)
            {
                if (method.DeclaringType == typeof (object))
                {
                    continue;
                }

                if (method.IsFinal)
                {
                    continue;
                }

                if (loadAllAvailable || HasAttribute<SynchronizeAttribute>(method))
                {
                    candidates.Add(method);
                }
            }

            return candidates;
        }

        #region Helper methods for cleaner syntax

        /// <summary>
        /// Determines whether the specified object has a particular attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <returns>
        /// 	<c>true</c> if the specified provider has attribute; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasAttribute<TAttribute>(ICustomAttributeProvider provider)
            where TAttribute : Attribute
        {
            return GetAllAttributes<TAttribute>(provider).Length != 0;
        }

        /// <summary>
        /// Gets all attributes of the specified type belonging to the target object.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <returns></returns>
        private static TAttribute[] GetAllAttributes<TAttribute>(ICustomAttributeProvider provider)
            where TAttribute : Attribute
        {
            return provider.GetCustomAttributes(typeof (TAttribute), true) as TAttribute[];
        }

        #endregion Helper methods for cleaner syntax
    }
}