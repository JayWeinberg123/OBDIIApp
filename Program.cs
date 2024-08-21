using System;
using System.Text;
using System.Threading;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Text.RegularExpressions;
using InTheHand.Net;

class Program
{
    static void Main()
    {
        
        var elm327Address = "00:1D:A5:03:F0:3C";

        try
        {
            
            var elm327Device = new BluetoothEndPoint(BluetoothAddress.Parse(elm327Address), BluetoothService.SerialPort);
            var bluetoothClient = new BluetoothClient();

            
            bluetoothClient.Connect(elm327Device);
            Console.WriteLine("Conectado ao ELM327!");

            using (var stream = bluetoothClient.GetStream())
            {
                
                var command = "09 02 5\r";
                Console.WriteLine($"Comando enviado: {command}");

                var buffer = Encoding.ASCII.GetBytes(command);
                stream.Write(buffer, 0, buffer.Length);

                
                Thread.Sleep(10000);

                
                var responseBuffer = new byte[1024];
                int bytesRead = 0, totalBytesRead = 0;

                do
                {
                    bytesRead = stream.Read(responseBuffer, totalBytesRead, responseBuffer.Length - totalBytesRead);
                    totalBytesRead += bytesRead;
                } while (bytesRead > 0 && stream.DataAvailable);

                Console.WriteLine($"Bytes lidos: {totalBytesRead}");

                
                var responseText = Encoding.ASCII.GetString(responseBuffer, 0, totalBytesRead);
                Console.WriteLine("Resposta VIN (texto): " + responseText.Trim());
                Console.WriteLine("Resposta (hex): " + BitConverter.ToString(responseBuffer, 0, totalBytesRead));

                var cleanedResponse = responseText.Replace("\r", "").Replace("\n", "").Trim();
                Console.WriteLine("Resposta limpa: " + cleanedResponse);


                //var vin = ExtractVin(cleanedResponse);
                //if (!string.IsNullOrEmpty(vin))
                //{
                //    Console.WriteLine("VIN Extraído: " + vin);
                //}
                //else
                //{
                //    Console.WriteLine("Não foi possível encontrar o VIN na resposta.");
                //}
            }

            bluetoothClient.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }

        Console.ReadKey();
    }

    //static string ExtractVin(string response)
    //{
        
    //    var vinMatch = Regex.Match(response, @"[A-HJ-NPR-Z0-9]{17}");

    //    if (vinMatch.Success)
    //    {
    //        return vinMatch.Value;
    //    }

        
    //    Console.WriteLine("Resposta completa para análise manual: " + response);


    //    var cleaned = Regex.Replace(response, @"[^A-HJ-NPR-Z0-9]", "");
    //    if (cleaned.Length >= 17)
    //    {
    //        var potentialVin = cleaned.Substring(0, 17);
    //        Console.WriteLine($"Possível VIN extraído: {potentialVin}");
    //        return potentialVin;
    //    }
    //    return null;
    //}
}
