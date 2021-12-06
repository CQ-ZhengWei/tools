using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasyBusinessOnline.Common
{
    public class ConfigUtils
    {
        //private static Dictionary<string, ConfigUtil> configs = new Dictionary<string, ConfigUtil>();
        ///// <summary>
        ///// 读取配置
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="filename"></param>
        ///// <param name="mode"></param>
        ///// <returns></returns>
        //public static T Read<T>(string filename, int mode = 0) where T : ConfigUtil
        //{
        //    T t = default(T);
        //    try
        //    {
        //        string path = Util2.MapPath(filename);
        //        FileInfo fileInfo = new FileInfo(path);
        //        if (!fileInfo.Exists)
        //        {
        //            throw new Exception($"file {filename} must be not exists");
        //        }
        //        //radis模式
        //        if (mode == 2)
        //        {
        //            string key = $"config_{fileInfo.LastWriteTime.Ticks}";
        //            t = RedisCacheHelper.Get<T>(key);
        //            if (t != null)
        //            {
        //                return t;
        //            }
        //            var data = File.ReadAllText(path);
        //            t = JsonConvert.DeserializeObject<T>(data);
        //            RedisCacheHelper.Add(key, t, DateTime.Now.AddDays(365));
        //            return t;
        //        }
        //        //内存模式
        //        else if (mode == 1)
        //        {
        //            string key = $"config_{fileInfo.LastWriteTime.Ticks}";
        //            if (configs.TryGetValue(key, out ConfigUtil configUtil))
        //            {
        //                t = (T)configUtil;
        //                return t;
        //            }
        //            var data = File.ReadAllText(path);
        //            t = JsonConvert.DeserializeObject<T>(data);
        //            configs.Add(key, t);
        //            return t;
        //        }
        //        //IO模式
        //        else
        //        {
        //            var data = File.ReadAllText(path);
        //            t = JsonConvert.DeserializeObject<T>(data);
        //            return t;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        if (t != null)
        //        {
        //            t.filename= filename;
        //        }
        //    }
        //}

        //public static bool Save<T>(T configUtil)
        //{
        //    try
        //    {
        //        var str = JsonConvert.SerializeObject(configUtil.config);
        //        string path = Util2.MapPath(configUtil.filename);
        //        File.WriteAllText(path, str);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }

    public class ConfigUtil
    {


        //private static Dictionary<string, ConfigUtil> configs = new Dictionary<string, ConfigUtil>();
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        protected static T Read<T>(string filename, int mode,bool create=false) where T : ConfigUtil
        {
            T t = default(T);
            try
            {
                string path = Utils.MapPath(filename);
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    //IO模式
                    var data = File.ReadAllText(path);
                    t = JsonConvert.DeserializeObject<T>(data);
                }
                else
                {
                    if (create)
                    {
                        Type type = typeof(T);
                        t = (T)Activator.CreateInstance(type);
                        t.filename = filename;
                        t.Save();
                    }
                }
                return t;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (t != null)
                {
                    t.filename = filename;
                }
            }
        }

        public bool Save()
        {
            try
            {
                var str = JsonConvert.SerializeObject(this);
                string path = Utils.MapPath(filename);
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    DirectoryInfo directoryInfo = fileInfo.Directory;
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                }
                File.WriteAllText(path, str);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }




        [JsonIgnore]
        public string filename { get; protected set; }
    }
}
