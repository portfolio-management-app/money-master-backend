using System;

namespace PublicAPI.Endpoints.Account
{
    public static class MessageBuilder
    {
        public static string BuildHtmlMessage(string lang, string otp)
        {
            switch (lang)
            {
                case "en":
                    return $"Your OTP code is: <b>{otp}</b>, invalid after 5 minutes";
                case "vn":
                    return $"Mã OTP của bạn là: <b>{otp}</b>, vô hiệu lực sau 5 phút";
                default:
                    return "";
            }
        }

        public static string BuildSubject(string lang, string email)
        {
            switch (lang)
            {
                case "en":
                    return $"Reset password for {email}";
                case "vn":
                    return $"Đặt lại mật khẩu cho {email}";
                default:
                    return "";
            }
        }
    }
}