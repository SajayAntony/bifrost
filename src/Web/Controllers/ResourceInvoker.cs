using System;
using System.Diagnostics;
using Web.Models;
using Web.Models.Data;

namespace Web.Controllers
{
    public class ResourceInvoker
    {
        public static object DynaminInvoke(resource resource)
        {
            if (String.IsNullOrEmpty(resource.name))
            {
                throw new ArgumentNullException("resource.name");
            }

            var type = GetType(resource.name);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);
                var method = type.GetMethod("Execute");
                if (method != null)
                {
                    var paramInfo = method.GetParameters();
                    if (paramInfo.Length == 0)
                    {
                        return method.Invoke(instance, new object[] { });
                    }
                    else if (paramInfo.Length == 1)
                    {
                        return method.Invoke(instance, new object[] { resource.parameters });
                    }

                }
            }

            throw new ArgumentException("Resource not found");
        }

        public static Type GetType(string typeName){            
            var type = Type.GetType(typeName);
            if(type != null)
            {
                return type;
            }

            if (!TypeCache.Cache.ContainsKey(typeName)) 
            {
                throw new ArgumentException("Could not locatype type " + typeName + " in " + WebApiApplication.Config.resourcesDirectory);
            }

            return TypeCache.Cache[typeName];
        }
    }
}

namespace Bridge.Commands
{
    public class WhoAmI
    {
        public string Execute()
        {
            var cmdExecutor = new CmdExecutor();
            cmdExecutor.ExecuteCommandSync("whoami");
            return cmdExecutor.Result;
        }
    }

    public class Hostname
    {
        public string Execute()
        {
            var cmdExecutor = new CmdExecutor();
            cmdExecutor.ExecuteCommandSync("hostname");
            return cmdExecutor.Result;
        }
    }

    public class CmdExecutor
    {
        public string Result { get; set; }

        public void ExecuteCommandSync(object command)
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Debug.WriteLine(result);
                this.Result = result;
            }
            catch (Exception)
            {
                // Log the exception
            }
        }
    }
}