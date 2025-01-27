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

using PDS.WITSMLstudio.Compatibility;
using PDS.WITSMLstudio.Store.Configuration;

namespace PDS.WITSMLstudio.Store.Data.Trajectories
{
    /// <summary>
    /// Trajectory131TestBase
    /// </summary>
    public partial class Trajectory131TestBase
    {
        partial void BeforeEachTest()
        {
            Trajectory.ServiceCompany = "Service Company T";
        }

        partial void AfterEachTest()
        {
            CompatibilitySettings.AllowDuplicateNonRecurringElements = DevKitAspect.DefaultAllowDuplicateNonRecurringElements;
            CompatibilitySettings.TrajectoryAllowPutObjectWithData = DevKitAspect.DefaultTrajectoryAllowPutObjectWithData;
            CompatibilitySettings.UnknownElementSetting = DevKitAspect.DefaultUnknownElementSetting;

            WitsmlSettings.MaxStationCount = DevKitAspect.DefaultMaxStationCount;
            WitsmlSettings.TrajectoryMaxDataNodesGet = DevKitAspect.DefaultTrajectoryMaxDataNodesGet;
            WitsmlSettings.TrajectoryMaxDataNodesAdd = DevKitAspect.DefaultTrajectoryMaxDataNodesAdd;
            WitsmlSettings.TrajectoryMaxDataNodesUpdate = DevKitAspect.DefaultTrajectoryMaxDataNodesUpdate;
            WitsmlSettings.TrajectoryMaxDataNodesDelete = DevKitAspect.DefaultTrajectoryMaxDataNodesDelete;
            WitsmlSettings.TrajectoryGrowingTimeoutPeriod = DevKitAspect.DefaultTrajectoryGrowingTimeoutPeriod;
        }

        public void TestReset(int maxStationCount)
        {
            TestCleanUp();
            TestSetUp();
            WitsmlSettings.MaxStationCount = maxStationCount;
        }
    }
}
