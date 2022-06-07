using System;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using System.IO;


namespace ApplicationCore.ExternalService
{

    public class FirebaseAdminMessaging
    {
        public FirebaseAdminMessaging()
        {

            string currentPath = Directory.GetCurrentDirectory();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile($"{currentPath}/firebase_admin.json")
            });
        }

        public async void SendSingleNotification(string fcmCode, Dictionary<string, string> data)
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

        public async void SendMultiNotification(List<string> registerTokens, Dictionary<string, string> data)
        {
            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = registerTokens,
                    Data = data,
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