using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QingFeng.Common.Plugin
{
    public class PluginManager
    {
        public static List<Assembly> GetAllAssembly(string dllName = "*.Area.dll")
        {
            var pluginpath = FindPlugin(dllName);
            var list = new List<Assembly>();
            foreach (var filename in pluginpath)
            {
                try
                {
                    var asmname = Path.GetFileNameWithoutExtension(filename);
                    if (asmname == string.Empty) continue;
                    var asm = Assembly.LoadFrom(filename);
                    list.Add(asm);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return list;
        }


        //查找所有插件的路径
        static IEnumerable<string> FindPlugin(string dllName)
        {
            var pluginpath = new List<string>();
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var dir = Path.Combine(path, "bin");
                var dllList = Directory.GetFiles(dir, dllName);
                if (dllList.Length > 0)
                {
                    pluginpath.AddRange(dllList.Select(item => Path.Combine(dir, item.Substring(dir.Length + 1))));
                }
            }
            catch
            {
                // ignored
            }
            return pluginpath;
        }

        public static void CopyFileToBin()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var sourceDir = Path.Combine(path, "Plugin");
            var backupDir = Path.Combine(path, "bin");

            var dllList = Directory.GetFiles(sourceDir, "*.dll");
            if (dllList.Length > 0)
            {
                foreach (var fName in dllList.Select(item => item.Substring(sourceDir.Length + 1)))
                {
                    File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
                }
            }
        }
    }
}
