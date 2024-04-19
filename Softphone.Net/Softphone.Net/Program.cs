using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;

namespace Softphone.Net
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var server = new Fleck.WebSocketServer("ws://0.0.0.0:8110");
            var allSockets = new List<IWebSocketConnection>();

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Client connected!");
                    foreach (var existingSocket in allSockets)
                    {
                        existingSocket.Close();
                    }
                    allSockets.Clear();

                    allSockets.Add(socket);
                };

                socket.OnClose = () =>
                {
                    Console.WriteLine("Client disconnected!");
                    allSockets.Remove(socket);
                };

                socket.OnMessage = (message) =>
                {
                    Console.WriteLine($"Inside OnMessage Function {message}");
                    string[] parts = message.Split(' ');
                    string part1 = parts[0];
                    string part2 = parts[1];
                    string part3 = parts[2];
                    string part4 = parts[3];

                    Application.OpenForms.OfType<Form1>().ToList().ForEach(form => form.Close());

                    Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1(part1, part2, part3, part4));
                };
            });
            Application.Run();
        }
    }
}
