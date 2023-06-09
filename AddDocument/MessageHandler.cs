﻿using Kafka.Interfaces;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Domain.Models;
using Application.Interfaces;
using Application.Constants;
using Application.Services;
using Confluent.Kafka;
using Kafka.Producer;
using Application.Interfaces.PostgresqlWrapper;
using Application.Services.PostgresqlWrapper;

namespace AddDocument
{
    public class MessageHandler : IKafkaHandler<string, AddDocumentInbound>
    {
        private readonly IKafkaProducer<string, AddDocumentOutbound> _responseProducer;
        private readonly IKafkaProducer<string, AddDocumentError> _errorResponseProducer;
        private readonly IAddDocumentService _addDocumentService;
        private readonly IAzureStorage _storage;
        private readonly IPostgresqlWrapper _postgresqlWrapper;
        public MessageHandler(
             IKafkaProducer<string, AddDocumentOutbound> responseProducer,
             IKafkaProducer<string, AddDocumentError> errorResponseProducer,
             IAddDocumentService addDocumentService,
             IAzureStorage storage,
             IPostgresqlWrapper postgresqlWrapper
            )
        {
            _addDocumentService = addDocumentService;
            _storage = storage;
            _responseProducer = responseProducer;
            _errorResponseProducer = errorResponseProducer;
            _postgresqlWrapper = postgresqlWrapper;
        }
        public Task HandleAsync(string key, AddDocumentInbound value)
        {
            try
            {
                // Here we can actually write the code to call microservices
                Console.WriteLine($"Consuming topic message with the below data\n CorrelationId: {value.CorrelationId}\n FileName: {value.FileName}\n FileSize: {value.FileSize}");

                // Get the SAS URI.
                var sasUrl = Convert.ToString(_storage.GetServiceSasUriForContainer());

                // Send correlation id and sasUrl to Kafka response topic.
                ProduceAddDocumentOutbound(value.CorrelationId, sasUrl);

                // Insert add document request details into database.
                InsertAddDocumentRecordIntoDB(value, sasUrl);

            }
            catch (Exception ex)
            {
                // Send correlation id and error message to Kafka error response topic.
                ProduceAddDocumentError(value.CorrelationId, ex.Message);
            }
            return Task.CompletedTask;
        }

        #region Private Method
        private void InsertAddDocumentRecordIntoDB(AddDocumentInbound addDocumentInbound, string? sasUrl)
        {
            Domain.Models.AddDocument addDocument = new()
            {
                CorrelationId = addDocumentInbound.CorrelationId,
                FileName = addDocumentInbound.FileName,
                TempBlobUrl = sasUrl,
                Flag = "P"  // "p" indicate pending here.

            };
            var res = _postgresqlWrapper.AddDocument(addDocument);
            //_addDocumentService.AddDocumentAsync(addDocument);
        }
        private void ProduceAddDocumentOutbound(Guid correlationId, string? sasUrl)
        {
            AddDocumentOutbound addDocumentOutbound = new()
            {
                SasUrl = sasUrl,
                CorrelationId = correlationId
            };
            var topicPart = new TopicPartition(KafkaTopics.AddDocumentOutbound, new Partition(1));
            _responseProducer.ProduceAsync(topicPart, Convert.ToString(correlationId), addDocumentOutbound);
        }
        private void ProduceAddDocumentError(Guid correlationId, string ex)
        {
            AddDocumentError addDocumentError = new()
            {
                CorrelationId = correlationId,
                Error = ex
            };
            var topicPart = new TopicPartition(KafkaTopics.AddDocumentError, new Partition(1));
            _errorResponseProducer.ProduceAsync(topicPart, Convert.ToString(correlationId), addDocumentError);
        }
        #endregion
    }


}
