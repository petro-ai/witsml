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

using Energistics.DataAccess;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.Messages
{
    [TestClass]
    public partial class Message131StoreTests : Message131TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            BeforeEachTest();
        }

        protected override void OnTestCleanUp()
        {
            AfterEachTest();
        }

        [TestMethod]
        public void Message131DataAdapter_GetFromStore_Can_Get_Message()
        {
            AddParents();
            DevKit.AddAndAssert<MessageList, Message>(Message);
            DevKit.GetAndAssert<MessageList, Message>(Message);
       }

        [TestMethod]
        public void Message131DataAdapter_AddToStore_Can_Add_Message()
        {
            AddParents();
            DevKit.AddAndAssert<MessageList, Message>(Message);
        }

        [TestMethod]
        public void Message131DataAdapter_UpdateInStore_Can_Update_Message()
        {
            AddParents();
            DevKit.AddAndAssert<MessageList, Message>(Message);
            DevKit.UpdateAndAssert<MessageList, Message>(Message);
            DevKit.GetAndAssert<MessageList, Message>(Message);
        }

        [TestMethod]
        public void Message131DataAdapter_DeleteFromStore_Can_Delete_Message()
        {
            AddParents();
            DevKit.AddAndAssert<MessageList, Message>(Message);
            DevKit.DeleteAndAssert<MessageList, Message>(Message);
            DevKit.GetAndAssert<MessageList, Message>(Message, isNotNull: false);
        }

        [TestMethod]
        public void Message131WitsmlStore_GetFromStore_Can_Transform_Message()
        {
            AddParents();
            DevKit.AddAndAssert<MessageList, Message>(Message);

            // Re-initialize all capServer providers
            DevKit.Store.CapServerProviders = null;
            DevKit.Container.BuildUp(DevKit.Store);

            string typeIn, queryIn;
            var query = DevKit.List(DevKit.CreateQuery(Message));
            DevKit.SetupParameters<MessageList, Message>(query, ObjectTypes.Message, out typeIn, out queryIn);

            var options = OptionsIn.Join(OptionsIn.ReturnElements.All, OptionsIn.DataVersion.Version141);
            var request = new WMLS_GetFromStoreRequest(typeIn, queryIn, options, null);
            var response = DevKit.Store.WMLS_GetFromStore(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.XMLout));
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var result = WitsmlParser.Parse(response.XMLout);
            var version = ObjectTypes.GetVersion(result.Root);
            Assert.AreEqual(OptionsIn.DataVersion.Version141.Value, version);
        }
    }
}