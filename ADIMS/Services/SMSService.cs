using System;
using System.Text.RegularExpressions;

namespace ADIMS.Services
{
    public class SMSService
    {
        public static bool Send(string number, string text)
        {
            string username = "Miyabi";
            string apiKey = "a14a5763f51997e977c51eee745da4ab6c92e723065b23493f7d41b47eb7d4af";

            string recipients = ProcessNumber(number);
            // And of course we want our recipients to know what we really do
            string message = text;

            // Create a new instance of our awesome gateway class
            AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey);

            try
            {
                // Thats it, hit send and we'll take care of the rest

                dynamic results = gateway.sendMessage(recipients, message);

                foreach (dynamic result in results)
                {
                    Console.Write((string)result["number"] + ",");
                    Console.Write((string)result["status"] + ","); // status is either "Success" or "error message"
                    Console.Write((string)result["messageId"] + ",");
                    Console.WriteLine((string)result["cost"]);
                }
                return true;
            }
            catch (AfricasTalkingGatewayException e)
            {
                Console.WriteLine("Encountered an error: " + e.Message);
                return false;
            }
        }

        private static string ProcessNumber(string _number)
        {
            _number = Regex.Escape(Convert.ToString(_number));
            if (_number.StartsWith("0"))
            {
                _number.Remove(0, 1);
                _number = $"254{_number}";
            }
            if (_number.StartsWith("7"))
            {
                _number = $"254{_number}";
            }
            return _number;
        }
    }
}