using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace WindowsWatchdog.Services
{
    public class RemoteIISManager
    {
        private string remoteHost;
        private string username;
        private string password;

        public RemoteIISManager(string remoteHost, string username, string password)
        {
            this.remoteHost = remoteHost;
            this.username = username;
            this.password = password;
        }

        // Create a new site
        public void CreateSite(string siteName, string bindingInfo, string physicalPath, string appPoolName)
        {
            string script = $@"
            Import-Module WebAdministration
            New-Website -Name '{siteName}' -Port 80 -PhysicalPath '{physicalPath}' -ApplicationPool '{appPoolName}'
            Get-Website | Where-Object {{ $_.Name -eq '{siteName}' }} | ForEach-Object {{ $_.Bindings.Add('*:80:{bindingInfo}', 'http') }}
        ";

            ExecuteRemoteScript(script);
        }

        // Update a site
        public void UpdateSite(string siteName, string newPhysicalPath)
        {
            string script = $@"
            Import-Module WebAdministration
            Set-ItemProperty 'IIS:\Sites\{siteName}' -Name physicalPath -Value '{newPhysicalPath}'
        ";

            ExecuteRemoteScript(script);
        }

        // Delete a site
        public void DeleteSite(string siteName)
        {
            string script = $@"
            Import-Module WebAdministration
            Remove-Website -Name '{siteName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Create a new application pool
        public void CreateAppPool(string appPoolName)
        {
            string script = $@"
            Import-Module WebAdministration
            New-WebAppPool -Name '{appPoolName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Delete an application pool
        public void DeleteAppPool(string appPoolName)
        {
            string script = $@"
            Import-Module WebAdministration
            Remove-WebAppPool -Name '{appPoolName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Start a site
        public void StartSite(string siteName)
        {
            string script = $@"
            Import-Module WebAdministration
            Start-Website -Name '{siteName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Stop a site
        public void StopSite(string siteName)
        {
            string script = $@"
            Import-Module WebAdministration
            Stop-Website -Name '{siteName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Start an application pool
        public void StartAppPool(string appPoolName)
        {
            string script = $@"
            Import-Module WebAdministration
            Start-WebAppPool -Name '{appPoolName}'
        ";

            ExecuteRemoteScript(script);
        }

        // Stop an application pool
        public void StopAppPool(string appPoolName)
        {
            string script = $@"
            Import-Module WebAdministration
            Stop-WebAppPool -Name '{appPoolName}'
        ";

            ExecuteRemoteScript(script);
        }

        private void ExecuteRemoteScript(string script)
        {
            var securePassword = new System.Security.SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            PSCredential credential = new PSCredential(username, securePassword);

            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri($"http://{remoteHost}:5985/wsman"), "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", credential);
            connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Default;

            using (Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                runspace.Open();

                using (PowerShell ps = PowerShell.Create())
                {
                    ps.Runspace = runspace;
                    ps.AddScript(script);

                    var results = ps.Invoke();

                    foreach (var result in results)
                    {
                        Console.WriteLine(result.ToString());
                    }

                    if (ps.Streams.Error.Count > 0)
                    {
                        foreach (var error in ps.Streams.Error)
                        {
                            Console.WriteLine($"Error: {error}");
                        }
                    }
                }
            }
        }
    }
}
