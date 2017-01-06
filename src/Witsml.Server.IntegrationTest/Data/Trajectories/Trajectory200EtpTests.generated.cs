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

using System.Threading.Tasks;
using Energistics.Common;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.DataAccess.WITSML200.ReferenceData;
using Energistics.Protocol;
using Energistics.Protocol.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Trajectories
{
    [TestClass]
    public partial class Trajectory200EtpTests : Trajectory200TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            EtpSetUp(DevKit.Container);
            BeforeEachTest();
            _server.Start();
        }

        protected override void OnTestCleanUp()
        {
            _server?.Stop();
            EtpCleanUp();
            AfterEachTest();
        }

        [TestMethod]
        public void Trajectory200_Ensure_Creates_Trajectory_With_Default_Values()
        {
            DevKit.EnsureAndAssert(Trajectory);
        }

        [TestMethod]
        public async Task Trajectory200_PutObject_Can_Add_Trajectory()
        {
            AddParents();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = Trajectory.GetUri();

            var dataObject = CreateDataObject(uri, Trajectory);

            // Wait for Open connection
            var isOpen = await _client.OpenAsync();
            Assert.IsTrue(isOpen);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check for message flag indicating No Data
            Assert.IsNotNull(args?.Header);
            Assert.AreEqual((int)MessageFlags.NoData, args.Header.MessageFlags);

            // Put Object
            await PutAndAssert(handler, dataObject);

            // Get Object
            args = await GetAndAssert(handler, uri);

            // Check Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetXml();

            var result = Parse<Trajectory>(xml);
            Assert.IsNotNull(result);
        }
    }
}