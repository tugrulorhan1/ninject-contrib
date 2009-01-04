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

using Ninject.Core.Planning.Directives;

#endregion

namespace NinjectContrib.Synchronization.Infrastructure.Planning.Directives
{
    internal class SynchronizationDirective : DirectiveBase
    {
        /// <summary>
        /// Builds the value that uniquely identifies the directive. This is called the first time
        /// the key is accessed, and then cached in the directive.
        /// </summary>
        /// <returns>The directive's unique key.</returns>
        /// <remarks>
        /// This exists because most directives' keys are based on reading member information,
        /// especially parameters. Since it's a relatively expensive procedure, it shouldn't be
        /// done each time the key is accessed.
        /// </remarks>
        protected override object BuildKey()
        {
            return "SynchronizationDirectiveKey";
        }
    }
}