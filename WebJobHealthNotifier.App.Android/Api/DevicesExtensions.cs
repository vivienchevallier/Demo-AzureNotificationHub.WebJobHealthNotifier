// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace WebJobHealthNotifier.Api
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Devices.
    /// </summary>
    public static partial class DevicesExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='token'>
            /// </param>
            public static void Put(this IDevices operations, string id, string token)
            {
                operations.PutAsync(id, token).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='token'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task PutAsync(this IDevices operations, string id, string token, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.PutWithHttpMessagesAsync(id, token, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}