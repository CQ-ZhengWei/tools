using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smell.Era.Media.AutoClear
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = Task.Run(() => {
                while (true)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " 执行中……");
                    var files=Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory,"*.bak");
                    foreach (var file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if((DateTime.Now- fileInfo.LastWriteTime).TotalDays > 5)
                        {
                            fileInfo.Delete();
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " 成功删除" + fileInfo.FullName);
                        }
                    }
                    Thread.Sleep(60*1000);
                }
            });
            Task.WaitAll(task);
        }
    }
}
