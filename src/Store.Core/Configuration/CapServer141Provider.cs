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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Witsml141 = Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using PDS.WITSMLstudio.Store.Properties;

namespace PDS.WITSMLstudio.Store.Configuration
{
    /// <summary>
    /// Provides common WITSML server capabilities for data schema version 1.4.1.1.
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Store.Configuration.CapServerProvider{CapServers}" />
    [Export(typeof(ICapServerProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CapServer141Provider : CapServerProvider<Witsml141.CapServers>
    {
        private const string Namespace141 = "http://www.witsml.org/schemas/1series";

        /// <summary>
        /// Gets the data schema version.
        /// </summary>
        /// <value>The data schema version.</value>
        public override string DataSchemaVersion
        {
            get { return OptionsIn.DataVersion.Version141.Value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IWitsml141Configuration"/> providers.
        /// </summary>
        /// <value>The collection of providers.</value>
        [ImportMany]
        public IEnumerable<IWitsml141Configuration> Providers { get; set; }

        /// <summary>
        /// Performs validation for the specified function and supplied parameters.
        /// </summary>
        public override void ValidateRequest()
        {
            var context = WitsmlOperationContext.Current;
            var request = context.Request;
            var document = context.Document;

            Logger.DebugFormat("Validating WITSML request for {0}; Function: {1}", request.ObjectType, request.Function);

            base.ValidateRequest();

            var optionsIn = OptionsIn.Parse(request.Options);

            if (request.Function == Functions.GetFromStore)
            {
                ValidateKeywords(optionsIn,
                    OptionsIn.ReturnElements.Keyword,
                    OptionsIn.MaxReturnNodes.Keyword,
                    OptionsIn.RequestLatestValues.Keyword,
                    OptionsIn.RequestPrivateGroupOnly.Keyword,
                    OptionsIn.RequestObjectSelectionCapability.Keyword,
                    OptionsIn.CompressionMethod.Keyword,
                    OptionsIn.DataVersion.Keyword,
                    OptionsIn.IntervalRangeInclusion.Keyword);
                ValidateRequestMaxReturnNodes(optionsIn);
                ValidateRequestRequestLatestValue(optionsIn);
                ValidateRequestObjectSelectionCapability(optionsIn, request.ObjectType, document);
                ValidateEmptyRootElement(request.ObjectType, document);
                ValidateReturnElements(optionsIn, request.ObjectType);
                ValidateIntervalRangeInclusion(optionsIn, request.ObjectType);
                ValidateSelectionCriteria(document);
            }
            else if (request.Function == Functions.AddToStore)
            {
                ValidateKeywords(optionsIn, OptionsIn.CompressionMethod.Keyword, OptionsIn.DataVersion.Keyword);
                ValidateCompressionMethod(optionsIn, GetCapServer().CapServer.CompressionMethod);
                ValidateEmptyRootElement(request.ObjectType, document);
                ValidateSingleChildElement(request.ObjectType, document);
            }
            else if (request.Function == Functions.UpdateInStore)
            {
                ValidateKeywords(optionsIn, OptionsIn.CompressionMethod.Keyword, OptionsIn.DataVersion.Keyword);
                ValidateCompressionMethod(optionsIn, GetCapServer().CapServer.CompressionMethod);
                ValidateEmptyRootElement(request.ObjectType, document);
                ValidateSingleChildElement(request.ObjectType, document);
            }
            else if (request.Function == Functions.DeleteFromStore)
            {
                ValidateKeywords(optionsIn, OptionsIn.CascadedDelete.Keyword, OptionsIn.DataVersion.Keyword);
                ValidateCascadedDelete(optionsIn, GetCapServer().CapServer.CascadedDelete.GetValueOrDefault());
                ValidateEmptyRootElement(request.ObjectType, document);
                ValidateSingleChildElement(request.ObjectType, document);
            }
        }

        /// <summary>
        /// Validates the namespace for a specific WITSML data schema version.
        /// </summary>
        /// <param name="document">The document.</param>
        protected override void ValidateNamespace(XDocument document)
        {
            Logger.Debug("Validating default namespace.");

            if (!Namespace141.Equals(GetNamespace(document)))
            {
                throw new WitsmlException(ErrorCodes.MissingDefaultWitsmlNamespace);
            }
        }

        /// <summary>
        /// Creates the capServers instance for a specific data schema version.
        /// </summary>
        /// <returns>The capServers instance.</returns>
        protected override Witsml141.CapServers CreateCapServer()
        {
            if (!Providers.Any())
            {
                Logger.WarnFormat("No WITSML configuration providers loaded for data schema version {0}", DataSchemaVersion);
                return null;
            }

            var capServer = new Witsml141.CapServer();

            foreach (var config in Providers)
            {
                config.GetCapabilities(capServer);
            }

            // Sort each function by data object name
            capServer.Function.ForEach(f => f.DataObject = f.DataObject?.OrderBy(x => x.Value).ToList());

            capServer.ApiVers = "1.4.1";
            capServer.SchemaVersion = DataSchemaVersion;
            capServer.SupportUomConversion = false; // TODO: update after UoM conversion implemented
            capServer.CompressionMethod = OptionsIn.CompressionMethod.None.Value; // TODO: update when compression is supported
            capServer.MaxRequestLatestValues = WitsmlSettings.MaxRequestLatestValues;
            capServer.ChangeDetectionPeriod = WitsmlSettings.ChangeDetectionPeriod;
            capServer.CascadedDelete = WitsmlSettings.IsCascadeDeleteEnabled;

            capServer.Name = WitsmlSettings.DefaultServerName;
            capServer.Version = WitsmlSettings.OverrideServerVersion;
            capServer.Description = Settings.Default.DefaultServerDescription;
            capServer.Vendor = Settings.Default.DefaultVendorName;
            capServer.Contact = new Contact()
            {
                Name = Settings.Default.DefaultContactName,
                Email = Settings.Default.DefaultContactEmail,
                Phone = Settings.Default.DefaultContactPhone
            };            

            return new Witsml141.CapServers()
            {
                CapServer = capServer,
                Version = capServer.ApiVers
            };
        }
    }
}
