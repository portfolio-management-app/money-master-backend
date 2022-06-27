using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure
{
    public class FirebaseAdminMessaging: IFireBaseAdminMessagingService
    {
        public FirebaseAdminMessaging()
        {
            var currentPath = Directory.GetCurrentDirectory();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile($"{currentPath}/firebase_admin.json")
            });
        }

        public async Task SendSingleNotification(string fcmCode, Dictionary<string, string> data)
        {
            try
            {
                var message = new Message()
                {
                    Token = fcmCode,
                    Data = data
                };
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message).ConfigureAwait(true);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendMultiNotification(List<string> registerTokens, Dictionary<string, string> data)
        {
            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = registerTokens,
                    Data = data
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
                Console.WriteLine(response.SuccessCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}