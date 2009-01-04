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
using System.Reflection;

#endregion

namespace NinjectContrib.Synchronization.Extensions
{
    internal static class AttributeExtension
    {
        /// <summary>
        /// Gets all attributes of the specified type belonging to the target object.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <remarks>defaults to inherit attributes.</remarks>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>( this ICustomAttributeProvider provider )
            where TAttribute : Attribute
        {
            return provider.GetCustomAttributes( typeof (TAttribute), true ) as TAttribute[];
        }

        /// <summary>
        /// Gets all attributes of the specified type belonging to the target object.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>( this ICustomAttributeProvider provider, bool inherit )
            where TAttribute : Attribute
        {
            return provider.GetCustomAttributes( typeof (TAttribute), inherit ) as TAttribute[];
        }
    }
}