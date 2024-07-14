using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Boutique.Infrastructure.ExternalServices {
    public class SmsService {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration) {
            _configuration = configuration;
            TwilioClient.Init(_configuration["Twilio:AccountSid"],_configuration["Twilio:AuthToken"]);
        }

        public void SendSms(string to,string message) {
            var messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber(to)) {
                From = _configuration["Twilio:FromNumber"],
                Body = message
            };
            MessageResource.Create(messageOptions);
        }
    }
}
