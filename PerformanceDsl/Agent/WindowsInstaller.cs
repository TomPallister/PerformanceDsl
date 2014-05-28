using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Agent
{
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        /// <summary>
        ///     Public Constructor for WindowsServiceInstaller.
        ///     - Put all of your Initialization code here.
        /// </summary>
        public WindowsServiceInstaller()
        {
            var serviceProcessInstaller =
                new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "TestRunnerAgent";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "TestRunnerAgent";

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}