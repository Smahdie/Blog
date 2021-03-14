using Core.Dtos.CaptchaDtos;
using Core.Dtos.CommonDtos;
using Core.Dtos.Settings;
using Core.Interfaces.CaptchaProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.CaptchaProviders
{
    public class CaptchaManager : ICaptchaManager
    {
        private readonly GoogleReCaptchaSettings _captchaSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CaptchaManager> _logger;

        public CaptchaManager(
            IOptions<GoogleReCaptchaSettings> settings, 
            IHttpClientFactory httpClientFactory,
            ILogger<CaptchaManager> logger)
        {
            _captchaSettings = settings.Value;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<CommandResultDto> IsValidAsync(string token)
        {
            string response = string.Empty;
            try
            {
                response = await _httpClient.GetStringAsync($"{_captchaSettings.EndPoint}={_captchaSettings.ReCaptchaSecretKey}&response={token}");
                var deserializedResponse = JsonConvert.DeserializeObject<CaptchaVerifyResponse>(response);

                var success = deserializedResponse.Success || deserializedResponse.Score > 0.5;
                var message = deserializedResponse.ErrorCodes == null ? null : string.Join(',', deserializedResponse.ErrorCodes);
                if (success)
                {
                    return CommandResultDto.Successful();
                }
                _logger.LogError("Error in validating captcha. token: {token}, response: {response}", token, response);                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in validating captcha. token: {token}, response: {response}", token, response);
            }
            return CommandResultDto.Failed("خطایی در ارسال پیام رخ داده است. لطفا اندکی صبر کنید و بعد دوباره تلاش کنید.");

        }
    }
}
