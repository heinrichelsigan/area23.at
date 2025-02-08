using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Util
{
    public static class ExtensionsMethods
    {

        public static bool IsPhoneOrMobile(this string phoneMobile)
        {
            bool result = true;
            char[] allowed = "+0123456789 -/".ToCharArray();
            if (phoneMobile.Length <= 3)
                result = false;
            else
            {

                for (int i = 0; i < phoneMobile.Length; i++)
                {
                    char phoneChar = phoneMobile[i];
                    if (!allowed.Contains(phoneChar))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public static bool IsEmail(this string emailAddr)
        {
            return (!string.IsNullOrEmpty(emailAddr) && emailAddr.Contains('@') && emailAddr.Contains('.'));
        }

        public static bool IsMobileOrPhone(string mobilePhone)
        {
            bool result = true;
            char[] allowed = "+0123456789 -/".ToCharArray();
            if (mobilePhone.Length <= 3)
                result = false;
            else
            {

                for (int i = 0; i < mobilePhone.Length; i++)
                {
                    char phoneChar = mobilePhone[i];
                    if (!allowed.Contains(phoneChar))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

    }

}
