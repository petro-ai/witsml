//----------------------------------------------------------------------- 
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

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.Trajectories
{
    public abstract partial class Trajectory131TestBase : IntegrationTestBase
    {

        public const string QueryMissingNamespace = "<trajectorys version=\"1.3.1.1\"><trajectory /></trajectorys>";
        public const string QueryInvalidNamespace = "<trajectorys xmlns=\"www.witsml.org/schemas/123\" version=\"1.3.1.1\"></trajectorys>";
        public const string QueryMissingVersion = "<trajectorys xmlns=\"http://www.witsml.org/schemas/131\"></trajectorys>";
        public const string QueryEmptyRoot = "<trajectorys xmlns=\"http://www.witsml.org/schemas/131\" version=\"1.3.1.1\"></trajectorys>";
        public const string QueryEmptyObject = "<trajectorys xmlns=\"http://www.witsml.org/schemas/131\" version=\"1.3.1.1\"><trajectory /></trajectorys>";

        public const string BasicXMLTemplate = "<trajectorys xmlns=\"http://www.witsml.org/schemas/131\" version=\"1.3.1.1\"><trajectory uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\">{3}</trajectory></trajectorys>";

        public Well Well { get; set; }
        public Wellbore Wellbore { get; set; }
        public Trajectory Trajectory { get; set; }

        public DevKit131Aspect DevKit { get; set; }

        public List<Trajectory> QueryEmptyList { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit131Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version131.Value)
                .ToArray();

            Well = new Well
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Well"),

                TimeZone = DevKit.TimeZone
            };
            Wellbore = new Wellbore
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Wellbore"),

                UidWell = Well.Uid,
                NameWell = Well.Name,
                MDCurrent = new MeasuredDepthCoord(0, MeasuredDepthUom.ft)

            };
            Trajectory = new Trajectory
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Trajectory"),

                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name

            };

            QueryEmptyList = DevKit.List(new Trajectory());

            BeforeEachTest();
            OnTestSetUp();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AfterEachTest();
            OnTestCleanUp();
            DevKit.Container.Dispose();
            DevKit = null;
        }

        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected virtual void OnTestSetUp() { }

        protected virtual void OnTestCleanUp() { }

        protected virtual void AddParents()
        {

            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.AddAndAssert<WellboreList, Wellbore>(Wellbore);

        }
    }
}