db = db.getSiblingDB('admin');

db.auth('root', 'example');

db = db.getSiblingDB('auth');

db.createUser({
    user: 'root',
    pwd: 'example',
    roles: [
        {
            role: 'dbAdmin',
            db: 'auth',
        },
        {
            role: 'readWrite',
            db: 'auth',
        }
    ]
});

db.ApiScope.insert(
    [{
        "Enabled": true,
        "Name": "ui",
        "DisplayName": "ui",
        "Description": null,
        "ShowInDiscoveryDocument": true,
        "UserClaims": [

        ],
        "Properties": {

        },
        "Required": false,
        "Emphasize": false
    },
    {
        "Enabled": true,
        "Name": "server",
        "DisplayName": "server",
        "Description": null,
        "ShowInDiscoveryDocument": true,
        "UserClaims": [

        ],
        "Properties": {

        },
        "Required": false,
        "Emphasize": false
    }],
    {
        ordered: false,
        writeConcern: {
            w: "majority",
            wtimeout: 5000,
            provenance: "getLastErrorDefaults"
        }
    }
);

db.Client.insert(
    [
        {
            "Enabled": true,
            "ClientId": "browser",
            "ProtocolType": "oidc",
            "ClientSecrets": [

            ],
            "RequireClientSecret": false,
            "ClientName": null,
            "Description": null,
            "ClientUri": null,
            "LogoUri": null,
            "RequireConsent": false,
            "AllowRememberConsent": true,
            "AllowedGrantTypes": [
                "password"
            ],
            "RequirePkce": true,
            "AllowPlainTextPkce": false,
            "RequireRequestObject": false,
            "AllowAccessTokensViaBrowser": false,
            "RedirectUris": [

            ],
            "PostLogoutRedirectUris": [

            ],
            "FrontChannelLogoutUri": null,
            "FrontChannelLogoutSessionRequired": true,
            "BackChannelLogoutUri": null,
            "BackChannelLogoutSessionRequired": true,
            "AllowOfflineAccess": false,
            "AllowedScopes": [
                "ui",
                "openid",
                "profile"
            ],
            "AlwaysIncludeUserClaimsInIdToken": false,
            "IdentityTokenLifetime": NumberInt(300),
            "AllowedIdentityTokenSigningAlgorithms": [

            ],
            "AccessTokenLifetime": NumberInt(3600),
            "AuthorizationCodeLifetime": NumberInt(300),
            "AbsoluteRefreshTokenLifetime": NumberInt(2592000),
            "SlidingRefreshTokenLifetime": NumberInt(1296000),
            "ConsentLifetime": null,
            "RefreshTokenUsage": NumberInt(1),
            "UpdateAccessTokenClaimsOnRefresh": false,
            "RefreshTokenExpiration": NumberInt(1),
            "AccessTokenType": NumberInt(0),
            "EnableLocalLogin": true,
            "IdentityProviderRestrictions": [

            ],
            "IncludeJwtId": true,
            "Claims": [

            ],
            "AlwaysSendClientClaims": false,
            "ClientClaimsPrefix": "client_",
            "PairWiseSubjectSalt": null,
            "UserSsoLifetime": null,
            "UserCodeType": null,
            "DeviceCodeLifetime": NumberInt(300),
            "AllowedCorsOrigins": [

            ],
            "Properties": {

            }
        },
        {
            "Enabled": true,
            "ClientId": "account-service",
            "ProtocolType": "oidc",
            "ClientSecrets": [
                {
                    "Description": null,
                    "Value": "Ha2FhcmPPUmNPzkHH9I\/fCzlHtvAy4RLgTBf0gspJis=",
                    "Expiration": null,
                    "Type": "SharedSecret"
                }
            ],
            "RequireClientSecret": true,
            "ClientName": null,
            "Description": null,
            "ClientUri": null,
            "LogoUri": null,
            "RequireConsent": false,
            "AllowRememberConsent": true,
            "AllowedGrantTypes": [
                "client_credentials"
            ],
            "RequirePkce": true,
            "AllowPlainTextPkce": false,
            "RequireRequestObject": false,
            "AllowAccessTokensViaBrowser": false,
            "RedirectUris": [

            ],
            "PostLogoutRedirectUris": [

            ],
            "FrontChannelLogoutUri": null,
            "FrontChannelLogoutSessionRequired": true,
            "BackChannelLogoutUri": null,
            "BackChannelLogoutSessionRequired": true,
            "AllowOfflineAccess": false,
            "AllowedScopes": [
                "server"
            ],
            "AlwaysIncludeUserClaimsInIdToken": false,
            "IdentityTokenLifetime": NumberInt(300),
            "AllowedIdentityTokenSigningAlgorithms": [

            ],
            "AccessTokenLifetime": NumberInt(3600),
            "AuthorizationCodeLifetime": NumberInt(300),
            "AbsoluteRefreshTokenLifetime": NumberInt(2592000),
            "SlidingRefreshTokenLifetime": NumberInt(1296000),
            "ConsentLifetime": null,
            "RefreshTokenUsage": NumberInt(1),
            "UpdateAccessTokenClaimsOnRefresh": false,
            "RefreshTokenExpiration": NumberInt(1),
            "AccessTokenType": NumberInt(0),
            "EnableLocalLogin": true,
            "IdentityProviderRestrictions": [

            ],
            "IncludeJwtId": true,
            "Claims": [

            ],
            "AlwaysSendClientClaims": false,
            "ClientClaimsPrefix": "client_",
            "PairWiseSubjectSalt": null,
            "UserSsoLifetime": null,
            "UserCodeType": null,
            "DeviceCodeLifetime": NumberInt(300),
            "AllowedCorsOrigins": [

            ],
            "Properties": {

            }
        },
        {
            "Enabled": true,
            "ClientId": "statistics-service",
            "ProtocolType": "oidc",
            "ClientSecrets": [
                {
                    "Description": null,
                    "Value": "4UeALo8qgpujh07mcTXdWPSbJmqUT9AK7zah0jKzUuo=",
                    "Expiration": null,
                    "Type": "SharedSecret"
                }
            ],
            "RequireClientSecret": true,
            "ClientName": null,
            "Description": null,
            "ClientUri": null,
            "LogoUri": null,
            "RequireConsent": false,
            "AllowRememberConsent": true,
            "AllowedGrantTypes": [
                "client_credentials"
            ],
            "RequirePkce": true,
            "AllowPlainTextPkce": false,
            "RequireRequestObject": false,
            "AllowAccessTokensViaBrowser": false,
            "RedirectUris": [

            ],
            "PostLogoutRedirectUris": [

            ],
            "FrontChannelLogoutUri": null,
            "FrontChannelLogoutSessionRequired": true,
            "BackChannelLogoutUri": null,
            "BackChannelLogoutSessionRequired": true,
            "AllowOfflineAccess": false,
            "AllowedScopes": [
                "server"
            ],
            "AlwaysIncludeUserClaimsInIdToken": false,
            "IdentityTokenLifetime": NumberInt(300),
            "AllowedIdentityTokenSigningAlgorithms": [

            ],
            "AccessTokenLifetime": NumberInt(3600),
            "AuthorizationCodeLifetime": NumberInt(300),
            "AbsoluteRefreshTokenLifetime": NumberInt(2592000),
            "SlidingRefreshTokenLifetime": NumberInt(1296000),
            "ConsentLifetime": null,
            "RefreshTokenUsage": NumberInt(1),
            "UpdateAccessTokenClaimsOnRefresh": false,
            "RefreshTokenExpiration": NumberInt(1),
            "AccessTokenType": NumberInt(0),
            "EnableLocalLogin": true,
            "IdentityProviderRestrictions": [

            ],
            "IncludeJwtId": true,
            "Claims": [

            ],
            "AlwaysSendClientClaims": false,
            "ClientClaimsPrefix": "client_",
            "PairWiseSubjectSalt": null,
            "UserSsoLifetime": null,
            "UserCodeType": null,
            "DeviceCodeLifetime": NumberInt(300),
            "AllowedCorsOrigins": [

            ],
            "Properties": {

            }
        },
        {
            "Enabled": true,
            "ClientId": "notification-service",
            "ProtocolType": "oidc",
            "ClientSecrets": [
                {
                    "Description": null,
                    "Value": "+2eIv/rrBAFX5ZtP5ucmY9gf7tbuiVBzc1nAyDfV+sM=",
                    "Expiration": null,
                    "Type": "SharedSecret"
                }
            ],
            "RequireClientSecret": true,
            "ClientName": null,
            "Description": null,
            "ClientUri": null,
            "LogoUri": null,
            "RequireConsent": false,
            "AllowRememberConsent": true,
            "AllowedGrantTypes": [
                "client_credentials"
            ],
            "RequirePkce": true,
            "AllowPlainTextPkce": false,
            "RequireRequestObject": false,
            "AllowAccessTokensViaBrowser": false,
            "RedirectUris": [

            ],
            "PostLogoutRedirectUris": [

            ],
            "FrontChannelLogoutUri": null,
            "FrontChannelLogoutSessionRequired": true,
            "BackChannelLogoutUri": null,
            "BackChannelLogoutSessionRequired": true,
            "AllowOfflineAccess": false,
            "AllowedScopes": [
                "server"
            ],
            "AlwaysIncludeUserClaimsInIdToken": false,
            "IdentityTokenLifetime": NumberInt(300),
            "AllowedIdentityTokenSigningAlgorithms": [

            ],
            "AccessTokenLifetime": NumberInt(3600),
            "AuthorizationCodeLifetime": NumberInt(300),
            "AbsoluteRefreshTokenLifetime": NumberInt(2592000),
            "SlidingRefreshTokenLifetime": NumberInt(1296000),
            "ConsentLifetime": null,
            "RefreshTokenUsage": NumberInt(1),
            "UpdateAccessTokenClaimsOnRefresh": false,
            "RefreshTokenExpiration": NumberInt(1),
            "AccessTokenType": NumberInt(0),
            "EnableLocalLogin": true,
            "IdentityProviderRestrictions": [

            ],
            "IncludeJwtId": true,
            "Claims": [

            ],
            "AlwaysSendClientClaims": false,
            "ClientClaimsPrefix": "client_",
            "PairWiseSubjectSalt": null,
            "UserSsoLifetime": null,
            "UserCodeType": null,
            "DeviceCodeLifetime": NumberInt(300),
            "AllowedCorsOrigins": [

            ],
            "Properties": {

            }
        },
        {
            "Enabled": true,
            "ClientId": "swagger",
            "ProtocolType": "oidc",
            "ClientSecrets": [
                {
                    "Description": null,
                    "Value": "sIooWycR7RWnkAqg0nOVxAJh5G9NZEJPUI5S7zl7\/fc=",
                    "Expiration": null,
                    "Type": "SharedSecret"
                }
            ],
            "RequireClientSecret": false,
            "ClientName": "Swagger",
            "Description": null,
            "ClientUri": null,
            "LogoUri": null,
            "RequireConsent": false,
            "AllowRememberConsent": true,
            "AllowedGrantTypes": [
                "password"
            ],
            "RequirePkce": false,
            "AllowPlainTextPkce": false,
            "RequireRequestObject": false,
            "AllowAccessTokensViaBrowser": false,
            "RedirectUris": [

            ],
            "PostLogoutRedirectUris": [

            ],
            "FrontChannelLogoutUri": null,
            "FrontChannelLogoutSessionRequired": true,
            "BackChannelLogoutUri": null,
            "BackChannelLogoutSessionRequired": true,
            "AllowOfflineAccess": false,
            "AllowedScopes": [
                "ui",
                "server"
            ],
            "AlwaysIncludeUserClaimsInIdToken": false,
            "IdentityTokenLifetime": NumberInt(300),
            "AllowedIdentityTokenSigningAlgorithms": [

            ],
            "AccessTokenLifetime": NumberInt(3600),
            "AuthorizationCodeLifetime": NumberInt(300),
            "AbsoluteRefreshTokenLifetime": NumberInt(2592000),
            "SlidingRefreshTokenLifetime": NumberInt(1296000),
            "ConsentLifetime": null,
            "RefreshTokenUsage": NumberInt(1),
            "UpdateAccessTokenClaimsOnRefresh": false,
            "RefreshTokenExpiration": NumberInt(1),
            "AccessTokenType": NumberInt(0),
            "EnableLocalLogin": true,
            "IdentityProviderRestrictions": [

            ],
            "IncludeJwtId": true,
            "Claims": [

            ],
            "AlwaysSendClientClaims": false,
            "ClientClaimsPrefix": "client_",
            "PairWiseSubjectSalt": null,
            "UserSsoLifetime": null,
            "UserCodeType": null,
            "DeviceCodeLifetime": NumberInt(300),
            "AllowedCorsOrigins": [

            ],
            "Properties": {

            }
        }
    ],
    {
        ordered: false,
        writeConcern: {
            w: "majority",
            wtimeout: 5000,
            provenance: "getLastErrorDefaults"
        }
    }
)

db.IdentityResource.insert(
    [
        {
            "_t": "OpenId",
            "Enabled": true,
            "Name": "openid",
            "DisplayName": "Your user identifier",
            "Description": null,
            "ShowInDiscoveryDocument": true,
            "UserClaims": [
                "sub"
            ],
            "Properties": {

            },
            "Required": true,
            "Emphasize": false
        },
        {
            "_t": "Profile",
            "Enabled": true,
            "Name": "profile",
            "DisplayName": "User profile",
            "Description": "Your user profile information (first name, last name, etc.)",
            "ShowInDiscoveryDocument": true,
            "UserClaims": [
                "name",
                "family_name",
                "given_name",
                "middle_name",
                "nickname",
                "preferred_username",
                "profile",
                "picture",
                "website",
                "gender",
                "birthdate",
                "zoneinfo",
                "locale",
                "updated_at"
            ],
            "Properties": {

            },
            "Required": false,
            "Emphasize": true
        }
    ],
    {
        ordered: false,
        writeConcern: {
            w: "majority",
            wtimeout: 5000,
            provenance: "getLastErrorDefaults"
        }
    }
)

db.ApiResource.insert(
    [
        {
            "Enabled": true,
            "Name": "all",
            "DisplayName": null,
            "Description": null,
            "ShowInDiscoveryDocument": true,
            "UserClaims": [

            ],
            "Properties": {

            },
            "ApiSecrets": [

            ],
            "Scopes": [
                "ui",
                "openid",
                "profile",
                "server"
            ],
            "AllowedAccessTokenSigningAlgorithms": [

            ]
        }
    ],
    {
        ordered: false,
        writeConcern: {
            w: "majority",
            wtimeout: 5000,
            provenance: "getLastErrorDefaults"
        }
    }
)

db.User.insert(
    [{
        "_id": BinData(3, "Zy3h9ydCDE+ZgrpN+0YWrg=="),
        "Created": [
            NumberLong(637580041754069199),
            NumberInt(0)
        ],
        "Updated": [
            NumberLong(637580041754070141),
            NumberInt(0)
        ],
        "Deleted": null,
        "UserName": "test",
        "PasswordHash": "$argon2id$v=19$m=65536,t=3,p=1$i78jcwwkFJToyIh4dJoL/w$deWTMeZIpTwI5GEhbpGs4M/MdXcnMlNPlmmeCMxvL6s",
        "FailedAttempts": NumberInt(0),
        "LockoutEndDate": null
    }],
    {
        ordered: false,
        writeConcern: {
            w: "majority",
            wtimeout: 5000,
            provenance: "getLastErrorDefaults"
        }
    }
);