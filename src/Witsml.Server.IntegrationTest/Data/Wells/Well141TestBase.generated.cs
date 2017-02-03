//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
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
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Wells
{
    public abstract partial class Well141TestBase : IntegrationTestBase
    {
        public const string QueryMissingNamespace = "<wells version=\"1.4.1.1\"><well /></wells>";
        public const string QueryInvalidNamespace = "<wells xmlns=\"www.witsml.org/schemas/123\" version=\"1.4.1.1\"></wells>";
        public const string QueryMissingVersion = "<wells xmlns=\"http://www.witsml.org/schemas/1series\"></wells>";
        public const string QueryEmptyRoot = "<wells xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"></wells>";
        public const string QueryEmptyObject = "<wells xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><well /></wells>";
        public const string BasicXMLTemplate = "<wells xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><well uid=\"{0}\">{1}</well></wells>";

        public Well Well { get; set; }
        public DevKit141Aspect DevKit { get; set; }
        public List<Well> QueryEmptyList { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit141Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version141.Value)
                .ToArray();

            Well = new Well
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Well"),
                TimeZone = DevKit.TimeZone
            };

            QueryEmptyList = DevKit.List(new Well());

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
        }
    }
}