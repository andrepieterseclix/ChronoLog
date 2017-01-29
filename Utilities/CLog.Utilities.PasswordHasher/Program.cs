using CLog.Infrastructure.Contracts.Security;
using CLog.Infrastructure.Security;
using System;
using System.Windows.Forms;

namespace CLog.Utilities.PasswordHasher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //ILoginTokenHelper tokenHelper = new LoginTokenHelper();
            IPasswordHelper passwordHelper = new PasswordHelperSha256();

            while (true)
            {
                Console.Write("Please enter a password to hash:  ");
                string password = Console.ReadLine();
                string salt = passwordHelper.GetRandomSalt();
                string hash = passwordHelper.ComputeHash(password, salt);

                Clipboard.SetText(string.Format("{0}\t{1}", hash, salt));
                Console.WriteLine("Copied to clipboard\r\n{0}\r\n{1}\r\n", hash, salt);
            }
        }
    }
}
