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
using System.Diagnostics;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using NinjectContrib.Synchronization.Extensions;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure
{
    internal class SynchronizationMetaDataStore
    {
        private const BindingFlags _bindingFlags = BindingFlags.Public |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Instance |
                                                   BindingFlags.DeclaredOnly;

        private readonly IDictionary<Type, SynchronizationMetaData> _classMetaDataLookupTable;
        private readonly IDictionary<Type, SynchronizationMetaData> _compositeMetaDataLookupTable;

        public SynchronizationMetaDataStore()
            : this( true )
        {
        }

        public SynchronizationMetaDataStore( bool useImplicitSynchronization )
        {
            UseImplicitSynchronization = useImplicitSynchronization;
            _classMetaDataLookupTable = new Dictionary<Type, SynchronizationMetaData>();
            _compositeMetaDataLookupTable = new Dictionary<Type, SynchronizationMetaData>();
        }

        public SynchronizationMetaData this[ Type type ]
        {
            get
            {
                SynchronizationMetaData metaData;
                bool success = _compositeMetaDataLookupTable.TryGetValue( type, out metaData );
                return success ? metaData : CreateMetaData( type );
            }
        }

        public bool UseImplicitSynchronization { get; set; }

        public SynchronizationMetaData CreateMetaData<T>()
        {
            return CreateMetaData( typeof (T) );
        }

        public SynchronizationMetaData CreateMetaData( Type type )
        {
            if ( type == null )
            {
                throw new ArgumentNullException( "type" );
            }

            if ( _compositeMetaDataLookupTable.ContainsKey( type ) )
            {
                return _compositeMetaDataLookupTable[type];
            }

            BuildClassMetaData( type );

            return BuildHierarchichalSynchronizationMetaData( type );
        }

        private void BuildClassMetaData( Type type )
        {
            if ( _classMetaDataLookupTable.ContainsKey( type ) )
            {
                return;
            }

            _classMetaDataLookupTable.Add( type, new SynchronizationMetaData( type ) );

            if ( type != typeof (object) ) // set our recursion guard
            {
                // making this call will force the following to be called on the base classes first.
                // After all of the types in the hierarchy are processed, the method data for each will
                // be processed.
                BuildClassMetaData( type.BaseType );
            }

            BuildMethodMetaData( type );
        }

        private void BuildMethodMetaData( Type type )
        {
            SynchronizationMetaData metaData = _classMetaDataLookupTable[type];

            MethodInfo[] methods = type.GetMethods( _bindingFlags );
            foreach ( MethodInfo method in methods )
            {
                // We do not need to proxy private methods as they will be in the context of the calling method.
                // If you try to use this extension and call reflected methods, you should be euthanized.
                // We shouldn't proxy .ctors as they are activated in a different way.
                // Skip items marked as DoNotIntercept
                if ( method.IsConstructor ||
                     method.HasAttribute<DoNotInterceptAttribute>() ||
                     method.IsPrivate )
                {
                    continue;
                }

                SynchronizeAttribute[] synchronizeAttributes = method.GetAttributes<SynchronizeAttribute>();
                Debug.Assert( synchronizeAttributes != null );

                if ( synchronizeAttributes.Length != 0 )
                {
                    metaData.Add( method, synchronizeAttributes[0] );
                }
                else
                {
                    metaData.Add( method, metaData.DefaultAttribute );
                }
            }
        }

        private SynchronizationMetaData BuildHierarchichalSynchronizationMetaData( Type type )
        {
            var metaData = _classMetaDataLookupTable[type].Clone() as SynchronizationMetaData;
            MergeSynchronizationMetaData( metaData, type.BaseType );
            _compositeMetaDataLookupTable.Add( type, metaData );
            return metaData;
        }

        private void MergeSynchronizationMetaData( SynchronizationMetaData metaData, Type type )
        {
            if ( type == null )
            {
                return;
            }
            IEnumerable<KeyValuePair<MethodInfo, SynchronizeAttribute>> methodSynchronizationData =
                _classMetaDataLookupTable[type].MethodSynchronizationData;
            foreach ( KeyValuePair<MethodInfo, SynchronizeAttribute> keyValuePair in methodSynchronizationData )
            {
                metaData.Add( keyValuePair.Key, keyValuePair.Value ?? metaData.DefaultAttribute );
            }

            if ( type != typeof (object) )
            {
                MergeSynchronizationMetaData( metaData, type.BaseType );
            }
        }
    }
}