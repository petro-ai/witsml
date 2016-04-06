﻿//----------------------------------------------------------------------- 
// ETP DevKit, 1.0
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

using System.Collections.Generic;
using Avro.IO;
using Energistics.Common;
using Energistics.Datatypes;
using Energistics.Datatypes.ChannelData;

namespace Energistics.Protocol.ChannelStreaming
{
    public class ChannelStreamingProducerHandler : EtpProtocolHandler, IChannelStreamingProducer
    {
        public const string SimpleStreamer = "SimpleStreamer";
        public const string DefaultUri = "DefaultUri";

        public ChannelStreamingProducerHandler() : base(Protocols.ChannelStreaming, "producer", "consumer")
        {
        }

        public bool IsSimpleStreamer { get; set; }

        public string DefaultDescribeUri { get; set; }

        public int MaxDataItems { get; private set; }

        public int MaxMessageRate { get; private set; }

        public override IDictionary<string, DataValue> GetCapabilities()
        {
            var capabilities = base.GetCapabilities();

            if (IsSimpleStreamer)
                capabilities[SimpleStreamer] = new DataValue() { Item = true };

            if (!string.IsNullOrWhiteSpace(DefaultDescribeUri))
                capabilities[DefaultUri] = new DataValue() { Item = DefaultDescribeUri };

            return capabilities;
        }

        public virtual void ChannelMetadata(MessageHeader request, IList<ChannelMetadataRecord> channelMetadataRecords)
        {
            var header = CreateMessageHeader(Protocols.ChannelStreaming, MessageTypes.ChannelStreaming.ChannelMetadata, request.MessageId, MessageFlags.FinalPart);

            var channelMetadata = new ChannelMetadata()
            {
                Channels = channelMetadataRecords
            };

            Session.SendMessage(header, channelMetadata);
        }

        public virtual void ChannelData(MessageHeader request, IList<DataItem> dataItems)
        {
            // NOTE: CorrelationId is only specified when responding to a ChannelRangeRequest message
            var correlationId = request == null ? 0 : request.MessageId;
            var header = CreateMessageHeader(Protocols.ChannelStreaming, MessageTypes.ChannelStreaming.ChannelData, correlationId, MessageFlags.MultiPart);

            var channelData = new ChannelData()
            {
                Data = dataItems
            };

            Session.SendMessage(header, channelData);
        }

        public virtual void ChannelDataChange(long channelId, long startIndex, long endIndex, IList<DataItem> dataItems)
        {
            var header = CreateMessageHeader(Protocols.ChannelStreaming, MessageTypes.ChannelStreaming.ChannelDataChange);

            var channelDataChange = new ChannelDataChange()
            {
                ChannelId = channelId,
                StartIndex = startIndex,
                EndIndex = endIndex,
                Data = dataItems
            };

            Session.SendMessage(header, channelDataChange);
        }

        public virtual void ChannelStatusChange(long channelId, ChannelStatuses status)
        {
            var header = CreateMessageHeader(Protocols.ChannelStreaming, MessageTypes.ChannelStreaming.ChannelStatusChange);

            var channelStatusChange = new ChannelStatusChange()
            {
                ChannelId = channelId,
                Status = status
            };

            Session.SendMessage(header, channelStatusChange);
        }

        public virtual void ChannelDelete(long channelId, string reason = null)
        {
            var header = CreateMessageHeader(Protocols.ChannelStreaming, MessageTypes.ChannelStreaming.ChannelDelete);

            var channelDelete = new ChannelDelete()
            {
                ChannelId = channelId,
                DeleteReason = reason
            };

            Session.SendMessage(header, channelDelete);
        }

        public event ProtocolEventHandler<Start> OnStart;

        public event ProtocolEventHandler<ChannelDescribe, IList<ChannelMetadataRecord>> OnChannelDescribe;

        public event ProtocolEventHandler<ChannelStreamingStart> OnChannelStreamingStart;

        public event ProtocolEventHandler<ChannelStreamingStop> OnChannelStreamingStop;

        public event ProtocolEventHandler<ChannelRangeRequest> OnChannelRangeRequest;

        protected override void HandleMessage(MessageHeader header, Decoder decoder)
        {
            switch (header.MessageType)
            {
                case (int)MessageTypes.ChannelStreaming.Start:
                    HandleStart(header, decoder.Decode<Start>());
                    break;

                case (int)MessageTypes.ChannelStreaming.ChannelDescribe:
                    HandleChannelDescribe(header, decoder.Decode<ChannelDescribe>());
                    break;

                case (int)MessageTypes.ChannelStreaming.ChannelStreamingStart:
                    HandleChannelStreamingStart(header, decoder.Decode<ChannelStreamingStart>());
                    break;

                case (int)MessageTypes.ChannelStreaming.ChannelStreamingStop:
                    HandleChannelStreamingStop(header, decoder.Decode<ChannelStreamingStop>());
                    break;

                case (int)MessageTypes.ChannelStreaming.ChannelRangeRequest:
                    HandleChannelRangeRequest(header, decoder.Decode<ChannelRangeRequest>());
                    break;

                default:
                    base.HandleMessage(header, decoder);
                    break;
            }
        }

        protected virtual void HandleStart(MessageHeader header, Start start)
        {
            MaxDataItems = start.MaxDataItems;
            MaxMessageRate = start.MaxMessageRate;
            Notify(OnStart, header, start);
        }

        protected virtual void HandleChannelDescribe(MessageHeader header, ChannelDescribe channelDescribe)
        {
            var args = Notify(OnChannelDescribe, header, channelDescribe, new List<ChannelMetadataRecord>());
            HandleChannelDescribe(args);

            ChannelMetadata(header, args.Context);
        }

        protected virtual void HandleChannelDescribe(ProtocolEventArgs<ChannelDescribe, IList<ChannelMetadataRecord>> args)
        {
        }

        protected virtual void HandleChannelStreamingStart(MessageHeader header, ChannelStreamingStart channelStreamingStart)
        {
            Notify(OnChannelStreamingStart, header, channelStreamingStart);
        }

        protected virtual void HandleChannelStreamingStop(MessageHeader header, ChannelStreamingStop channelStreamingStop)
        {
            Notify(OnChannelStreamingStop, header, channelStreamingStop);
        }

        protected virtual void HandleChannelRangeRequest(MessageHeader header, ChannelRangeRequest channelRangeRequest)
        {
            Notify(OnChannelRangeRequest, header, channelRangeRequest);
        }
    }
}
