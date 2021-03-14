using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Dtos.CaptchaDtos
{
    public class CaptchaVerifyResponse
    {
        public bool Success { get; set; }

        public double Score { get; set; }

        public string Action { get; set; }

        public DateTime Challenge_Ts { get; set; }

        public string Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
