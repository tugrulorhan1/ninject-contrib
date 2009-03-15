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
using System.Windows.Forms;
using NinjectContrib.Synchronization.Extensions;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure
{
    internal class SynchronizationMetaData : ICloneable
    {
        public SynchronizationMetaData( Type type )
        {
            if ( type == null )
            {
                throw new ArgumentNullException( "type" );
            }

            ImplementationType = type;

            SupportsImplicitSynchronization = typeof (ISynchronizeInvoke).IsAssignableFrom( ImplementationType );

            if ( typeof (Control).IsAssignableFrom( ImplementationType ) )
            {
                DefaultAttribute = new SynchronizeAttribute( typeof (WindowsFormsSynchronizationContext) );
            }
            else
            {
                SynchronizeAttribute[] synchronizeAttributes = ImplementationType.GetAttributes<SynchronizeAttribute>();
                Debug.Assert( synchronizeAttributes != null );
                DefaultAttribute = synchronizeAttributes.Length > 0 ? synchronizeAttributes[0] : null;
            }

            AttributeLookupTable = new Dictionary<MethodInfo, SynchronizeAttribute>();
        }

        private Dictionary<MethodInfo, SynchronizeAttribute> AttributeLookupTable { get; set; }

        public Type ImplementationType { get; private set; }

        public bool SupportsImplicitSynchronization { get; private set; }

        public SynchronizeAttribute DefaultAttribute { get; private set; }

        public MethodInfo[] Methods
        {
            get
            {
                var methods = new MethodInfo[AttributeLookupTable.Count];
                AttributeLookupTable.Keys.CopyTo( methods, 0 );
                return methods;
            }
        }

        public IEnumerable<KeyValuePair<MethodInfo, SynchronizeAttribute>> MethodSynchronizationData
        {
            get
            {
                var syncData = new List<KeyValuePair<MethodInfo, SynchronizeAttribute>>( AttributeLookupTable.Count );
                foreach ( KeyValuePair<MethodInfo, SynchronizeAttribute> keyValuePair in AttributeLookupTable )
                {
                    syncData.Add( new KeyValuePair<MethodInfo, SynchronizeAttribute>( keyValuePair.Key,
                                                                                      keyValuePair.Value ) );
                }
                return syncData;
            }
        }

        public void Add( MethodInfo method, SynchronizeAttribute attribute )
        {
            AttributeLookupTable.Add( method, attribute );
        }

        public SynchronizeAttribute this[MethodInfo methodInfo]
        {
            get
            {
                return AttributeLookupTable[methodInfo];
            }
        }

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var clone = new SynchronizationMetaData( ImplementationType );
            foreach ( KeyValuePair<MethodInfo, SynchronizeAttribute> pair in AttributeLookupTable )
            {
                clone.Add( pair.Key, pair.Value );
            }
            return clone;
        }

        #endregion
    }
}