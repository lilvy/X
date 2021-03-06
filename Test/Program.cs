﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NewLife.Agent;
using NewLife.Caching;
using NewLife.Collections;
using NewLife.Log;
using NewLife.Reflection;
using NewLife.Remoting;
using NewLife.Security;
using NewLife.Serialization;
using NewLife.Threading;
using NewLife.Web;
using NewLife.Yun;
using XCode;
using XCode.DataAccessLayer;
using XCode.Membership;

namespace Test
{
    public class Program
    {
        private static void Main(String[] args)
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;

            //XTrace.Log = new NetworkLog();
            XTrace.UseConsole();
#if DEBUG
            XTrace.Debug = true;
#endif
            while (true)
            {
                var sw = Stopwatch.StartNew();
#if !DEBUG
                try
                {
#endif
                    Test3();
#if !DEBUG
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex?.GetTrue());
                }
#endif

                sw.Stop();
                Console.WriteLine("OK! 耗时 {0}", sw.Elapsed);
                //Thread.Sleep(5000);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.C) break;
            }
        }

        private static Int32 ths = 0;
        static void Test1()
        {
            //Console.Title = "SQLite极速插入测试 之 天下无贼 v2.0 " + AssemblyX.Entry.Compile.ToFullString();

            ////Console.WriteLine(DateTime.Now.ToFullString());
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //if (ths <= 0)
            //{
            //    foreach (var item in ".".AsDirectory().GetAllFiles("*.db"))
            //    {
            //        item.Delete();
            //    }

            //    //var db = "Membership.db".GetFullPath();
            //    //if (File.Exists(db)) File.Delete(db);

            //    //Console.Write("请输入线程数（推荐1）：");
            //    //ths = Console.ReadLine().ToInt();
            //    //if (ths < 1) ths = 1;
            //    ths = 1;
            //}

            ////var set = XCode.Setting.Current;
            ////set.UserParameter = true;

            ////UserOnline.Meta.Modules.Modules.Clear();
            ////Shard.Meta.ConnName = "Membership";
            //var ds = new XCode.Common.DataSimulation<DemoEntity>
            //{
            //    Log = XTrace.Log,
            //    //ds.BatchSize = 10000;
            //    Threads = ths,
            //    UseSql = true
            //};
            //ds.Run(400000);
        }

        class A
        {
            public String Name { get; set; }
            public DateTime Time { get; set; }
        }

        static void TestTimer(Object state)
        {
            XTrace.WriteLine("State={0} Timer={1} Scheduler={2}", state, TimerX.Current, TimerScheduler.Current);
        }

        static void Test2()
        {
            var addr = "容县容西镇移动营业厅";
            var city = "玉林";

            var bm = new BaiduMap();
            bm.Log = XTrace.Log;

            var gp = bm.GetGeoAsync(addr, city).Result;
            Console.WriteLine(gp);

            var geo = bm.GetGeoAsync(gp.Location).Result;
            Console.WriteLine(geo);

            //var org = bm.GetGeoAsync("新府中路1650号").Result;
            //var dst = bm.GetGeoAsync("广西容县高中").Result;
            //var dv = bm.GetDistanceAsync(org, dst).Result;
            //Console.WriteLine("{0}:\t{1}", dv.Distance, dv.Duration);

            var am = new AMap
            {
                Log = XTrace.Log
            };

            geo = am.GetGeoAsync(addr, city).Result;
            Console.WriteLine(geo);

            //var org2 = am.GetGeoAsync("新府中路1650号").Result;
            //var dst2 = am.GetGeoAsync("广西容县高中").Result;
            //dv = am.GetDistanceAsync(org2.Location, dst2.Location).Result;
            //Console.WriteLine("{0}:\t{1}", dv.Distance, dv.Duration);

            //var addrs = am.GetDistrictAsync("广西", 1, 450000).Result;
            //Console.WriteLine(addrs);
        }

        private static TimerX _timer;
        static void Test3()
        {
            //if (_timer == null) _timer = new TimerX(s =>
            //{
            //    Console.WriteLine();
            //    XTrace.WriteLine("start");
            //    Parallel.For(0, 3, k =>
            //    {
            //        Thread.Sleep(300);
            //        XTrace.WriteLine("pfor {0}", k);
            //    });
            //    XTrace.WriteLine("end");
            //}, null, 1000, 5000);

            //var list = new LinkList<Int32>();
            //list.Add(123);
            //list.Add(456);
            //list.Add(789);

            //Console.WriteLine(list.Contains(456));
            //list.Remove(456);

            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}

            var pool = new Pool<TcpClient>();
            pool.Log = XTrace.Log;
            pool.Max = 4;
            Task.Run(() =>
            {
                var st = new Stack<TcpClient>();
                for (var i = 0; i < 4; i++)
                {
                    st.Push(pool.Acquire(3000));
                    Thread.Sleep(500);
                }
                Thread.Sleep(100);
                for (var i = 0; i < 4; i++)
                {
                    pool.Release(st.Pop());
                    Thread.Sleep(500);
                }
            });
            Task.Run(() =>
            {
                Thread.Sleep(1900);
                var st = new Stack<TcpClient>();
                for (var i = 0; i < 4; i++)
                {
                    st.Push(pool.Acquire(2000));
                    Thread.Sleep(500);
                }
                Thread.Sleep(1000);
                for (var i = 0; i < 4; i++)
                {
                    pool.Release(st.Pop());
                    Thread.Sleep(500);
                }
            });
            //Parallel.For(0, 2, k =>
            //{
            //    var st = new Stack<TcpClient>();
            //    for (var i = 0; i < 10; i++)
            //    {
            //        if (st.Count == 0 || Rand.Next(2) == 0)
            //            st.Push(pool.Acquire());
            //        else
            //            pool.Release(st.Pop());

            //        Thread.Sleep(Rand.Next(200, 3000));
            //    }
            //});
        }
    }
}