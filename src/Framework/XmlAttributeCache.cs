﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Core, 2018.3
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace PDS.WITSMLstudio.Framework
{
    /// <summary>
    /// Caches XML attributes for data type members.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    public static class XmlAttributeCache<TAttribute> where TAttribute : Attribute
    {
        private static readonly ConcurrentDictionary<MemberInfo, TAttribute> _cache = new ConcurrentDictionary<MemberInfo, TAttribute>();

        /// <summary>
        /// Gets the custom attribute.  Tries to get it from the cache first and then through reflection if not in the cache.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The attribute or <c>null</c> if no such attribute exists.</returns>
        public static TAttribute GetCustomAttribute(MemberInfo member)
        {
            if (member == null)
                return null;

            TAttribute attribute;
            if (_cache.TryGetValue(member, out attribute))
                return attribute;

            attribute = member.GetCustomAttribute<TAttribute>();
            _cache[member] = attribute;

            return attribute;
        }

        /// <summary>
        /// Determines whether there is an attribute of <typeparamref name="TAttribute"/> defined on the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>
        ///   <c>true</c> if the specified member information is defined; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Includes derived types, but this is ignored for properties and events.
        /// </remarks>
        public static bool IsDefined(MemberInfo member)
        {
            return GetCustomAttribute(member) != null;
        }
    }
}
