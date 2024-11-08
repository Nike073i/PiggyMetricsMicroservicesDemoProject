﻿using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AuthService.Utils
{
    internal class EndpointRouter : IEndpointRouter
    {
        const string TOKEN_ENDPOINT = "/oauth/token";

        private readonly IEnumerable<Endpoint> _endpoints;
        private readonly IdentityServerOptions _options;
        private readonly ILogger _logger;

        public EndpointRouter(IEnumerable<Endpoint> endpoints, IdentityServerOptions options, ILogger<EndpointRouter> logger)
        {
            _endpoints = endpoints;
            _options = options;
            _logger = logger;
        }

        public IEndpointHandler Find(Microsoft.AspNetCore.Http.HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Request.Path.Equals(TOKEN_ENDPOINT, StringComparison.OrdinalIgnoreCase))
            {
                Endpoint tokenEndPoint = GetEndPoint(EndpointNames.Token);
                return GetEndpointHandler(tokenEndPoint, context);
            }
            else
            {
                foreach (Endpoint endpoint in _endpoints)
                {
                    string path = endpoint.Path;
                    if (context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                    {
                        string endpointName = endpoint.Name;
                        _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", context.Request.Path, endpointName);

                        return GetEndpointHandler(endpoint, context);
                    }
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {path}", context.Request.Path);
            return null;
        }

        private Endpoint GetEndPoint(string endPointName)
        {
            Endpoint endpoint = null;
            foreach (Endpoint ep in _endpoints)
            {
                if (ep.Name == endPointName)
                {
                    endpoint = ep;
                    break;
                }
            }
            return endpoint;
        }

        private IEndpointHandler GetEndpointHandler(Endpoint endpoint, Microsoft.AspNetCore.Http.HttpContext context)
        {
            if (_options.Endpoints.IsEndpointEnabled(endpoint))
            {
                IEndpointHandler handler = context.RequestServices.GetService(endpoint.Handler) as IEndpointHandler;
                if (handler != null)
                {
                    _logger.LogDebug("Endpoint enabled: {endpoint}, successfully created handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                    return handler;
                }
                else
                {
                    _logger.LogDebug("Endpoint enabled: {endpoint}, failed to create handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                }
            }
            else
            {
                _logger.LogWarning("Endpoint disabled: {endpoint}", endpoint.Name);
            }

            return null;
        }
    }

    internal static class EndpointNames
    {
        public const string Authorize = "Authorize";
        public const string Token = "Token";
        public const string DeviceAuthorization = "DeviceAuthorization";
        public const string Discovery = "Discovery";
        public const string Introspection = "Introspection";
        public const string Revocation = "Revocation";
        public const string EndSession = "Endsession";
        public const string CheckSession = "Checksession";
        public const string UserInfo = "Userinfo";
    }

    internal static class EndpointOptionsExtensions
    {
        internal static bool IsEndpointEnabled(this EndpointsOptions options, Endpoint endpoint)
        {
            switch (endpoint?.Name)
            {
                case EndpointNames.Authorize:
                    return options.EnableAuthorizeEndpoint;
                case EndpointNames.CheckSession:
                    return options.EnableCheckSessionEndpoint;
                case EndpointNames.Discovery:
                    return options.EnableDiscoveryEndpoint;
                case EndpointNames.EndSession:
                    return options.EnableEndSessionEndpoint;
                case EndpointNames.Introspection:
                    return options.EnableIntrospectionEndpoint;
                case EndpointNames.Revocation:
                    return options.EnableTokenRevocationEndpoint;
                case EndpointNames.Token:
                    return options.EnableTokenEndpoint;
                case EndpointNames.UserInfo:
                    return options.EnableUserInfoEndpoint;
                default:
                    // fall thru to true to allow custom endpoints
                    return true;
            }
        }
    }
}
