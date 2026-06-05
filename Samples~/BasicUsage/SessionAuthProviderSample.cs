using System;
using System.Threading;
using System.Threading.Tasks;
using JorisHoef.APIHelper.Configuration;
using JorisHoef.APIHelper.Core;
using UnityEngine;

namespace JorisHoef.SessionHelper.APIHelper.Samples
{
    /// <summary>
    /// Minimal sample showing how to pass Session Helper tokens to APIHelper.
    /// </summary>
    public sealed class SessionAuthProviderSample : MonoBehaviour
    {
        [SerializeField] private ApiClientConfig apiClientConfig;

        private IApiClient apiClient;
        private ISessionService sessionService;

        private void Awake()
        {
            sessionService = new SessionService(
                new PlayerPrefsSessionStore("session-helper.api-helper.sample"),
                new FakeRefreshService());

            var authProvider = new SessionAuthProvider(sessionService);
            apiClient = ApiClientFactory.Create(apiClientConfig, authProvider);
        }

        private sealed class FakeRefreshService : ISessionRefreshService
        {
            public Task<SessionResult> RefreshAsync(
                SessionData currentSession,
                CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                return Task.FromResult(
                    SessionResult.Success(
                        new SessionData(
                            "sample-refreshed-access-token",
                            currentSession.RefreshToken,
                            DateTimeOffset.UtcNow.AddMinutes(15))));
            }
        }
    }
}
