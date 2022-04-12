using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace runas_manager
{
    internal class ImpersonatedProcess
    {
        private readonly ManualResetEvent _exited = new(false);

        public event EventHandler? Exited;
        public TextReader StandardOutput { get; private set; }
        public TextReader StandardError { get; private set; }
        public TextWriter StandardInput { get; private set; }

        public ImpersonatedProcess(IntPtr hProcess, TextWriter standardInput, TextReader standardOutput, TextReader standardError)
        {
            StandardInput = standardInput;
            StandardOutput = standardOutput;
            StandardError = standardError;
            WaitForExitAsync(hProcess);
        }
        public static ImpersonatedProcess Start(string username, string password, string domain, string executablePath)
        {
            var startInfo = new NativeMethods.StartupInfo();
            bool success;

            var securityAttributes = new NativeMethods.SecurityAttributes
            {
                bInheritHandle = true
            };

            success = NativeMethods.CreatePipe(out var hReadOut, out var hWriteOut, securityAttributes, 0);
            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            success = NativeMethods.CreatePipe(out var hReadErr, out var hWriteErr, securityAttributes, 0);
            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            success = NativeMethods.CreatePipe(out var hReadIn, out var hWriteIn, securityAttributes, 0);
            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            success = NativeMethods.SetHandleInformation(hReadOut, NativeMethods.Constants.HANDLE_FLAG_INHERIT, 0);
            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());


            // Create process
            startInfo.cb = Marshal.SizeOf(startInfo);
            startInfo.dwFlags = NativeMethods.Constants.STARTF_USESTDHANDLES;
            startInfo.hStdOutput = hWriteOut;
            startInfo.hStdError = hWriteErr;
            startInfo.hStdInput = hReadIn;

            success = NativeMethods.CreateProcessWithLogonW(
                username,
                domain,
                password,
                NativeMethods.LogonFlags.LOGON_NETCREDENTIALS_ONLY,
                null,
                executablePath,
                NativeMethods.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT,
                IntPtr.Zero,
                null,
                ref startInfo,
                out var processInfo
                );

            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            startInfo.hStdOutput.Close();
            startInfo.hStdError.Close();
            startInfo.hStdInput.Close();
            var standardOutput = new StreamReader(new FileStream(hReadOut, FileAccess.Read), Console.OutputEncoding);
            var standardError = new StreamReader(new FileStream(hReadErr, FileAccess.Read), Console.OutputEncoding);
            var standardInput = new StreamWriter(new FileStream(hWriteIn, FileAccess.Write), Console.InputEncoding);

            var ip = new ImpersonatedProcess(processInfo.hProcess, standardInput, standardOutput, standardError);

            return ip;
        }

        private void WaitForExitAsync(IntPtr hProcess)
        {
            var thr = new Thread(() =>
            {
                _ = NativeMethods.WaitForSingleObject(hProcess, NativeMethods.Constants.INFINITE);
                Exited?.Invoke(this, EventArgs.Empty);
                _exited.Set();
            });
            thr.Start();
        }

        public void WaitForExit()
        {
            WaitForExit(-1);
        }

        public bool WaitForExit(int milliseconds)
        {
            return _exited.WaitOne(milliseconds);
        }
    }
}
