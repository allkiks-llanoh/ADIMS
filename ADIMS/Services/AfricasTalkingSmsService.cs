
using System;
using System.Configuration;
using System.Text.RegularExpressions;
namespace ADIMS.Services
{
    public class AfricasTalkingSmsService
    {
        public readonly string _username;
        private readonly string _apiKey;
        private AfricasTalkingGateway _gateway;
        public AfricasTalkingSmsService()
        {
            _username = ConfigurationManager.AppSettings["ApiUser"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _gateway = new AfricasTalkingGateway(_username, _apiKey);


        }
        public void SendSms(string recipient, string message)
        {
            try
            {
                _gateway.sendMessage(ProcessNumber(recipient), message);

            }
            catch (AfricasTalkingGatewayException)
            {
                throw;
            }
        }
        private string ProcessNumber(string number)
        {
            number = Regex.Escape(Convert.ToString(number));
            if (number.StartsWith("0"))
            {
                number.Remove(0, 1);
                number = $"254{number}";
            }
            if (number.StartsWith("7"))
            {
                number = $"254{number}";
            }
            if (number.StartsWith("+"))
            {
                number = number.Remove(0, 1);
            }
            return number;
        }
    }

}
