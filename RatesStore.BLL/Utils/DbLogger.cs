using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RatesStore.BLL.Interfaces;
using RatesStore.BLL.Models;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;

namespace RatesStore.BLL.Utils
{
    public class DbLogger
    {
        static readonly int countToFlush = 14;
        private static readonly ConcurrentBag<Log> concurrentBuffer = new ConcurrentBag<Log>();

        //save current logs to db and clear buffer
        private static void FlushBufferAsync()
        {
            var arrayOfLofs = concurrentBuffer.ToArray();

            //clear
            lock (concurrentBuffer)
            {
                Log someItem;
                while (!concurrentBuffer.IsEmpty)
                {
                    concurrentBuffer.TryTake(out someItem);
                }
            }

            Task.Run(() =>
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Repository<Log>().AddRange(arrayOfLofs);
                    unitOfWork.Save();
                }
            });
        }

        public static void LogMessage(string message)
        {
            concurrentBuffer.Add(new Log()
            {
                LogTime = DateTime.Now,
                Type = LogType.Log,
                Message = message
            });

            if (concurrentBuffer.Count > countToFlush)
                FlushBufferAsync();            
        }

        public static void LogError(Exception ex, string prefix = "")
        {
            var message = prefix + Environment.NewLine +
                 ex.Message + Environment.NewLine + ex.StackTrace;

            concurrentBuffer.Add(new Log()
            {
                LogTime = DateTime.Now,
                Type = LogType.Error,
                Message = message
            });

            if (concurrentBuffer.Count > countToFlush)
                FlushBufferAsync();
        }
    }
}
