using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi2_Owin_BasicAuthentication.Models;

namespace WebApi2_Owin_BasicAuthentication.Client
{
    class Program
    {
        const string ServerAddress = "http://localhost:55234/api/test";

        static void Main(string[] args)
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            Action<string> WaitForEnter = (title) =>
            {
                Console.WriteLine(title);
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
            };

            Action<string> PrintTitle = (title) =>
            {
                Console.WriteLine(new String('=', 60));
                Console.WriteLine(title);
                Console.WriteLine(new String('=', 60));
            };


            // Allow time for the web api to run
            WaitForEnter("Wait for Api to run and then press ENTER to start . . .");

            using (HttpClient client = new HttpClient())
            {
                PrintTitle("Sending request with no credentials:");
                await TryRequestAsync(client, null);

                PrintTitle("Sending request with no credentials:");
                await TryRequestAsync(client, CreateBasicCredentials("SuperAdmin", "P@assword!"));
            }


            // Terminate
            WaitForEnter("Press ENTER to exit . . .");
        }

        private static async Task TryRequestAsync(HttpClient client, AuthenticationHeaderValue authorization)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ServerAddress))
            {
                request.Headers.Authorization = authorization;
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    Console.WriteLine($"{(int)response.StatusCode} {response.ReasonPhrase}");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine(response.ReasonPhrase);
                        return;
                    }

                    Console.WriteLine();

                    UserVM user = await response.Content.ReadAsAsync<UserVM>();
                    Console.WriteLine($"UserName: {user.UserName}");
                    Console.WriteLine("Claims:");

                    foreach (ClaimVM claim in user.Claims)
                    {
                        Console.WriteLine($"{claim.Type} => {claim.Value}");
                    }
                }
                Console.WriteLine("\n");
            }
        }

        private static AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

    }
}
