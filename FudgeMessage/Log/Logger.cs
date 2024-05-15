// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace FudgeMessage.Log
{
    /// <summary>
    /// Logger Description
    /// </summary>
    internal static class Logger
    {
        private static string sSource = "Fudge Message";
        private static string sLog = "Application";
        private static string s_Path = "";

        static Logger()
        {
            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);
            }
            catch (System.Exception ex)
            {
                WriteConsole(ex.Message, EventLogEntryType.Information);
            }

            s_Path = GetApplicationLogPath();
        }

        public static void Information(String sEvent)
        {
            try
            {
                LogWrite(sEvent, EventLogEntryType.Information);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information);
            }
            catch
            {
                WriteConsole(sEvent, EventLogEntryType.Information);
            }
        }

        public static void Warning(String sEvent)
        {
            try
            {
                LogWrite(sEvent, EventLogEntryType.Warning);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning);
            }
            catch
            {
                WriteConsole(sEvent, EventLogEntryType.Warning);
            }
        }

        public static void Error(System.Exception e)
        {
            Error(String.Format("Error: {0}\r\n[StackTrace]\r\n{1}", e.Message, e.StackTrace));
        }

        public static void Error(String sEvent)
        {
            try
            {
                LogWrite(sEvent, EventLogEntryType.Error);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error);
            }
            catch
            {
                WriteConsole(sEvent, EventLogEntryType.Error);
            }
        }

        private static void LogWrite(String logMessage, EventLogEntryType type)
        {
            try
            {
                var now = DateTime.Now;
                String year = now.Year.ToString();
                String month = now.Month.ToString();
                String day = now.Day.ToString();

                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                if (day.Length == 1)
                {
                    day = "0" + day;
                }

                using (StreamWriter w = File.AppendText(s_Path + "\\" + year + "-" + month + "-" + day + ".log"))
                {
                    Log(logMessage, type, w);
                }
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void WriteConsole(string logMessage, EventLogEntryType type)
        {
            String strType = "";

            if (type == EventLogEntryType.Information)
                strType = "Information";
            else if (type == EventLogEntryType.Error)
                strType = "Error";
            else if (type == EventLogEntryType.Warning)
                strType = "Warning";
            else if (type == EventLogEntryType.SuccessAudit)
                strType = "SuccessAudit";
            else if (type == EventLogEntryType.FailureAudit)
                strType = "FailureAudit";

            Console.Write("\r\nLog Entry [{0}] : ", strType);
            Console.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            Console.WriteLine("  :");
            Console.WriteLine("  :{0}", logMessage);
            Console.WriteLine("-------------------------------");
        }

        private static void Log(string logMessage, EventLogEntryType type, TextWriter txtWriter)
        {
            try
            {
                String strType = "";

                if (type == EventLogEntryType.Information)
                    strType = "Information";
                else if (type == EventLogEntryType.Error)
                    strType = "Error";
                else if (type == EventLogEntryType.Warning)
                    strType = "Warning";
                else if (type == EventLogEntryType.SuccessAudit)
                    strType = "SuccessAudit";
                else if (type == EventLogEntryType.FailureAudit)
                    strType = "FailureAudit";

                txtWriter.Write("\r\nLog Entry [{0}] : ", strType);
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        private static string GetApplicationLogPath()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    return appSettings["LogPath"] ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
            }
            catch
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}
