using System;
using System.Threading;
using log4net;
using log4net.Config;

namespace NewRelicInsights.Log4Net.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var Log = LogManager.GetLogger(typeof(Program));

            LogicalThreadContext.Properties["event1Type"] = "NewRelicInsightsExampleLogs";

            // only to show that the thread is always running
            while (true)
            {
                try
                {
                    var random = new Random();
                    
                    for (var i = 1; i <= 10; i++)
                    {
                        Thread.Sleep(random.Next(500, 2000));

                        var randomNumber = random.Next(0, 30);
                        switch (randomNumber)
                        {
                            case 1:
                                Log.Warn("Please verify the data uploaded recently.");
                                break;
                            case 5:
                            case 15:
                                Log.Debug("logging from Main function...");
                                break;
                            case 19:
                                throw new Exception("Something went wrong. Please contact administrator.");
                            default:
                                Log.Info($"{i} : Information goes here...");
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Test exception", ex);
                }

                Thread.Sleep(3000);
            }
        }
    }
}
