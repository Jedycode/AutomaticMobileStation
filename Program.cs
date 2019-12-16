using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomaticMobileStation
{
    class Program
    {
        delegate void Call(PhoneNumber n);

        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var tariffPlans = new List<ITariffPlan> { new PaidTariffPlan()};
            var billingSystem = new BillingSystem("+37529", 100100100, logger, tariffPlans);
            var atsStation = new ATS(new List<IPort>() 
            { new Port(logger), new Port(logger), new Port(logger), new Port(logger)}, logger);

            billingSystem.ConnectToATS(atsStation);
            atsStation.ConnectToBillingSystem(billingSystem);

            LocalDateTime.OnDayChanged += billingSystem.DayChanged;

            var User1 = billingSystem.ConcludeContract(new PaidTariffPlan());
            User1.Plug();
            var User2 = billingSystem.ConcludeContract(new PaidTariffPlan());
            User2.Plug();
            var User3 = billingSystem.ConcludeContract(new PaidTariffPlan());
            User3.Plug();
            var User4 = billingSystem.ConcludeContract(new PaidTariffPlan());
            User4.Plug();

            logger.WriteToLog("\n---Connection when two ports are free---");
            User1.SendRequest(User2.Number);
            User2.Answer();
            User2.TerminateConnection();

            logger.WriteToLog("\n---Try to call with unplug terminal");
            User3.UnPlug();
            User3.SendRequest(User2.Number);

            logger.WriteToLog("\n---Try to call to busy terminal");
            User3.Plug();
            User3.SendRequest(User2.Number);
            logger.WriteToLog("---\n");
            User4.SendRequest(User3.Number);
            logger.WriteToLog("---\n");
            User3.Drop();

            logger.WriteToLog("\n---Try to call to unplug terminal");
            User2.UnPlug();
            User1.SendRequest(User2.Number);

            User1.SendRequest(User3.Number);
            User3.Answer();
            User3.TerminateConnection();

            User1.SendRequest(User2.Number);
            User2.Answer();
            User2.TerminateConnection();

            User1.SendRequest(User4.Number);
            User4.Answer();
            User4.TerminateConnection();

            logger.WriteToLog("\n---Get debt of user with number " + User1.Number);
            var debt = billingSystem.GetDebtOnCurrentMoment(User1.Number);
            logger.WriteToLog(debt.ToString());

            logger.WriteToLog("\n---Try to call without money");
            User2.Plug();

            var dateOfContract = billingSystem.GetContracts.SingleOrDefault(x => x.Number == User1.Number);

            while(true)
            {
                var time = LocalDateTime.Now;
                if (time.Day >= dateOfContract.Date.Day && time.Month == dateOfContract.Date.AddMonths(2).Month)
                { 
                   break;
                }
            }
            User1.SendRequest(User2.Number);
            logger.WriteToLog("\n---Pay and try to call again");
            billingSystem.Pay(User1.Number);
            User1.SendRequest(User2.Number);
            User2.Answer();
            User2.TerminateConnection();

            

            logger.WriteToLog("\n---Get outgoing call from  " + User1.Number.GetValue);
            var info = billingSystem.GetInfoAboutCalls((x) => { return x.Source == User1.Number; });
            foreach(var call in info)
            {
                logger.WriteToLog(ObjectToLogString.ToLogString(call));
            }
            Console.ReadLine();
        }
    }
}
