﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using EnsureThat;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Health.Fhir.Core.Features.Context
{
    public class FhirRequestContext : IFhirRequestContext
    {
        private readonly string _uriString;
        private readonly string _baseUriString;
        private string _resourceType = null;
        private bool _resourceTypeSet = false;
        private Uri _uri;
        private Uri _baseUri;

        public FhirRequestContext(
            string method,
            string uriString,
            string baseUriString,
            string correlationId,
            IDictionary<string, StringValues> requestHeaders,
            IDictionary<string, StringValues> responseHeaders,
            string resourceType)
        {
            EnsureArg.IsNotNullOrWhiteSpace(method, nameof(method));
            EnsureArg.IsNotNullOrWhiteSpace(uriString, nameof(uriString));
            EnsureArg.IsNotNullOrWhiteSpace(baseUriString, nameof(baseUriString));
            EnsureArg.IsNotNullOrWhiteSpace(correlationId, nameof(correlationId));
            EnsureArg.IsNotNull(responseHeaders, nameof(responseHeaders));
            EnsureArg.IsNotNull(requestHeaders, nameof(requestHeaders));

            Method = method;
            _uriString = uriString;
            _baseUriString = baseUriString;
            CorrelationId = correlationId;
            RequestHeaders = requestHeaders;
            ResponseHeaders = responseHeaders;
            _resourceType = resourceType;

            if (!string.IsNullOrEmpty(resourceType))
            {
                _resourceTypeSet = true;
            }
        }

        public string Method { get; }

        public Uri BaseUri => _baseUri ?? (_baseUri = new Uri(_baseUriString));

        public Uri Uri => _uri ?? (_uri = new Uri(_uriString));

        public string CorrelationId { get; }

        public string RouteName { get; set; }

        public string AuditEventType { get; set; }

        public ClaimsPrincipal Principal { get; set; }

        public IDictionary<string, StringValues> RequestHeaders { get; }

        public IDictionary<string, StringValues> ResponseHeaders { get; }

        public IStorageRequestMetrics StorageRequestMetrics { get; set; }

        public string GetResourceType()
        {
            if (!_resourceTypeSet)
            {
                throw new Exception("ResourceType was not initialized yet.");
            }

            return _resourceType;
        }

        public void SetResourceType(string resourceType)
        {
            _resourceType = resourceType;
            _resourceTypeSet = true;
        }
    }
}
