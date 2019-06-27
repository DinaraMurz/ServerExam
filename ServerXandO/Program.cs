using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerXandO
{
    class Program
    {
        static void Main(string[] args)
        {
            //UDP сервер
            Socket srvUdp =
                new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);
            //IP-адрес и порт UDP-сервера
            string ipServer = "0.0.0.0";
            int port = 12345;
            //конечная точка, End point Сервера
            EndPoint srvEP =
                new IPEndPoint(IPAddress.Parse(ipServer),
                port);
            //Привязка сокета с конечной точкой
            srvUdp.Bind(srvEP);
            Console.WriteLine("Сервер запущен: " +
                srvEP.ToString()); //0.0.0.0:12345
            byte[] buf = new byte[64 * 1024]; //64 Kb
            EndPoint clientEP1 = new IPEndPoint(0, 0);
            EndPoint clientEP2 = new IPEndPoint(0, 0);
            EndPoint tempEP = new IPEndPoint(0, 0);
            int rsize = srvUdp.ReceiveFrom(buf, ref clientEP1);
            string name1 = Encoding.UTF8.GetString(buf, 0, rsize);
            srvUdp.SendTo(Encoding.UTF8.GetBytes("first"), clientEP1);

            Console.WriteLine("name 1 "+name1 + ";" +clientEP1.ToString());
            rsize = srvUdp.ReceiveFrom(buf, ref clientEP2);
            string name2 = Encoding.UTF8.GetString(buf, 0, rsize);
            Console.WriteLine("name 2 " + name2 + ";" + clientEP2.ToString());
            srvUdp.SendTo(Encoding.UTF8.GetBytes("second"),  clientEP2);

            Console.WriteLine("Начало игры");

            while (true)
            {
                rsize=srvUdp.ReceiveFrom(buf, ref tempEP);
                Console.WriteLine(Encoding.UTF8.GetString(buf, 0, rsize));
                if (tempEP == clientEP1)
                {
                    Console.WriteLine("Получено сообщение от клиента" + name1);
                    srvUdp.SendTo(buf, 0, rsize, SocketFlags.None, clientEP2);
                }
                else
                {
                    Console.WriteLine("Получено сообщение от клиента" + name2);
                    srvUdp.SendTo(buf, 0, rsize, SocketFlags.None, clientEP1);
                }
                //rsize = srvUdp.ReceiveFrom(buf, ref clientEP2);
                //Console.WriteLine("Получено сообщение от клиента" + name2);
                //Console.WriteLine(Encoding.UTF8.GetString(buf, 0, rsize));
                //srvUdp.SendTo(buf, clientEP1);
                //int rsize =
                //    srvUdp.ReceiveFrom(buf, ref clientEP);
                //Console.WriteLine("Получено сообщение \n" +
                //    "от клиента");
                //Console.WriteLine("size = {0}", rsize);
                //Console.WriteLine(Encoding.UTF8.GetString(buf, 0, rsize));
                //if(rsize <= 15)
                //{
                //    //регистрация
                //}
                ////srvUdp.SendTo(buf,clientEP);
            }
        }
    }
}
