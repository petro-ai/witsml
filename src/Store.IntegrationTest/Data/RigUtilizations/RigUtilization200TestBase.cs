﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.3
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

using Energistics.DataAccess.WITSML200;

namespace PDS.WITSMLstudio.Store.Data.RigUtilizations
{
    /// <summary>
    /// RigUtilization200TestBase
    /// </summary>
    public partial class RigUtilization200TestBase
    {
        public Rig Rig { get; set; }

        partial void BeforeEachTest()
        {
            Rig = new Rig
            {
                Uuid = DevKit.Uid(),
                Citation = DevKit.Citation("Rig"),
                SchemaVersion = "2.0"
            };

            DevKit.AddAndAssert(Rig);

            RigUtilization.Rig = DevKit.DataObjectReference(Rig);
        }
    }
}
