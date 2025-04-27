using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class AttrCustomizeResources
{
    private static AttrCustomizeConfig _config;

    public static AttrCustomizeConfig Config
    {
        get
        {
            if (_config == null)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AttrCustomizeConfig.json");

                // 如果文件不存在，则创建一个默认的配置文件
                if (!File.Exists(filePath))
                {
                    string defaultConfigString =
                        JsonConvert.SerializeObject(AttrCustomizeConfig.DefaultConfig, Formatting.Indented);
                    File.WriteAllText(filePath, defaultConfigString);
                    _config = AttrCustomizeConfig.DefaultConfig;
                    return _config;
                }

                // 如果文件存在，则读取配置文件
                string fileContent = File.ReadAllText(filePath);
                
                string source = fileContent;
                string target = JsonConvert.SerializeObject(AttrCustomizeConfig.DefaultConfig, Formatting.Indented);

                target = MergeJson(source, target);

                _config = JsonConvert.DeserializeObject<AttrCustomizeConfig>(target) ?? AttrCustomizeConfig.DefaultConfig;

                //更新配置文件
                string copyConfigJson = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(filePath, copyConfigJson);
            }

            return _config;
        }
    }
    public static string MergeJson(string sourceJson, string targetJson)
    {
        JObject source = JObject.Parse(sourceJson);
        JObject target = JObject.Parse(targetJson);
    
        // 合并源对象到目标对象，覆盖同名属性
        target.Merge(source, new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Replace
        });
    
        return target.ToString();
    }
    public static void ResetConfig()
    {
        _config = null;
    }
}