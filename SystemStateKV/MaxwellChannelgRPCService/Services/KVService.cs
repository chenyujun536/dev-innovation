
using Grpc.Core;
using LoggerLibrary.core;
using SystemStateKV;

namespace MaxwellChannelgRPCService.Services
{
    public class KvService : SystemStateKVService.SystemStateKVServiceBase
    {
        private readonly ILogger<KvService> _logger;
        private readonly IConfiguration _configuration;

        public KvService(ILogger<KvService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task<KVResult> UpdateKeyValue(KVRequest request, ServerCallContext context)
        {
            Logger.Info($"receive req {request.Key}={request.Value}");
            return Task.FromResult(new KVResult()
            {
                Message = $"{request.Key} is updated",
                Result = true
            });
        }
    }
}